using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

return Harness.Run(args);

static class Libc {
    [DllImport("libc", SetLastError = true)]
    internal static extern int openpty(out int amaster, out int aslave, IntPtr name, IntPtr termios, IntPtr winsize);
    [DllImport("libc")]
    internal static extern int close(int fd);
    [DllImport("libc", SetLastError = true)]
    internal static extern int fcntl(int fd, int cmd, int arg);
    [DllImport("libc", SetLastError = true)]
    internal static extern nint read(int fd, byte[] buf, nuint count);
    [DllImport("libc", SetLastError = true)]
    internal static extern nint write(int fd, byte[] buf, nuint count);
    [DllImport("libc", SetLastError = true)]
    internal static extern int kill(int pid, int sig);
    [DllImport("libc", SetLastError = true)]
    internal static extern int waitpid(int pid, out int status, int options);
    [DllImport("libc", SetLastError = true)]
    internal static extern int poll([In, Out] PollFd[] fds, uint nfds, int timeout);
    [DllImport("libc")]
    internal static extern int posix_spawnattr_init(byte[] attr);
    [DllImport("libc")]
    internal static extern int posix_spawnattr_setflags(byte[] attr, short flags);
    [DllImport("libc")]
    internal static extern int posix_spawnattr_destroy(byte[] attr);
    [DllImport("libc")]
    internal static extern int posix_spawn_file_actions_init(byte[] actions);
    [DllImport("libc", CharSet = CharSet.Ansi)]
    internal static extern int posix_spawn_file_actions_addchdir_np(byte[] actions, string path);
    [DllImport("libc")]
    internal static extern int posix_spawn_file_actions_adddup2(byte[] actions, int fd, int newfd);
    [DllImport("libc")]
    internal static extern int posix_spawn_file_actions_addclose(byte[] actions, int fd);
    [DllImport("libc")]
    internal static extern int posix_spawn_file_actions_destroy(byte[] actions);
    [DllImport("libc", CharSet = CharSet.Ansi)]
    internal static extern int posix_spawn(out int pid, string path, byte[] actions, byte[] attr, string?[] argv, string?[] envp);
}

[StructLayout(LayoutKind.Sequential)]
struct PollFd {
    public int Fd;
    public short Events;
    public short Revents;
}

static class Harness {
    const int Setfl = 3;
    const int Nonblock = 2048;
    const int Pollin = 1;
    const int Wnohang = 1;
    const int Sigterm = 15;
    const int Sigkill = 9;
    const int Eio = 5;
    const int Eagain = 11;
    const int Stdin = 0;
    const int Stdout = 1;
    const int Stderr = 2;
    const short Setsid = 0x80;
    const double DefaultTimeout = 20.0;
    const string Cp437High =
        "ÇüéâäàåçêëèïîìÄÅ" +
        "ÉæÆôöòûùÿÖÜ¢£¥₧ƒ" +
        "áíóúñÑªº¿⌐¬½¼¡«»" +
        "░▒▓│┤╡╢╖╕╣║╗╝╜╛┐" +
        "└┴┬├─┼╞╟╚╔╩╦╠═╬╧" +
        "╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀" +
        "αßΓπΣσµτΦΘΩδ∞φε∩" +
        "≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ ";
    static readonly JsonSerializerOptions //
        Json = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public static int Run(string[] argv) {
        List<string> positional = [];
        string? logPath = null;
        double timeout = DefaultTimeout;
        for (int i = 0; i < argv.Length; i++) {
            string arg = argv[i];
            if (!arg.StartsWith("--")) {
                positional.Add(arg);
                continue;
            }
            if (arg == "--log" || arg == "--timeout") {
                if (i + 1 >= argv.Length) {
                    return Fail($"missing value for option: {arg}");
                }
                i++;
                if (arg == "--log") {
                    logPath = argv[i];
                } else if (!double.TryParse(argv[i], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out timeout)) {
                    return Fail($"invalid timeout: {argv[i]}");
                }
                continue;
            }
            if (arg == "--pyte" || arg == "--cols" || arg == "--rows") {
                return Fail($"unsupported option: {arg}");
            }
            return Fail($"unknown option: {arg}");
        }
        if (positional.Count != 2) {
            return Fail("expected: expect-pty <executable> <scenario> --log <path> [--timeout <seconds>]");
        }
        if (logPath is null) {
            return Fail("missing required option: --log");
        }
        if (timeout <= 0) {
            return Fail("timeout must be positive");
        }
        string exe = Path.GetFullPath(positional[0]);
        string scene = Path.GetFullPath(positional[1]);
        if (!File.Exists(exe)) {
            return Fail($"executable does not exist: {exe}");
        }
        JsonDocument scenario;
        try {
            scenario = JsonDocument.Parse(File.ReadAllText(scene));
        } catch (Exception error) when (error is IOException or JsonException or UnauthorizedAccessException) {
            return Fail($"cannot read scenario: {error.Message}");
        }
        using (scenario) {
            if (!scenario.RootElement.TryGetProperty("steps", out JsonElement steps) ||
                steps.ValueKind != JsonValueKind.Array ||
                steps.GetArrayLength() == 0) {
                return Fail("scenario must contain a nonempty 'steps' array");
            }
            return Drive(exe, scene, scenario.RootElement, logPath, timeout);
        }
    }

    static int Fail(string message) {
        Console.Error.WriteLine($"expect-pty: {message}");
        Environment.Exit(2);
        return 2;
    }

    static string Decode(byte[] data, int count, string encoding) {
        if (encoding == "cp437") {
            char[] chars = new char[count];
            for (int i = 0; i < count; i++) {
                byte b = data[i];
                chars[i] = b < 0x80 ? (char)b : Cp437High[b - 0x80];
            }
            return new string(chars);
        }
        return Encoding.GetEncoding(encoding).GetString(data, 0, count);
    }

    static int Drive(string exe, string scenarioPath, JsonElement root, string logPath, double timeout) {
        string encoding = root.TryGetProperty("encoding", out JsonElement enc) ? enc.GetString() ?? "cp437" : "cp437";
        List<string> extra = root.TryGetProperty("args", out JsonElement args)
            ? args.EnumerateArray().Select(e => e.GetString() ?? "").ToList()
            : [];
        string? cwd = root.TryGetProperty("cwd", out JsonElement cwdElement) ? cwdElement.GetString() : null;
        cwd ??= Path.Combine(FindRepoRoot(), "build", "pty-native", Path.GetFileNameWithoutExtension(scenarioPath));
        Directory.CreateDirectory(cwd);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(logPath)) ?? ".");
        using StreamWriter log = new(Path.GetFullPath(logPath), false, new UTF8Encoding(false)) { NewLine = "\n" };
        Stopwatch started = Stopwatch.StartNew();
        void Record(string kind, Dictionary<string, object?> fields) {
            Dictionary<string, object?> ordered = new() {
                ["seconds"] = Math.Round(started.Elapsed.TotalSeconds, 6),
                ["event"] = kind,
            };
            foreach (KeyValuePair<string, object?> field in fields) {
                ordered[field.Key] = field.Value;
            }
            string line = JsonSerializer.Serialize(ordered, Json);
            log.WriteLine(line);
            log.Flush();
            Console.WriteLine(line);
            Console.Out.Flush();
        }
        int master = -1;
        int slave = -1;
        int child = -1;
        int? returncode = null;
        try {
            if (Libc.openpty(out master, out slave, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0) {
                throw new IOException("openpty failed");
            }
            List<string> command = [exe, .. extra];
            Record("START", new() {
                ["command"] = command,
                ["scenario"] = scenarioPath,
                ["cwd"] = cwd,
            });
            child = Spawn(command, cwd, master, slave);
            Libc.close(slave);
            slave = -1;
            Libc.fcntl(master, Setfl, Nonblock);
            int index = 0;
            string pending = "";
            foreach (JsonElement step in root.GetProperty("steps").EnumerateArray()) {
                index++;
                string label = step.TryGetProperty("label", out JsonElement labelElement) ? labelElement.GetString() ?? $"step {index}" : $"step {index}";
                if (step.TryGetProperty("expect_exit", out JsonElement exitElement) && exitElement.GetBoolean()) {
                    if (step.TryGetProperty("send", out _)) {
                        throw new InvalidOperationException($"step {index} combines expect_exit with send");
                    }
                    double exitDeadline = started.Elapsed.TotalSeconds + timeout;
                    while (true) {
                        if (PollChild(child, ref returncode)) {
                            Record("EXIT", new() {
                                ["step"] = index,
                                ["label"] = label,
                                ["returncode"] = returncode,
                            });
                            break;
                        }
                        double remaining = exitDeadline - started.Elapsed.TotalSeconds;
                        if (remaining <= 0) {
                            throw new TimeoutException($"step {index} ({label}) timed out waiting for exit");
                        }
                        if (!WaitReadable(master, remaining)) {
                            continue;
                        }
                        byte[] data = ReadChunk(master);
                        if (data.Length == 0) {
                            PollChild(child, ref returncode);
                            Record("EXIT", new() {
                                ["step"] = index,
                                ["label"] = label,
                                ["returncode"] = returncode,
                            });
                            break;
                        }
                        Record("OUTPUT", new() {
                            ["step"] = index,
                            ["label"] = label,
                            ["text"] = Decode(data, data.Length, encoding),
                            ["raw_hex"] = Convert.ToHexStringLower(data),
                        });
                    }
                    continue;
                }
                if (!step.TryGetProperty("expect", out JsonElement expectElement) || expectElement.ValueKind != JsonValueKind.String) {
                    throw new InvalidOperationException($"step {index} is missing its expect regex");
                }
                string patternText = expectElement.GetString() ?? "";
                Regex pattern;
                try {
                    pattern = new Regex(patternText);
                } catch (ArgumentException error) {
                    throw new InvalidOperationException($"step {index} has an invalid expect regex: {error.Message}");
                }
                double deadline = started.Elapsed.TotalSeconds + timeout;
                while (true) {
                    Match match = pattern.Match(pending);
                    if (match.Success) {
                        Record("MATCH", new() {
                            ["step"] = index,
                            ["label"] = label,
                            ["regex"] = patternText,
                            ["matched"] = match.Value,
                        });
                        pending = pending[(match.Index + match.Length)..];
                        break;
                    }
                    double remaining = deadline - started.Elapsed.TotalSeconds;
                    if (remaining <= 0) {
                        throw new TimeoutException($"step {index} ({label}) timed out waiting for /{patternText}/");
                    }
                    if (!WaitReadable(master, remaining)) {
                        if (PollChild(child, ref returncode) && returncode is not null) {
                            throw new InvalidOperationException($"child exited with status {returncode} while waiting for step {index} ({label})");
                        }
                        continue;
                    }
                    byte[] data = ReadChunk(master);
                    if (data.Length == 0) {
                        throw new InvalidOperationException($"child closed its PTY while waiting for step {index} ({label})");
                    }
                    string text = Decode(data, data.Length, encoding);
                    pending += text;
                    Record("OUTPUT", new() {
                        ["step"] = index,
                        ["label"] = label,
                        ["text"] = text,
                        ["raw_hex"] = Convert.ToHexStringLower(data),
                    });
                }
                if (step.TryGetProperty("send", out JsonElement sendElement)) {
                    if (sendElement.ValueKind != JsonValueKind.String) {
                        throw new InvalidOperationException($"step {index} send value must be a string");
                    }
                    string value = sendElement.GetString() ?? "";
                    byte[] payload = Encoding.UTF8.GetBytes(value);
                    bool appendNewline = !step.TryGetProperty("append_newline", out JsonElement newlineElement) || newlineElement.GetBoolean();
                    string? lineEnding = null;
                    if (appendNewline) {
                        lineEnding = root.TryGetProperty("line_ending", out JsonElement endingElement) ? endingElement.GetString() : "\n";
                        if (lineEnding != "\n" && lineEnding != "\r" && lineEnding != "\r\n") {
                            throw new InvalidOperationException("scenario line_ending must be LF, CR, or CRLF");
                        }
                        payload = [.. payload, .. Encoding.ASCII.GetBytes(lineEnding)];
                    }
                    Record("INPUT", new() {
                        ["step"] = index,
                        ["label"] = label,
                        ["text"] = value,
                        ["append_newline"] = appendNewline,
                        ["line_ending"] = lineEnding,
                        ["raw_hex"] = Convert.ToHexStringLower(payload),
                    });
                    WriteAll(master, payload);
                }
            }
            Record("SCENARIO_COMPLETE", new() { ["steps"] = index });
            return 0;
        } catch (Exception error) when (error is IOException or InvalidOperationException or TimeoutException or ArgumentException) {
            try {
                Record("FAIL", new() { ["message"] = error.Message });
            } catch (IOException) {
            }
            return 1;
        }
        finally {
            if (child > 0 && !PollChild(child, ref returncode)) {
                Libc.kill(-child, Sigterm);
                if (!WaitExit(child, 2000, ref returncode)) {
                    Libc.kill(-child, Sigkill);
                    WaitExit(child, -1, ref returncode);
                }
                Record("PROCESS_EXIT", new() { ["returncode"] = returncode });
                while (PollFdReady(master, 0)) {
                    byte[] data = ReadChunk(master);
                    if (data.Length == 0) {
                        break;
                    }
                    Record("SHUTDOWN_OUTPUT", new() {
                        ["text"] = Decode(data, data.Length, encoding),
                        ["raw_hex"] = Convert.ToHexStringLower(data),
                    });
                }
            } else if (child > 0) {
                Record("PROCESS_EXIT", new() { ["returncode"] = returncode });
            }
            if (slave >= 0) {
                Libc.close(slave);
            }
            if (master >= 0) {
                Libc.close(master);
            }
            log.Close();
        }
    }

    static string FindRepoRoot() {
        string? dir = AppContext.BaseDirectory;
        while (dir is not null) {
            if (File.Exists(Path.Combine(dir, "BombkiPro.slnx"))) {
                return dir.TrimEnd('/');
            }
            dir = Directory.GetParent(dir)?.FullName;
        }
        return Directory.GetCurrentDirectory();
    }

    static bool WaitReadable(int fd, double seconds) {
        return PollFdReady(fd, (int)Math.Max(0, Math.Ceiling(seconds * 1000)));
    }

    static bool PollFdReady(int fd, int timeoutMs) {
        PollFd[] fds = [new PollFd { Fd = fd, Events = Pollin }];
        return Libc.poll(fds, 1, timeoutMs) > 0;
    }

    static byte[] ReadChunk(int fd) {
        byte[] buffer = new byte[4096];
        nint got = Libc.read(fd, buffer, (nuint)buffer.Length);
        if (got > 0) {
            return buffer[..(int)got];
        }
        int error = Marshal.GetLastSystemError();
        if (got == 0 || error == Eio) {
            return [];
        }
        if ((int)got < 0 && error == Eagain) {
            return [];
        }
        throw new IOException($"PTY read failed: {error}");
    }

    static void WriteAll(int fd, byte[] payload) {
        int offset = 0;
        while (offset < payload.Length) {
            byte[] rest = payload[offset..];
            nint wrote = Libc.write(fd, rest, (nuint)rest.Length);
            if (wrote <= 0) {
                throw new IOException("PTY write failed");
            }
            offset += (int)wrote;
        }
    }

    static int Spawn(List<string> command, string cwd, int master, int slave) {
        byte[] attr = new byte[512];
        byte[] actions = new byte[512];
        CheckSpawn(Libc.posix_spawnattr_init(attr), "spawnattr_init");
        CheckSpawn(Libc.posix_spawnattr_setflags(attr, Setsid), "spawnattr_setflags");
        CheckSpawn(Libc.posix_spawn_file_actions_init(actions), "file_actions_init");
        CheckSpawn(Libc.posix_spawn_file_actions_addchdir_np(actions, cwd), "addchdir");
        CheckSpawn(Libc.posix_spawn_file_actions_adddup2(actions, slave, Stdin), "adddup2");
        CheckSpawn(Libc.posix_spawn_file_actions_adddup2(actions, slave, Stdout), "adddup2");
        CheckSpawn(Libc.posix_spawn_file_actions_adddup2(actions, slave, Stderr), "adddup2");
        CheckSpawn(Libc.posix_spawn_file_actions_addclose(actions, master), "addclose");
        CheckSpawn(Libc.posix_spawn_file_actions_addclose(actions, slave), "addclose");
        string?[] argv = [.. command, null];
        string?[] envp = [.. Environment.GetEnvironmentVariables().Keys.Cast<string>().Select(key => $"{key}={Environment.GetEnvironmentVariable(key)}"), null];
        int spawned = Libc.posix_spawn(out int pid, command[0], actions, attr, argv, envp);
        Libc.posix_spawn_file_actions_destroy(actions);
        Libc.posix_spawnattr_destroy(attr);
        CheckSpawn(spawned, "posix_spawn");
        return pid;
    }

    static void CheckSpawn(int result, string call) {
        if (result != 0) {
            throw new IOException($"{call} failed: {result}");
        }
    }

    static bool PollChild(int pid, ref int? returncode) {
        int status;
        int result = Libc.waitpid(pid, out status, Wnohang);
        if (result <= 0) {
            return returncode is not null;
        }
        returncode = DecodeStatus(status);
        return true;
    }

    static bool WaitExit(int pid, int timeoutMs, ref int? returncode) {
        Stopwatch watch = Stopwatch.StartNew();
        while (true) {
            if (PollChild(pid, ref returncode)) {
                return true;
            }
            if (timeoutMs >= 0 && watch.ElapsedMilliseconds >= timeoutMs) {
                return false;
            }
            Thread.Sleep(50);
        }
    }

    static int DecodeStatus(int status) {
        if ((status & 0x7f) == 0) {
            return (status >> 8) & 0xff;
        }
        return -(status & 0x7f);
    }
}
