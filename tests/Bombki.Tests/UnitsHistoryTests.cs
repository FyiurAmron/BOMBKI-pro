namespace Bombki.Tests;

public class UnitsHistoryTests {
    [Fact]
    public void RememberSkipsBlankLines() {
        History.Lines.Clear();
        History.Remember("");
        History.Remember("   ");
        History.Remember("PIERWSZY");
        Assert.Single(History.Lines);
        Assert.Equal("PIERWSZY", History.Lines[0]);
    }

    [Fact]
    public void LimitDropsTheOldestLine() {
        History.Lines.Clear();
        for (int i = 1; i <= History.Limit + 1; i++) {
            History.Remember($"LINIA {i}");
        }

        Assert.Equal(History.Limit, History.Lines.Count);
        Assert.Equal("LINIA 2", History.Lines[0]);
        Assert.Equal($"LINIA {History.Limit + 1}", History.Lines[^1]);
    }

    [Fact]
    public void UpAndDownWalkTheHistoryAndKeepTheDraft() {
        History.Lines.Clear();
        History.Remember("PIERWSZY");
        History.Remember("DRUGI");
        History.Begin();
        Assert.Equal("DRUGI", History.Up("SZKIC"));
        Assert.Equal("PIERWSZY", History.Up("DRUGI"));
        Assert.Equal("PIERWSZY", History.Up("PIERWSZY"));
        Assert.Equal("DRUGI", History.Down("PIERWSZY"));
        Assert.Equal("SZKIC", History.Down("DRUGI"));
        Assert.Equal("SZKIC", History.Down("SZKIC"));
    }

    [Fact]
    public void ArrowsOnEmptyHistoryKeepTheLine() {
        History.Lines.Clear();
        History.Begin();
        Assert.Equal("ABC", History.Up("ABC"));
        Assert.Equal("ABC", History.Down("ABC"));
    }

    [Fact]
    public void UpArrowRecallsThePreviousLine() {
        History.Lines.Clear();
        using (TestStdin.Feed("EXIT\n\x1b[A\n")) {
            Assert.Equal("EXIT", Crt.ReadLn());
            Assert.Equal("EXIT", Crt.ReadLn());
        }
    }

    [Fact]
    public void DownArrowRestoresTheTypedDraft() {
        History.Lines.Clear();
        using (TestStdin.Feed("EXIT\nJA\x1b[A\x1b[B\n")) {
            Assert.Equal("EXIT", Crt.ReadLn());
            Assert.Equal("JA", Crt.ReadLn());
        }
    }
}
