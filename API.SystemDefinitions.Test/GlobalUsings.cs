using System.Diagnostics;
using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;

namespace API.SystemDefinitions.Test;

[TestFixture]
public class SystemsTests : TestBase
{



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
        var systems = SystemsDatabase.Instance.Systems; ;
        Assert.That(systems.All(x => string.IsNullOrEmpty(x.Slug) == false));
    }

    [Test]
    public async Task GetBackgroundFolderssRecords()
    {
        var systems = SystemsDatabase.Instance.Systems; ;

        Assert.That(systems.All(x => string.IsNullOrEmpty(x.Folder) == false));


    }


}