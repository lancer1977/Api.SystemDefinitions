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
    [TestCase("", SystemsDatabase.UnknownSystemSlug)]
    [TestCase("   ", SystemsDatabase.UnknownSystemSlug)]
    public void GetSystemFromExtension_NormalizesCommonInputShapes(string extension, string expectedSlug)
    {
        var systemSlug = SystemsDatabase.Instance.GetSystemFromExtension(extension);
        Assert.That(systemSlug, Is.EqualTo(expectedSlug));
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
            "pcenginecd"
        }));
    }

    [Test]
    public void StaticHelpers_DelegateToSystemsDatabase()
    {
        Assert.That(SystemHelpers.GetSystem("SMS").Slug, Is.EqualTo("sms"));
        Assert.That("mastersystem".GetCoreFromSlug(), Is.EqualTo(SystemsDatabase.Instance.GetCoreFromSlug("mastersystem")));
        Assert.That("SMS".ToFolder(), Is.EqualTo("sms"));
    }
}
