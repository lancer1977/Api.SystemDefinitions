using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolyhydraGames.Core.Models;

namespace PolyhydraGames.Api.SystemDefinitions;

public class SystemsDatabase
{
    private ILogger Logger { get; }
    private bool _initialized;
    private SystemsDatabase(ILogger logger)
    {
        Logger = logger;
    }

    public async Task Initialize( )
    { 
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
            Logger.LogError(e, "Failed to initialize systems database");
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

    public async Task Setup(ILogger logger)
    {
        Instance = new SystemsDatabase(logger);
        await Instance.Initialize();
    }
    public static SystemsDatabase Instance { get; private set; }

    public string GetFolderFromCore(string core)
    {
        return Systems.FirstOrDefault(x => x.Core == core)?.Folder ?? "";
    }
    public string GetSystemFromExtension(string ext)
    {
        ext = ext.Replace(".", "");
        var system = Systems.FirstOrDefault(x => x.Extensions.Contains(ext));
        var result = system?.Slug;
        return string.IsNullOrEmpty(result) ? "unknown" : result;
    }

    public string GetIgdbIdFromSlug(string slug) => GetSystem(slug).IgdbId;

    public string GetCoreFromSlug(string slug) => GetSystem(slug).Core;

    public string ToFolder(string slug) => GetSystem(slug).Folder;

    public List<string> GameSlugs()=> Systems.Select(x => x.Slug).ToList();
    
    public SystemDefinition GetSystem(string slug)
    {
        slug = SanitizeSlug(slug);
        var result = Systems.FirstOrDefault(x => x.Slug == slug);
        if (result == null) throw new Exception($"System {slug} not found");
        return result;
    }

    public SystemDefinition GetSystemFromCore(string core)
    {
        core = SanitizeSlug(core);
        var result = Systems.FirstOrDefault(x => x.Core == core);
        if (result == null) throw new Exception($"System {core} not found");
        return result;
    }

    public string SanitizeSlug(string value)
    {
        switch (value)
        {
            case "mastersystem": return "sms";
            case "pce": return "pcenengine";
            default: return value;
        }

    }
}