namespace Bombki.Tests;

public class UnitsHistoryTests {
    [Fact]
    public void RememberSkipsBlankLines() {
        Crt.HistoryLines.Clear();
        Crt.RememberHistory("");
        Crt.RememberHistory("   ");
        Crt.RememberHistory("PIERWSZY");
        Assert.Single(Crt.HistoryLines);
        Assert.Equal("PIERWSZY", Crt.HistoryLines[0]);
    }

    [Fact]
    public void LimitDropsTheOldestLine() {
        Crt.HistoryLines.Clear();
        for (int i = 1; i <= Crt.HistoryLimit + 1; i++) {
            Crt.RememberHistory($"LINIA {i}");
        }

        Assert.Equal(Crt.HistoryLimit, Crt.HistoryLines.Count);
        Assert.Equal("LINIA 2", Crt.HistoryLines[0]);
        Assert.Equal($"LINIA {Crt.HistoryLimit + 1}", Crt.HistoryLines[^1]);
    }

    [Fact]
    public void UpAndDownWalkTheHistoryAndKeepTheDraft() {
        Crt.HistoryLines.Clear();
        Crt.RememberHistory("PIERWSZY");
        Crt.RememberHistory("DRUGI");
        Crt.BeginHistory();
        Assert.Equal("DRUGI", Crt.HistoryUp("SZKIC"));
        Assert.Equal("PIERWSZY", Crt.HistoryUp("DRUGI"));
        Assert.Equal("PIERWSZY", Crt.HistoryUp("PIERWSZY"));
        Assert.Equal("DRUGI", Crt.HistoryDown("PIERWSZY"));
        Assert.Equal("SZKIC", Crt.HistoryDown("DRUGI"));
        Assert.Equal("SZKIC", Crt.HistoryDown("SZKIC"));
    }

    [Fact]
    public void ArrowsOnEmptyHistoryKeepTheLine() {
        Crt.HistoryLines.Clear();
        Crt.BeginHistory();
        Assert.Equal("ABC", Crt.HistoryUp("ABC"));
        Assert.Equal("ABC", Crt.HistoryDown("ABC"));
    }

    [Fact]
    public void UpArrowRecallsThePreviousLine() {
        Crt.HistoryLines.Clear();
        using (TestStdin.Feed("EXIT\n\x1b[A\n")) {
            Assert.Equal("EXIT", Crt.ReadLn());
            Assert.Equal("EXIT", Crt.ReadLn());
        }
    }

    [Fact]
    public void DownArrowRestoresTheTypedDraft() {
        Crt.HistoryLines.Clear();
        using (TestStdin.Feed("EXIT\nJA\x1b[A\x1b[B\n")) {
            Assert.Equal("EXIT", Crt.ReadLn());
            Assert.Equal("JA", Crt.ReadLn());
        }
    }
}
