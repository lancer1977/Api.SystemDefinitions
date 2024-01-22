namespace PolyhydraGames.Api.SystemDefinitions;

public static class SystemHelpers
{ 
    public static string GetFolderFromCore(string core)
    {
        return SystemsDatabase.Instance.Systems.FirstOrDefault(x => x.Core == core)?.Folder ?? "";
    }
    public static string GetSystemFromExtension(string ext)
    {
        ext = ext.Replace(".", "");
        var system = SystemsDatabase.Instance.Systems.FirstOrDefault(x => x.Extensions.Contains(ext));
        var result = system?.Slug;
        return string.IsNullOrEmpty(result) ? "unknown" : result;
    }

    public static string GetIgdbIdFromSlug(string slug) => GetSystem(slug).IgdbId;

    public static string GetCoreFromSlug(this string slug) => GetSystem(slug).Core;



    public static string ToFolder(this string slug) => GetSystem(slug).Folder;


    public static List<string> GameSlugs()
    {
        return SystemsDatabase.Instance.Systems.Select(x => x.Slug).ToList();
    }

    public static SystemDefinition GetSystem(string slug)
    {
        var result = SystemsDatabase.Instance.Systems.FirstOrDefault(x => x.Slug == slug);
        if (result == null) throw new Exception($"System {slug} not found");
        return result;
    }

    public static SystemDefinition GetSystemFromCore(string core)
    {
        var result = SystemsDatabase.Instance.Systems.FirstOrDefault(x => x.Core == core);
        if (result == null) throw new Exception($"System {core} not found");
        return result;
    }
}