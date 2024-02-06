using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;

namespace API.SystemDefinitions.Test;

[TestFixture]
public class SystemsTests
{
    public SystemsTests()
    {
        App = PolyhydraGames.Core.Test.TestFixtures.GetHost((x, services) =>
        {

            services.AddSingleton<HttpClient>();
        });
    }

    [Test]
    public async Task VerifySystems()
    {
        var systems = SystemsDatabase.Instance.Systems.ToList();
        systems.ForEach(x => Trace.WriteLine((string)x.ToString()));

        var systemsCount = systems.Count();
        if (systemsCount < 10) Assert.Fail("systemsCount is unusually low 0");
        Assert.Pass($"Systems count was: {systemsCount}");
    }

    [Test]
    public async Task GetBackgroundSlugsRecords()
    {
        var systems = SystemsDatabase.Instance.Systems;
        Assert.That(systems.All(x => string.IsNullOrEmpty(x.Slug) == false));
    }

    [TestCaseSource(nameof(SystemNames)),
    TestCase("mastersystem")]
    public async Task GetBackgroundFoldersRecords(string name)
    {
        var systemFromFolder = SystemsDatabase.Instance.GetCoreFromSlug(name);
        

        Assert.That(systemFromFolder, Is.Not.Null);
    }


#pragma warning disable NUnit1032
    protected IHost App { get; }
#pragma warning restore NUnit1032

    [SetUp]
    public async Task Setup()
    {
        await SystemsDatabase.Instance.Initialize();
    }

    public static string[] SystemNames()
    {
        return new[]
        {
            "atarilynx",
            "gameandwatch",
            "gb",
            "gba",
            "gbc",
            "ngp",
            "ngpc",
            "n3ds",
            "nds",
            "psp",
            "vita",
            "gamegear",
            "wonderswan",
            "wonderswancolor",
            "arcade",
            "arcade_chd",
            "daphne",
            "fba",
            "model123",
            "naomi",
            "genesis",
            "actionmax",
            "amigacd32",
            "atari2600",
            "atari5200",
            "atari7800",
            "3do",
            "atarijaguar",
            "atarijaguarcd",
            "astrocade",
            "coleco",
            "dreamcast",
            "channelf",
            "famicom",
            "intellivision",
            "neogeo",
            "neogeocd",
            "n64",
            "nes",
            "gamecube",
            "nswitch",
            "sgfx",
            "pcfx",
            "cdi",
            "ps1",
            //"ps2",
            //"ps3",
            //"ps4",
            //"ps5",
            "sega32x",
            "segacd",
            "sms",
            "genesis",
            "megadrive",
            "segapico",
            "saturn",
            "sg-1000",
            "snes",
            "tg16",
            "tg16cd",
            "pcengine",
            "pcenginecd",
            "vectrex",
            "virtualboy",
            "wii",
            "wiiu",
            "xbox",
            "fds",
            "odyssey2",
            "pico",
            "wiiware",
            "amiga",
            "amstradcpc",
            "apple2",
            "atarist",
            "atari800",
            "c64",
            "dos",
            "msdos",
            "pc", 
            "scummvm",
            "fmtowns",
            "ti99",
            "msx",
            "msx2",
            "zxspectrum",
            "zinc",
            "zmachine",
            "creativision",
            "crvision",
            "vg5000",
            "videopac",
            "x68000",
        };
    }
}