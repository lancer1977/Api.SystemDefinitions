using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using PolyhydraGames.Core.Test;

namespace API.SystemDefinitions.Test;

    [TestFixture]
    public class TestBase
    {
        public TestBase()
        {
            App = PolyhydraGames.Core.Test.TestFixtures.GetHost((x, services) =>
            {
 
                services.AddSingleton<HttpClient>();
            });
        }
#pragma warning disable NUnit1032
        protected IHost App { get; }
#pragma warning restore NUnit1032



        [TestCaseSource(nameof(GetSystemNames))]
        public async Task GetRandomBackground(string platform)
        {

 

        }

        //public static async Task GetArtBase(string game, string platform)
        //{
        //    var result = await _cdnService.GetBoxart(game, platform);
        //    Assert.That(result != null);
        //    Assert.That(!string.IsNullOrEmpty(result.ImageUrl));
        //    Assert.That(result.ImageUrl.IsImageFile());
        //    Console.WriteLine(result.ImageUrl);
        //}
        //[TestCaseSource(nameof(GetSystemNames))]
        //public static async Task GetRandomBoxart(string platform)
        //{

        //    var result = await _cdnService.GetRandomArt(platform, ArtType.Boxart);
        //    Console.WriteLine(result.ImageUrl);
        //    Assert.That(result.Success, result.ErrorMessage);
        //    Assert.That(result != null, "result was null");
        //    Assert.That(!string.IsNullOrEmpty(result.ImageUrl));
        //    Assert.That(result.ImageUrl.IsImageFile());

        //}

        //public static void GetSystemBase(string slug)
        //{

        //    var system = SystemHelpers.GetSystem(slug);
        //    //Assert.That(system.IgdbId, !Is.Empty);
        //    Assert.That(system.Folder, !Is.Empty);
        //    Assert.That(system.Slug, !Is.Empty);
        //    //Assert.That(system.Core, !Is.Empty);
        //}

        //public abstract void GetSystem(string slug);


        public static string[] GetSystemNames()
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
            "scumm",
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

