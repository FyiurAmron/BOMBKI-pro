namespace Bombki.Tests;

internal static class UnitsSeed {
    public static void Reseed(Game game) {
        Environment.SetEnvironmentVariable("BOMBKI_DEBUG", "2137");
        Environment.SetEnvironmentVariable("BOMBKI_SEED", "0x9E3779B97F4A7C15");
        game.Randomize();
    }
}
