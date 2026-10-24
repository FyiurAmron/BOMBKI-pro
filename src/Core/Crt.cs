using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;


public static class Crt {
    static Stream? _stdin;
    static Termios _saved;
    static bool _hasSaved;
    static Stream Stdin =>
        _stdin ??= new FileStream(new SafeFileHandle(0, false), FileAccess.Read, 1);

    [StructLayout(LayoutKind.Sequential)]
    struct PollFd {
        public int fd;
        public int events;
        public int revents;
    }

    [DllImport("libc", SetLastError = true)]
    static extern int poll(ref PollFd fds, uint nfds, int timeout);

    [StructLayout(LayoutKind.Sequential)]
    struct Termios {
        public uint c_iflag;
        public uint c_oflag;
        public uint c_cflag;
        public uint c_lflag;
        public byte c_line;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] c_cc;
        public uint c_ispeed;
        public uint c_ospeed;
    }

    [DllImport("libc", SetLastError = true)]
    static extern int tcgetattr(int fd, out Termios termios);

    [DllImport("libc", SetLastError = true)]
    static extern int tcsetattr(int fd, int action, ref Termios termios);

    const uint ICANON = 0x0002;
    const uint ECHO = 0x0008;
    const uint ISIG = 0x0001;
    const uint OPOST = 0x0001;

    public static readonly bool IsWindows = OperatingSystem.IsWindows();

    static readonly double DelayMult = ReadDelayMult();

    static double ReadDelayMult() {
        if (!DebugEnabled()) {
            return 1;
        }

        string? raw = Environment.GetEnvironmentVariable("BOMBKI_DELAY_MULT");
        return raw is not null && double.TryParse(raw,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out double mult)
            && double.IsFinite(mult) && mult >= 0
            ? mult
            : 1;
    }

    static Crt() {
        if (IsWindows) {
            return;
        }

        if (tcgetattr(0, out Termios saved) == 0) {
            _saved = saved;
            _hasSaved = true;
            Termios raw = saved;
            raw.c_lflag &= ~(ICANON | ECHO | ISIG);
            raw.c_oflag &= ~OPOST;
            raw.c_cc[6] = 1;
            raw.c_cc[5] = 0;
            if (tcsetattr(0, 0, ref raw) == 0) {
                Termios restore = saved;
                AppDomain.CurrentDomain.ProcessExit += (_, _) =>
                    tcsetattr(0, 0, ref restore);
            }
        }
    }

    public static void ClrScr() =>
        Out.Write("\x1b[H\x1b[2J");
    public static void Sound(int tone) { }
    public static void NoSound() { }

    static int _colorState;
    static readonly Dictionary<(int, int), string> ColorDeltas = new() {
        [(0, 4)] = "\x1b[31m",
        [(0, 14)] = "\x1b[1;33m",
        [(0, 15)] = "\x1b[1m",
        [(4, 0)] = "\x1b[m",
        [(4, 14)] = "\x1b[0;1;33m",
        [(4, 15)] = "\x1b[0;1m",
        [(14, 0)] = "\x1b[m",
        [(14, 4)] = "\x1b[0;31m",
        [(14, 15)] = "\x1b[0;1m",
        [(15, 0)] = "\x1b[m",
        [(15, 4)] = "\x1b[0;31m",
        [(15, 14)] = "\x1b[0;1;33m",
    };

    public static void TextColor(int color) {
        if (color == 7) {
            color = 0;
        }

        if (color == _colorState) {
            return;
        }

        if (!ColorDeltas.TryGetValue((_colorState, color), out string? delta)) {
            throw new InvalidOperationException($"unprobed color transition {_colorState}->{color}");
        }

        _colorState = color;
        Out.Write(delta);
    }

    public static void TextBackground(int color) {
        int bits = color & 31;
        if (bits == 0 || bits > 23) {
            return;
        }

        Out.Write(bits == 8 ? "\x1b[m" : $"\x1b[0;4{bits % 8}m");
    }

    public static int ReadInt() {
        string line = ReadLn().Trim();
        return int.TryParse(line, out int value) ? value : 0;
    }
    public static void Delay(int ms) {
        int scaled = (int)(ms * DelayMult);
        if (scaled > 0) {
            Thread.Sleep(scaled);
        }
    }

    public static bool KeyPressed() =>
        IsWindows ? WinKeyPressed() : UnixKeyPressed();

    static bool UnixKeyPressed() {
        PollFd fds = new() { fd = 0, events = 1 };
        return poll(ref fds, 1, 0) > 0;
    }

    public static void RestoreTerminal() {
        if (!IsWindows && _hasSaved) {
            Termios restore = _saved;
            tcsetattr(0, 0, ref restore);
        }
    }

    public static void Interrupt() {
        RestoreTerminal();
        Environment.Exit(130);
    }

    public static char ReadKey() {
        char key = IsWindows ? WinReadKey() : UnixReadKey();
        if (key == '\x3') {
            Interrupt();
        }

        return key;
    }

    static char UnixReadKey() {
        int b = Stdin.ReadByte();
        if (b == 3) {
            Interrupt();
        }

        if (b == 27) {
            ConsumeEscape();
            return '\0';
        }
        return b < 0 ? '\0' : (char)b;
    }

    static int TryReadByte(int timeoutMs) {
        PollFd fds = new() { fd = 0, events = 1 };
        return poll(ref fds, 1, timeoutMs) <= 0 ? -1 : Stdin.ReadByte();
    }

    static char ConsumeEscape() {
        int second = TryReadByte(50);
        if (second < 0) {
            return '\0';
        }

        if (second == 'O') {
            int key = TryReadByte(50);
            return key < 0 ? '\0' : (char)key;
        }

        if (second != '[') {
            return '\0';
        }

        while (true) {
            int c = TryReadByte(50);
            if (c < 0) {
                return '\0';
            }

            if (c >= 0x40 && c <= 0x7E) {
                return (char)c;
            }
        }
    }

    public static void EchoChar(char c) =>
        Out.Write(c.ToString());
    public static void EchoText(string value) =>
        Out.Write(value);
    public static void EchoNewLine() =>
        Out.Write("\r\n");
    public static void EchoErase() =>
        Out.WriteRaw("\b \b");

    public static bool DebugEnabled() =>
        Environment.GetEnvironmentVariable("BOMBKI_DEBUG") == "2137";

    public const int HistoryLimit = 100;
    public static readonly List<string> HistoryLines = [];
    static int _historyIndex;
    static string _historyDraft = "";

    public static void BeginHistory() {
        _historyIndex = HistoryLines.Count;
        _historyDraft = "";
    }

    public static void RememberHistory(string line) {
        if (string.IsNullOrWhiteSpace(line)) {
            return;
        }

        if (HistoryLines.Count == HistoryLimit) {
            HistoryLines.RemoveAt(0);
        }

        HistoryLines.Add(line);
    }

    public static string HistoryUp(string current) {
        if (!HistoryLines.Any()) {
            return current;
        }

        if (_historyIndex == HistoryLines.Count) {
            _historyDraft = current;
        }

        if (_historyIndex > 0) {
            _historyIndex--;
        }

        return HistoryLines[_historyIndex];
    }

    public static string HistoryDown(string current) {
        if (_historyIndex >= HistoryLines.Count) {
            return current;
        }

        _historyIndex++;
        return _historyIndex < HistoryLines.Count ? HistoryLines[_historyIndex] : _historyDraft;
    }

    public static string ReadLn() {
        BeginHistory();
        string line = IsWindows ? WinReadLn() : UnixReadLn();
        RememberHistory(line);
        return StripDiacritics(line).ToUpperInvariant();
    }

    public static string StripDiacritics(string value) =>
        string.Concat(value.Normalize(NormalizationForm.FormD).Where(IsSpacing).Select(FoldStroke));

    static bool IsSpacing(char c) =>
        CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark;

    static char FoldStroke(char c) =>
        c switch {
            'ł' => 'l',
            'Ł' => 'L',
            _ => c,
        };

    static string UnixReadLn() {
        List<byte> bytes = [];
        int b;
        while ((b = Stdin.ReadByte()) >= 0 && b != '\n') {
            if (b == 3) {
                Interrupt();
            } else if (b == 27) {
                char sequence = ConsumeEscape();
                if (sequence == 'A') {
                    ReplaceLine(bytes, HistoryUp(CurrentLine(bytes)));
                } else if (sequence == 'B') {
                    ReplaceLine(bytes, HistoryDown(CurrentLine(bytes)));
                }
            } else if (b == 8 || b == 127) {
                if (bytes.Any()) {
                    EraseChar(bytes);
                    EchoErase();
                }
            } else {
                byte raw = (byte)b;
                bytes.Add(raw);
                Out.WriteByte(raw);
            }
        }
        EchoNewLine();
        return CurrentLine(bytes);
    }

    static string CurrentLine(List<byte> bytes) =>
        Encoding.UTF8.GetString([.. bytes]);

    static void ReplaceLine(List<byte> bytes, string line) {
        for (int i = 0; i < CurrentLine(bytes).Length; i++) {
            EchoErase();
        }

        bytes.Clear();
        if (line.Length == 0) {
            return;
        }

        bytes.AddRange(Encoding.UTF8.GetBytes(line));
        EchoText(line);
    }

    static void EraseChar(List<byte> bytes) {
        int length = TrailingCharLength(bytes);
        bytes.RemoveRange(bytes.Count - length, length);
    }

    static int TrailingCharLength(List<byte> bytes) {
        int length = 1;
        while (length < bytes.Count && (bytes[^length] & 0xC0) == 0x80) {
            length++;
        }
        return length;
    }

    static bool WinKeyPressed() =>
        Console.KeyAvailable;

    static char WinReadKey() =>
        Console.ReadKey(intercept: true).KeyChar;

    static string WinReadLn() {
        StringBuilder sb = new();
        while (true) {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.KeyChar == '\x3') {
                Interrupt();
            }

            if (key.Key == ConsoleKey.Enter || key.KeyChar == '\n') {
                EchoNewLine();
                return sb.ToString();
            }
            if (key.Key == ConsoleKey.UpArrow) {
                WinReplaceLine(sb, HistoryUp(sb.ToString()));
                continue;
            }
            if (key.Key == ConsoleKey.DownArrow) {
                WinReplaceLine(sb, HistoryDown(sb.ToString()));
                continue;
            }
            if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.Delete || key.KeyChar == '\b') {
                if (sb.Length > 0) {
                    sb.Length--;
                    EchoErase();
                }
                continue;
            }
            if (key.KeyChar == '\0') {
                continue;
            }

            sb.Append(key.KeyChar);
            EchoChar(key.KeyChar);
        }
    }

    static void WinReplaceLine(StringBuilder sb, string line) {
        while (sb.Length > 0) {
            sb.Length--;
            EchoErase();
        }

        if (line.Length == 0) {
            return;
        }

        sb.Append(line);
        EchoText(line);
    }
}


public static class Out {
    static readonly Stream Stdout = Crt.IsWindows
        ? Console.OpenStandardOutput()
        : new FileStream(new SafeFileHandle(1, false), FileAccess.Write);

    public static void WriteRaw(string value) {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        Stdout.Write(bytes, 0, bytes.Length);
        Stdout.Flush();
    }

    public static void WriteByte(byte value) {
        Stdout.WriteByte(value);
        Stdout.Flush();
    }

    public static void Write(string value) =>
        WriteRaw(value);

    public static void WriteLine(string value) =>
        WriteRaw($"{value}\r\n");

    public static void WriteLine() =>
        WriteRaw("\r\n");
    public static void Write(params object?[] parts) =>
        WriteRaw(string.Concat(parts));
    public static void WriteLine(params object?[] parts) =>
        WriteRaw($"{string.Concat(parts)}\r\n");
}
