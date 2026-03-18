using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;

namespace API.SystemDefinitions.Test;

[TestFixture]
public class SystemsTests
{
    public SystemsTests()
    {
        App = PolyhydraGames.Core.Test.TestHelpers.GetHost((_, _) => { });
    }

    [Test]
    public void VerifySystems()
    {
        var systems = SystemsDatabase.Instance.Systems.ToList();
        Assert.That(systems, Has.Count.EqualTo(5));
        Assert.That(systems.All(x => !string.IsNullOrWhiteSpace(x.Slug)));
    }

    [TestCase("sms", "sms")]
    [TestCase("SMS", "sms")]
    [TestCase("mastersystem", "sms")]
    [TestCase("pce", "pcengine")]
    [TestCase("arcade_chd", "arcade")]
    [TestCase("daphne", "arcade")]
    [TestCase("fba", "arcade")]
    [TestCase("model123", "arcade")]
    [TestCase("naomi", "arcade")]
    [TestCase("zinc", "arcade")]
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

    [Test]
    public void StaticHelpers_DelegateToSystemsDatabase()
    {
        Assert.That(SystemHelpers.GetSystem("SMS").Slug, Is.EqualTo("sms"));
        Assert.That("mastersystem".GetCoreFromSlug(), Is.EqualTo(SystemsDatabase.Instance.GetCoreFromSlug("mastersystem")));
        Assert.That("SMS".ToFolder(), Is.EqualTo("sms"));
    }

#pragma warning disable NUnit1032
    protected IHost App { get; }
#pragma warning restore NUnit1032

    [OneTimeSetUp]
    public async Task Setup()
    {
        var logger = App.Services.GetRequiredService<ILogger<SystemsTests>>();
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "TestData", "platform.fixture.json");
        await SystemsDatabase.Setup(logger, dataSource: fixturePath);
    }
}