namespace PolyhydraGames.Api.SystemDefinitions;

public static class SystemHelpers
{
    public static string GetFolderFromCore(string core) => SystemsDatabase.Instance.GetFolderFromCore(core);

    public static string GetSystemFromExtension(string ext) => SystemsDatabase.Instance.GetSystemFromExtension(ext);

    public static string GetIgdbIdFromSlug(string slug) => GetSystem(slug).IgdbId;

    public static string GetCoreFromSlug(this string slug) => GetSystem(slug).Core;

    public static string ToFolder(this string slug) => GetSystem(slug).Folder;

    public static List<string> GameSlugs() => SystemsDatabase.Instance.GameSlugs();

    public static SystemDefinition GetSystem(string slug) => SystemsDatabase.Instance.GetSystem(slug);

    public static SystemDefinition GetSystemFromCore(string core) => SystemsDatabase.Instance.GetSystemFromCore(core);

    public static string SanitizeSlug(this string value) => SystemsDatabase.NormalizeSlug(value);
}