using System.Linq;

namespace Bombki.Tests;

public class RngTests {
    readonly Game game = new();
    List<int> Draws(int bound, int count) =>
        [.. Enumerable.Range(0, count).Select(_ => game.Roll(bound))];

    [Fact]
    public void Rolls_match_oracle_sequence() {
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "0x9E3779B97F4A7C15");
        game.Randomize();
        Assert.Equal(new List<int> { 74, 77, 46, 11, 93, 99, 36, 78, 86, 9 },
            Draws(100, 10));
    }

    [Fact]
    public void Roll_zero_returns_zero_without_advancing() {
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "0x9E3779B97F4A7C15");
        game.Randomize();
        var before = Draws(100, 3);
        Assert.Equal(new List<int> { 74, 77, 46 }, before);
        Assert.Equal(0, game.Roll(0));
        Assert.Equal(11, game.Roll(100));
    }

    [Fact]
    public void Empty_seed_falls_back_to_default() {
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "");
        game.Randomize();
        var blank = Draws(100, 3);
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "0x9E3779B97F4A7C15");
        game.Randomize();
        Assert.Equal(Draws(100, 3), blank);
    }

    [Fact]
    public void Decimal_seed_parses() {
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "12345");
        game.Randomize();
        Assert.Equal(new List<int> { 70, 79, 39 }, Draws(100, 3));
    }
}
