using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;
using PolyhydraGames.IGDB.Test;

namespace API.SystemDefinitions.Test;

[TestFixture]
public class SystemsTests :TestBase
{
    [Test]
    public void VerifySystems()
    {
        var systems = SystemsDatabase.Instance.Systems.ToList();
        Assert.That(systems, Is.Not.Empty);
        Assert.That(systems.All(x => !string.IsNullOrWhiteSpace(x.Slug)), Is.True);
        Assert.That(systems.All(x => !string.IsNullOrWhiteSpace(x.Folder)), Is.True);
        Assert.That(systems.All(x => !string.IsNullOrWhiteSpace(x.Extensions)), Is.True);
        Assert.That(systems.All(x => !string.IsNullOrWhiteSpace(x.Core)), Is.True);
    }

    [TestCase("sms", "sms")]
    [TestCase("SMS", "sms")]
    [TestCase("mastersystem", "sms")]
    [TestCase("master system", "sms")]
    [TestCase("pce", "pcengine")]
    [TestCase("pc engine", "pcengine")]
    [TestCase("arcade_chd", "arcade")]
    [TestCase("daphne", "arcade")]
    [TestCase("fba", "arcade")]
    [TestCase("model123", "arcade")]
    [TestCase("naomi", "naomi")]
    [TestCase("zinc", "arcade")]
    [TestCase("dreamcast", "dreamcast")]
    [TestCase("odyssey2", "odyssey2")]
    [TestCase("pcenginecd", "pcenginecd")]
    [TestCase("megadrive", "genesis")]
    [TestCase("mega drive", "genesis")]
    [TestCase("md", "genesis")]
    [TestCase("sfc", "snes")]
    [TestCase("super famicom", "snes")]
    [TestCase("super nintendo", "snes")]
    [TestCase("nintendo64", "n64")]
    [TestCase("nintendo 64", "n64")]
    [TestCase("ps1", "psx")]
    [TestCase("playstation", "psx")]
    [TestCase("  SMS  ", "sms")]
    public void GetSystem_NormalizesSlugAliases(string name, string expectedSlug)
    {
        var system = SystemsDatabase.Instance.GetSystem(name);
        Assert.That(system.Slug, Is.EqualTo(expectedSlug));
    }

    [Test]
    public void GetSystemFromCore_IsCaseInsensitive()
    {
        var system = SystemsDatabase.Instance.GetSystemFromCore("sega - dreamcast/naomi");
        Assert.That(system.Slug, Is.EqualTo("naomi"));
    }

    [Test]
    public void GetSystemFromExtension_ReturnsUnknownForMissingExtension()
    {
        var systemSlug = SystemsDatabase.Instance.GetSystemFromExtension(".7z");
        Assert.That(systemSlug, Is.EqualTo(SystemsDatabase.UnknownSystemSlug));
    }

    [Test]
    public void GetSystemFromExtension_NormalizesLeadingDot()
    {
        var systemSlug = SystemsDatabase.Instance.GetSystemFromExtension(".gba");
        Assert.That(systemSlug, Is.EqualTo("gba"));
    }

    [TestCase("gba", "gba")]
    [TestCase(" .GBA ", "gba")]
    [TestCase("smc", "snes")]
    [TestCase(".SFC", "snes")]
    [TestCase("md", "genesis")]
    [TestCase(".GEN", "genesis")]
    [TestCase("nes", "nes")]
    [TestCase("z64", "n64")]
    [TestCase(".pbp", "psx")]
    [TestCase("", SystemsDatabase.UnknownSystemSlug)]
    [TestCase("   ", SystemsDatabase.UnknownSystemSlug)]
    public void GetSystemFromExtension_NormalizesCommonInputShapes(string extension, string expectedSlug)
    {
        var systemSlug = SystemsDatabase.Instance.GetSystemFromExtension(extension);
        Assert.That(systemSlug, Is.EqualTo(expectedSlug));
    }

    [TestCase("/roms/snes/Chrono Trigger (USA).sfc", "snes")]
    [TestCase(@"D:\roms\genesis\Sonic The Hedgehog 2.md", "genesis")]
    [TestCase("/roms/psx/Castlevania - Symphony of the Night.cue", "psx")]
    [TestCase("/roms/pcenginecd/Dracula X.cue", "pcenginecd")]
    [TestCase("/roms/unknown/readme.txt", SystemsDatabase.UnknownSystemSlug)]
    public void GetSystemFromPath_MapsInventoryPathsToCanonicalSlugs(string path, string expectedSlug)
    {
        var systemSlug = SystemsDatabase.Instance.GetSystemFromPath(path);
        Assert.That(systemSlug, Is.EqualTo(expectedSlug));
    }

    [TestCase("snes", "igdb", "19")]
    [TestCase("super nintendo", "IGDB", "19")]
    [TestCase("genesis", "thegamesdb", "")]
    public void GetProviderIdFromSlug_ReturnsKnownProviderIdsDeterministically(string slug, string provider, string expectedId)
    {
        var providerId = SystemsDatabase.Instance.GetProviderIdFromSlug(slug, provider);
        Assert.That(providerId, Is.EqualTo(expectedId));
    }

    [Test]
    public void GetSystem_ThrowsUsefulErrorForUnknownSlug()
    {
        var ex = Assert.Throws<KeyNotFoundException>(() => SystemsDatabase.Instance.GetSystem("not-a-system"));
        Assert.That(ex!.Message, Does.Contain("not-a-system"));
    }

    [Test]
    public void GetSystemFromCore_ThrowsUsefulErrorForUnknownCore()
    {
        var ex = Assert.Throws<KeyNotFoundException>(() => SystemsDatabase.Instance.GetSystemFromCore("unknown core"));
        Assert.That(ex!.Message, Does.Contain("unknown core"));
    }

    [Test]
    public void GameSlugs_ReturnsFixtureSlugsInSourceOrder()
    {
        var slugs = SystemsDatabase.Instance.GameSlugs();
        Assert.That(slugs, Is.EqualTo(new[]
        {
            "arcade",
            "sms",
            "gba",
            "pcengine",
            "naomi",
            "dreamcast",
            "odyssey2",
            "pcenginecd",
            "nes",
            "snes",
            "genesis",
            "n64",
            "psx"
        }));
    }

    [Test]
    public void StaticHelpers_DelegateToSystemsDatabase()
    {
        Assert.That(SystemHelpers.GetSystem("SMS").Slug, Is.EqualTo("sms"));
        Assert.That("mastersystem".GetCoreFromSlug(), Is.EqualTo(SystemsDatabase.Instance.GetCoreFromSlug("mastersystem")));
        Assert.That("SMS".ToFolder(), Is.EqualTo("sms"));
        Assert.That(SystemHelpers.GetSystemFromPath("/roms/snes/Chrono Trigger.smc"), Is.EqualTo("snes"));
        Assert.That(SystemHelpers.GetProviderIdFromSlug("snes", "igdb"), Is.EqualTo("19"));
    }
}
