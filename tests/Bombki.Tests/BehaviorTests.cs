using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Bombki.Tests;

public class BehaviorTests {
    const string Seed = "0x9E3779B97F4A7C15";
    const int Level2Deduction = 725;

    public static TheoryData<string> Scenarios => new() {
        "level-up-deduction",
        "level-zero",
        "shop-item-display",
        "shop-repeat-buy",
        "shop-quirks",
        "race-stats-czlowiek",
        "race-stats-olbrzym",
        "race-stats-nimfa",
        "race-stats-pol-elf",
        "race-stats-ufok",
        "race-stats-czaromil",
        "zre-limits",
        "mode",
        "mode-save-load",
        "test_save_load_roundtrip",
        "mode-sleep",
        "ground-commands",
        "ground-skills",
        "shops-rich",
        "czaromil-checkpoints",
        "arena-poster-tour",
        "cage-fights-tour",
        "cages-tour",
        "concert-tour",
        "dark-street-tour",
        "quest-road-tour",
        "school-tour",
        "road-rooms",
        "city-dogs",
        "city-people",
        "cage-14-loot",
        "cage-14-loss",
        "cage-15-loot",
        "cage-15-loss",
        "cage-16-loot",
        "cage-16-loss",
        "cage-diploma",
        "cage-heart-drop",
        "room72-exit",
        "wyjscie-quit",
        "living-door-escape",
        "living-door-defeat",
        "staruch-gifts",
        "well",
        "backspace-edit",
        "escape-keys",
        "intro-ctrl-c",
        "bar-fights",
        "bar-intro",
        "cage-14-heart",
        "cage-16",
        "cage-loss",
        "cave-boss-death",
        "checkpoint-826",
        "command-history",
        "concert-barricade",
        "concert-crowd",
        "concert-dispatch",
        "concert-pub",
        "dj-fight",
        "fajka-ground",
        "garden-dispatch",
        "level-max-equal",
        "level-max-mad-top",
        "level-max-zre-top",
        "level-max-zretie",
        "level-up",
        "minibarman-fight",
        "poster-reward",
        "shops-tour",
        "skill-comparison",
        "skill-flee",
        "skill-kick",
        "sleep-rounds",
        "stats-training",
        "test_save_load",
        "training-poster",
        "wroggen-200",
        "wroggen-win",
        "zamiatacz-kill",
    };

    [Theory]
    [MemberData(nameof(Scenarios))]
    public void Scenario_matches_original_behavior(string name) {
        var root = FindRepoRoot();
        var binary = FindStubBinary(root);
        var scenario = $"{root}/tests/scenarios/{name}.json";
        Assert.True(File.Exists(scenario), $"scenario file not found: {scenario}");

        var logDir = $"{root}/build/pty-csharp";
        Directory.CreateDirectory(logDir);
        var log = $"{logDir}/{name}.jsonl";
        var harness = FindHarnessBinary(root);

        var psi = new ProcessStartInfo(harness, $"\"{binary}\" \"{scenario}\" --log \"{log}\" --timeout 20 --settle 2") {
            WorkingDirectory = root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        psi.Environment["BOMBKI_SEED"] = Seed;
        psi.Environment["BOMBKI_DELAY_MULT"] = "0";
        psi.Environment["BOMBKI_DEBUG"] = "2137";

        using var process = Process.Start(psi);
        Assert.NotNull(process);
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        if (!process.WaitForExit(180_000)) {
            try {
                process.Kill(entireProcessTree: true);
            } catch { }
            Assert.Fail($"FAIL {name}: harness timed out after 180s\nstdout:\n{stdout}\nstderr:\n{stderr}");
        }

        if (process.ExitCode != 0) {
            Assert.Fail($"FAIL {name}: expect_pty exited {process.ExitCode}\nstdout tail:\n{Tail(stdout, 2000)}\nstderr:\n{stderr}\nlog: {log}");
        }

        if (name == "level-up-deduction") {
            var reason = CheckLevelUpDeduction(log);
            if (reason is not null) {
                Assert.Fail($"FAIL {name}: {reason}");
            }
        }

        if (name == "escape-keys") {
            var reason = CheckNoCursorMotion(log);
            if (reason is not null) {
                Assert.Fail($"FAIL {name}: {reason}");
            }
        }
    }

    static string? CheckNoCursorMotion(string logPath) {
        foreach (var line in File.ReadLines(logPath)) {
            JsonDocument doc;
            try {
                doc = JsonDocument.Parse(line);
            } catch {
                continue;
            }
            using (doc) {
                var element = doc.RootElement;
                if (!element.TryGetProperty("event", out var ev) || ev.GetString() != "OUTPUT") {
                    continue;
                }

                if (!element.TryGetProperty("raw_hex", out var raw)) {
                    continue;
                }

                if (element.TryGetProperty("step", out var step) && step.GetInt32() <= 1) {
                    continue;
                }

                byte[] bytes;
                try {
                    bytes = Convert.FromHexString(raw.GetString() ?? "");
                } catch {
                    continue;
                }
                string text = Encoding.Latin1.GetString(bytes).Replace("\x1b[H\x1b[2J", "");
                int at = text.IndexOf('\x1b');
                if (at >= 0) {
                    return $"cursor motion bytes {ToHex(text.Substring(at, Math.Min(8, text.Length - at)))} in output";
                }
            }
        }

        return null;
    }

    static string ToHex(string value) =>
        BitConverter.ToString(Encoding.Latin1.GetBytes(value));

    static string? CheckLevelUpDeduction(string logPath) {
        int? before = null;
        int? residue = null;
        var promptValue = new Regex(@"[0-9]+%\.(?<kunszt>-?[0-9]+)>", RegexOptions.Compiled);
        var gap = new Regex(@"BRAKUJE CI (?<gap>-?[0-9]+) KUNSZTU",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        foreach (var line in File.ReadLines(logPath)) {
            JsonDocument doc;
            try {
                doc = JsonDocument.Parse(line);
            } catch {
                continue;
            }
            using (doc) {
                var element = doc.RootElement;
                if (!element.TryGetProperty("event", out var ev) || ev.GetString() != "MATCH") {
                    continue;
                }

                var label = element.TryGetProperty("label", out var l) ? l.GetString() ?? "" : "";
                var matched = element.TryGetProperty("matched", out var m) ? m.GetString() ?? "" : "";
                if (label.Contains("to force the pass")) {
                    var mm = promptValue.Match(matched);
                    if (mm.Success && int.TryParse(mm.Groups["kunszt"].Value, out var k)) {
                        before = k;
                    }
                }

                if (label.Contains("sheet shows the level-2 line")) {
                    var mm = gap.Match(matched);
                    if (mm.Success && int.TryParse(mm.Groups["gap"].Value, out var g)) {
                        residue = g;
                    }
                }
            }
        }

        if (before is null || residue is null) {
            return "transcript lacks the leveling prompt or the level-2 sheet line";
        }

        var observed = before.Value + residue.Value - Level2Deduction;
        return observed != Level2Deduction
            ? $"prompt KUNSZT {before} + sheet residue {residue} implies a {observed} deduction at level 1, expected {Level2Deduction}"
            : null;
    }

    static string FindRepoRoot() {
        var dir = AppContext.BaseDirectory;
        while (dir is not null) {
            if (File.Exists(Path.Combine(dir, "BombkiPro.slnx")) && File.Exists(Path.Combine(dir, "tools/expect-pty/expect-pty.csproj"))) {
                return dir.TrimEnd('/');
            }

            dir = Directory.GetParent(dir)?.FullName;
        }

        throw new InvalidOperationException("repo root (BombkiPro.slnx) not found");
    }

    static string FindHarnessBinary(string root) {
        List<string> names = OperatingSystem.IsWindows() ? ["expect-pty.exe"] : ["expect-pty"];
        var configs = new List<string> { "Debug", "Release" };
        foreach (var config in configs) {
            foreach (var name in names) {
                var candidate = $"{root}/build/out/expect-pty/{config}/net11.0/{name}";
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }
        }

        throw new FileNotFoundException($"harness binary not found under build/out/expect-pty/*/net11.0/. Run 'dotnet build' first.");
    }

    static string FindStubBinary(string root) {
        List<string> names = OperatingSystem.IsWindows() ? ["Bombki.exe"] : ["Bombki", "Bombki.dll"];
        var configs = new List<string> { "Debug", "Release" };
        foreach (var config in configs) {
            foreach (var name in names) {
                var candidate = $"{root}/build/out/Core/{config}/net11.0/{name}";
                if (File.Exists(candidate)) {
                    if (name.EndsWith(".dll")) {
                        continue;
                    }

                    return candidate;
                }
            }
        }

        throw new FileNotFoundException($"stub binary not found under build/out/Core/*/net11.0/. Run 'dotnet build' first.");
    }

    static string Tail(string text, int maxChars) =>
        text.Length <= maxChars ? text : text[^maxChars..];
}
