using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolyhydraGames.Core.Models;

namespace PolyhydraGames.Api.SystemDefinitions;

public class SystemsDatabase
{
    ILogger<SystemsDatabase> _logger;
    private bool _initialized;
    private SystemsDatabase()
    {
    }

    public async Task Initialize(IServiceProvider appServices = null)
    {
        if (appServices != null)
        {
            _logger = appServices.GetRequiredService<ILogger<SystemsDatabase>>();
        }

        try
        {
            if (_systems.Any()) return;

            var path = "https://raw.githubusercontent.com/lancer1977/DataSeeds/master/retro/platform.json";
            using var client = new HttpClient();
            using var response = await client.GetAsync(path);
            var stream = await response.Content.ReadAsStringAsync();
            _systems.AddRange(stream.FromJson<List<SystemDefinition>>());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to initialize systems database");
        }

        _initialized = _systems.Any();
    }

    private List<SystemDefinition> _systems { get; } = new();
    public IEnumerable<SystemDefinition> Systems
    {
        get
        {
            if (!_initialized) throw new Exception("Systems not initialized!");
            return _systems;
        }
    }

    public static SystemsDatabase Instance { get; } = new();
}