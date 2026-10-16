using System.Runtime.CompilerServices;

namespace Bombki.Tests;

internal static class AssemblySetup {
    [ModuleInitializer]
    public static void Init() {
        Environment.SetEnvironmentVariable("BOMBKI_DEBUG", "2137");
        if (Environment.GetEnvironmentVariable("BOMBKI_DELAY_MULT") is null)
            Environment.SetEnvironmentVariable("BOMBKI_DELAY_MULT", "0");
    }
}
