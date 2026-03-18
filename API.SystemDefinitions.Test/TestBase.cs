using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using PolyhydraGames.Api.SystemDefinitions;

namespace PolyhydraGames.IGDB.Test;

public abstract class TestBase
{
    [SetUp]
    public async Task Setup()
    {
        var logger =  NullLogger.Instance;
        try
        {
            await SystemsDatabase.Setup(logger, new HttpClient());
        }
        catch
        {
            // SystemsDatabase may not be available in test context
        }
    }
}