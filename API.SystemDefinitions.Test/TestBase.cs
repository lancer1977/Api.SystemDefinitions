using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;

namespace PolyhydraGames.IGDB.Test;

public abstract class TestBase
{
    [SetUp]
    public async Task Setup()
    {
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "TestData", "platform.fixture.json");
        await SystemsDatabase.Setup(NullLogger.Instance, dataSource: fixturePath);
    }
}
