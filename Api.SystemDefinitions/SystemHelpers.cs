namespace PolyhydraGames.Api.SystemDefinitions
{
    public static class SystemHelpers
    {
        public static string GetFolderFromCore(string core)
        {
            return SystemsDatabase.Instance.GetFolderFromCore(core);
        }

        public static string GetSystemFromExtension(string ext)
        {
            return SystemsDatabase.Instance.GetSystemFromExtension(ext);
        }

        public static string GetIgdbIdFromSlug(string slug)
        {
            return GetSystem(slug).IgdbId;
        }

        public static string GetCoreFromSlug(this string slug)
        {
            return GetSystem(slug).Core;
        }

        public static string ToFolder(this string slug)
        {
            return GetSystem(slug).Folder;
        }

        public static List<string> GameSlugs()
        {
            return SystemsDatabase.Instance.GameSlugs();
        }

        public static SystemDefinition GetSystem(string slug)
        {
            return SystemsDatabase.Instance.GetSystem(slug);
        }

        public static SystemDefinition GetSystemFromCore(string core)
        {
            return SystemsDatabase.Instance.GetSystemFromCore(core);
        }

        public static string SanitizeSlug(this string value)
        {
            return SystemsDatabase.NormalizeSlug(value);
        }
    }
}