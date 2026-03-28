using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PolyhydraGames.Api.SystemDefinitions;

public class SystemsDatabase
{
    private const string DefaultDataSource = "https://raw.githubusercontent.com/lancer1977/DataSeeds/master/retro/platform.json";

    private readonly ILogger? _logger;
    private readonly HttpClient _httpClient;
    private readonly string _dataSource;
    private readonly List<SystemDefinition> _systems = new();
    private bool _initialized;

    private SystemsDatabase(ILogger? logger, HttpClient httpClient, string dataSource)
    {
        _logger = logger;
        _httpClient = httpClient;
        _dataSource = dataSource;
    }

    public const string UnknownSystemSlug = "unknown";

    public static SystemsDatabase Instance
    {
        get
        {
            return field ?? throw new InvalidOperationException("SystemsDatabase not initialized. Call Setup(ILogger) first.");
        }
        private set;
    }

    public static async Task Setup(ILogger? logger = null, HttpClient? httpClient = null, string? dataSource = null)
    {
        var database = new SystemsDatabase(logger, httpClient ?? new HttpClient(), dataSource ?? DefaultDataSource);
        Instance = database;
        await database.Initialize();
    }

    private async Task Initialize()
    {
        try
        {
            if (_initialized) return;

            var systems = await LoadSystemsAsync();
            _systems.Clear();
            _systems.AddRange(systems);
            _initialized = _systems.Count > 0;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to initialize systems database");
            _initialized = false;
        }
    }

    private async Task<List<SystemDefinition>> LoadSystemsAsync()
    {
        if (Uri.TryCreate(_dataSource, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            using var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            return await DeserializeSystemsAsync(stream);
        }

        await using var fileStream = File.OpenRead(_dataSource);
        return await DeserializeSystemsAsync(fileStream);
    }

    private static async Task<List<SystemDefinition>> DeserializeSystemsAsync(Stream stream)
    {
        var systems = await JsonSerializer.DeserializeAsync<List<SystemDefinition>>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return systems ?? throw new InvalidOperationException("System definitions could not be deserialized.");
    }

    public IReadOnlyList<SystemDefinition> Systems => !_initialized ? throw new InvalidOperationException("Systems not initialized.") : _systems;
    

    public string GetFolderFromCore(string core) => FindByCore(core)?.Folder ?? string.Empty;

    public string GetSystemFromExtension(string ext)
    {
        var result = FindByExtension(ext)?.Slug;
        return string.IsNullOrEmpty(result) ? UnknownSystemSlug : result;
    }

    public string GetIgdbIdFromSlug(string slug) => GetSystem(slug).IgdbId;

    public string GetCoreFromSlug(string slug) => GetSystem(slug).Core;

    public string ToFolder(string slug) => GetSystem(slug).Folder;

    public List<string> GameSlugs() => Systems.Select(x => x.Slug).ToList();

    public SystemDefinition GetSystem(string slug)
    {
        var normalizedSlug = NormalizeSlug(slug);
        var result = FindBySlug(normalizedSlug);
        if (result == null)
        {
            var ex = new KeyNotFoundException($"System {normalizedSlug} not found.");
            _logger?.LogCritical(ex, "GetSystem failed");
            throw ex;
        }
        return result;
    }

    public SystemDefinition GetSystemFromCore(string core)
    {
        var normalizedCore = NormalizeValue(core);
        var result = FindByCore(normalizedCore); 
        if (result == null)
        {
            var ex = new KeyNotFoundException($"System with core {normalizedCore} not found.");
            _logger?.LogCritical(ex, "GetSystem failed");
            throw ex;
        }
        return result;
    }

    public static string NormalizeSlug(string value)
    {
        var normalized = NormalizeValue(value).ToLowerInvariant();

        return normalized switch
        {
            "mastersystem" => "sms",
            "master system" => "sms",
            "pc engine" => "pcengine",
            "pce" => "pcengine",
            "arcade_chd" => "arcade",
            "daphne" => "arcade",
            "fba" => "arcade",
            "model123" => "arcade",
            "zinc" => "arcade",
            _ => normalized
        };
    }

    private SystemDefinition? FindBySlug(string slug)
    {
        return Systems.FirstOrDefault(x => string.Equals(x.Slug, slug, StringComparison.OrdinalIgnoreCase));
    }

    private SystemDefinition? FindByCore(string core)
    {
        return string.IsNullOrWhiteSpace(core) ? null : Systems.FirstOrDefault(x => string.Equals(x.Core, core, StringComparison.OrdinalIgnoreCase));
    }

    private SystemDefinition? FindByExtension(string ext)
    {
        var normalizedExtension = NormalizeExtension(ext);
        return string.IsNullOrEmpty(normalizedExtension) ? null : Systems.FirstOrDefault(x => string.Equals(x.Extensions, normalizedExtension, StringComparison.OrdinalIgnoreCase));
    }

    private static string NormalizeExtension(string ext)
    {
        return NormalizeValue(ext).TrimStart('.').ToLowerInvariant();
    }

    private static string NormalizeValue(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}