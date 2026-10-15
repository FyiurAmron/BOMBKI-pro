using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

List<string> rest = [.. args];
string? //
    harness = null,
    binary = null,
    scenario = null;
int //
    seeds = 24,
    confirm = 2;
double timeout = 60.0;
bool help = false;
for (int i = 0; i < rest.Count; i++) {
    string arg = rest[i];
    if (arg == "--help" || arg == "-h") {
        help = true;
    } else if (arg == "--harness" || arg.StartsWith("--harness=", StringComparison.Ordinal)) {
        harness = Shift(arg, rest, ref i);
    } else if (arg == "--binary" || arg.StartsWith("--binary=", StringComparison.Ordinal)) {
        binary = Shift(arg, rest, ref i);
    } else if (arg == "--scenario" || arg.StartsWith("--scenario=", StringComparison.Ordinal)) {
        scenario = Shift(arg, rest, ref i);
    } else if (arg == "--seeds" || arg.StartsWith("--seeds=", StringComparison.Ordinal)) {
        string? raw = Shift(arg, rest, ref i);
        if (raw is null || !int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out seeds)) {
            Console.Error.WriteLine($"invalid --seeds: {raw}");
            return 2;
        }
    } else if (arg == "--confirm" || arg.StartsWith("--confirm=", StringComparison.Ordinal)) {
        string? raw = Shift(arg, rest, ref i);
        if (raw is null || !int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out confirm)) {
            Console.Error.WriteLine($"invalid --confirm: {raw}");
            return 2;
        }
    } else if (arg == "--timeout" || arg.StartsWith("--timeout=", StringComparison.Ordinal)) {
        string? raw = Shift(arg, rest, ref i);
        if (raw is null || !double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out timeout)) {
            Console.Error.WriteLine($"invalid --timeout: {raw}");
            return 2;
        }
    } else {
        Console.Error.WriteLine($"unknown argument: {arg}");
        return 2;
    }
}
if (help) {
    Console.WriteLine("usage: find-stable-seed --harness <exe> --binary <exe> --scenario <json> [--seeds N] [--confirm N] [--timeout S]");
    return 0;
}
if (harness is null) {
    Console.Error.WriteLine("missing required --harness");
    return 2;
}
if (binary is null) {
    Console.Error.WriteLine("missing required --binary");
    return 2;
}
if (scenario is null) {
    Console.Error.WriteLine("missing required --scenario");
    return 2;
}
string //
    root = FindRoot(),
    log = Path.Combine(root, "build", "tmp", "seedsearch.jsonl"),
    work = Path.Combine(root, "build", "tmp", "tour-run");
Directory.CreateDirectory(work);
for (int index = 1; index <= seeds; index++) {
    string seed = index.ToString(CultureInfo.InvariantCulture);
    (bool ok, string message) = Run(harness, binary, scenario, seed, timeout, log, work);
    if (!ok) {
        Console.WriteLine($"  seed {seed,4}: fail - {message}");
        continue;
    }
    int streak = 1;
    while (streak < confirm) {
        (bool again, string _) = Run(harness, binary, scenario, seed, timeout, log, work);
        if (!again) {
            break;
        }
        streak++;
    }
    if (streak >= confirm) {
        Console.WriteLine($"  seed {seed,4}: PASS ({streak} consecutive runs)");
        return 0;
    }
    Console.WriteLine($"  seed {seed,4}: passed once, not reproducible");
}
Console.WriteLine("no reproducible seed found");
return 1;

static string? Shift(string arg, List<string> rest, ref int i) {
    int marker = arg.IndexOf('=', StringComparison.Ordinal);
    if (marker >= 0) {
        return arg.Substring(marker + 1);
    }
    if (i + 1 >= rest.Count) {
        return null;
    }
    i++;
    return rest[i];
}

static string FindRoot() =>
    Search(AppContext.BaseDirectory) ?? Search(Directory.GetCurrentDirectory()) ?? Directory.GetCurrentDirectory();

static string? Search(string? start) {
    string? current = start;
    while (current is not null) {
        if (File.Exists(Path.Combine(current, "BombkiPro.slnx"))) {
            return current;
        }
        current = Path.GetDirectoryName(current);
    }
    return null;
}

static string FormatTimeout(double value) {
    string text = value.ToString(CultureInfo.InvariantCulture);
    return text.Contains('.') || text.Contains('e') || text.Contains('E') ? text : $"{text}.0";
}

static (bool, string) Run(string harness, string binary, string scenario, string seed, double timeout, string log, string work) {
    ProcessStartInfo info = new() {
        FileName = harness,
        WorkingDirectory = work,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
    };
    List<string> arguments = [
        binary,
        scenario,
        "--log",
        log,
        "--timeout",
        FormatTimeout(timeout),
    ];
    foreach (string part in arguments) {
        info.ArgumentList.Add(part);
    }
    info.Environment.Remove("BOMBKI_SEED");
    info.Environment["BOMBKI_SEED"] = seed;
    using Process process = Process.Start(info)!;
    bool exited = process.WaitForExit((int)(timeout * 3 * 1000));
    if (!exited) {
        try {
            process.Kill(true);
        } catch (InvalidOperationException) {
        }
        process.WaitForExit();
        return (false, "timeout");
    }
    return (process.ExitCode == 0, ReadFail(log));
}

static string ReadFail(string log) {
    if (!File.Exists(log)) {
        return "";
    }
    string message = "";
    foreach (string line in File.ReadAllText(log).Split('\n')) {
        string trimmed = line.Trim();
        if (trimmed.Length == 0) {
            continue;
        }
        try {
            using JsonDocument document = JsonDocument.Parse(trimmed);
            if (document.RootElement.TryGetProperty("event", out JsonElement kind) && kind.GetString() == "FAIL") {
                if (document.RootElement.TryGetProperty("message", out JsonElement detail) && detail.GetString() is string text) {
                    message = text.Length > 120 ? text.Substring(0, 120) : text;
                }
            }
        } catch (JsonException) {
        }
    }
    return message;
}
