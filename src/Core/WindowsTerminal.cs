using System.Text;


public static class WindowsTerminal {
    public static bool KeyPressed() =>
        Console.KeyAvailable;

    public static char ReadKeyRaw() =>
        Console.ReadKey(intercept: true).KeyChar;

    public static string ReadLine() {
        StringBuilder sb = new();
        while (true) {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.KeyChar == '\x3') {
                Crt.Interrupt();
            }

            if (key.Key == ConsoleKey.Enter || key.KeyChar == '\n') {
                Crt.EchoNewLine();
                return sb.ToString();
            }
            if (key.Key == ConsoleKey.UpArrow) {
                ReplaceLine(sb, History.Up(sb.ToString()));
                continue;
            }
            if (key.Key == ConsoleKey.DownArrow) {
                ReplaceLine(sb, History.Down(sb.ToString()));
                continue;
            }
            if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.Delete || key.KeyChar == '\b') {
                if (sb.Length > 0) {
                    sb.Length--;
                    Crt.EchoErase();
                }
                continue;
            }
            if (key.KeyChar == '\0') {
                continue;
            }

            sb.Append(key.KeyChar);
            Crt.EchoChar(key.KeyChar);
        }
    }

    static void ReplaceLine(StringBuilder sb, string line) {
        while (sb.Length > 0) {
            sb.Length--;
            Crt.EchoErase();
        }

        if (line.Length == 0) {
            return;
        }

        sb.Append(line);
        Crt.EchoText(line);
    }
}
