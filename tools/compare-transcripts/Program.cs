using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
if (args.Length != 2) {
    Console.Error.WriteLine("usage: compare-transcripts <ref> <got>");
    return 2;
}
List<string> //
    reference = ContentLines(ReadBytes(args[0])),
    actual = ContentLines(ReadBytes(args[1]));
Console.WriteLine($"lines: {reference.Count} ref vs {actual.Count} got");
if (reference.SequenceEqual(actual)) {
    Console.WriteLine("IDENTICAL");
    return 0;
}
int //
    total = Math.Min(reference.Count, actual.Count),
    first = FindFirst(reference, actual, total);
if (first < 0) {
    string name = reference.Count > actual.Count ? "ref" : "got";
    List<string> longer = reference.Count > actual.Count ? reference : actual;
    Console.WriteLine($"DIVERGE: {name} has {Math.Abs(reference.Count - actual.Count)} extra lines from {total}:");
    foreach (string line in longer.Skip(total).Take(4)) {
        Console.WriteLine($"  {name}: {Repr(Truncate(line))}");
    }
    return 1;
}
Console.WriteLine($"DIVERGE at line {first}:");
for (int i = Math.Max(0, first - 1); i < Math.Min(total, first + 2); i++) {
    string mark = i == first ? ">>>" : "   ";
    Console.WriteLine($"  {mark} ref: {Repr(Truncate(reference[i]))}");
    Console.WriteLine($"  {mark} got: {Repr(Truncate(actual[i]))}");
}
return 1;

static byte[] ReadBytes(string path) {
    var raw = File.ReadAllBytes(path);
    int start = 0;
    while (start < raw.Length && IsSpace(raw[start])) {
        start++;
    }
    if (start >= raw.Length || raw[start] != 123) {
        return raw;
    }
    string text = Encoding.UTF8.GetString(raw);
    StringBuilder hex = new();
    foreach (string part in text.Split('\n')) {
        string trimmed = part.Trim();
        if (trimmed.Length == 0) {
            continue;
        }
        using JsonDocument document = JsonDocument.Parse(trimmed);
        if (document.RootElement.TryGetProperty("event", out JsonElement kind) && kind.GetString() == "OUTPUT") {
            if (document.RootElement.TryGetProperty("raw_hex", out JsonElement digest) && digest.GetString() is string piece) {
                hex.Append(piece);
            }
        }
    }
    return Convert.FromHexString(hex.ToString());
}

static bool IsSpace(byte value) =>
    value == 32 || value == 9 || value == 10 || value == 13 || value == 11 || value == 12;

static List<string> ContentLines(byte[] raw) {
    int offset = HasPrefix(raw) ? 10 : 0;
    if (raw.Length - offset >= 2 && raw[offset] == 13 && raw[offset + 1] == 10) {
        offset += 2;
    }
    Encoding encoding = Encoding.GetEncoding(437);
    string //
        text = encoding.GetString(raw, offset, raw.Length - offset),
        cleaned = Ansi().Replace(text, "");
    List<string> lines = [.. Splits().Split(cleaned)];
    return [.. lines.Where(Keeps)];
}

static bool HasPrefix(byte[] raw) =>
    raw.Length >= 10 && raw[0] == 27 && raw[1] == 91 && raw[2] == 54 && raw[3] == 110 && raw[4] == 27 &&
    raw[5] == 91 && raw[6] == 72 && raw[7] == 27 && raw[8] == 91 && raw[9] == 109;

static Regex Ansi() =>
    new("\x1b\\[[0-9;?]*[A-Za-z]|\x1b[()][0-9A-B]|\x1b=|\x1b>");

static Regex Splits() =>
    new("\\r\\n|\\n\\r|\\n|\\r");

static bool Keeps(string line) =>
    line.Trim() != "" && line.Trim() != "TICK !!!";

static int FindFirst(List<string> reference, List<string> actual, int total) {
    for (int i = 0; i < total; i++) {
        if (reference[i] != actual[i]) {
            return i;
        }
    }
    return -1;
}

static string Truncate(string line) =>
    line.Length > 120 ? line.Substring(0, 120) : line;

static string Repr(string value) {
    string quote = value.Contains('\'') && !value.Contains('"') ? "\"" : "'";
    StringBuilder builder = new();
    builder.Append(quote);
    foreach (char c in value) {
        builder.Append(Escape(c, quote));
    }
    builder.Append(quote);
    return builder.ToString();
}

static string Escape(char c, string quote) =>
    c switch {
        '\\' => "\\\\",
        '\n' => "\\n",
        '\r' => "\\r",
        '\t' => "\\t",
        _ when c.ToString() == quote => $"\\{quote}",
        _ when c < 32 || c == 127 => $"\\x{(int)c:x2}",
        _ when c < 127 => c.ToString(),
        _ when !char.IsControl(c) => c.ToString(),
        _ when c <= 255 => $"\\x{(int)c:x2}",
        _ when c <= 65535 => $"\\u{(int)c:x4}",
        _ => $"\\U{(int)c:x8}",
    };
