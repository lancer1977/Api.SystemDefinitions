namespace PolyhydraGames.Api.SystemDefinitions;

public sealed record SystemDefinition
{
    public string Name { get; init; } = string.Empty;
    // IGDB ID.
    public string IgdbId { get; init; } = string.Empty;
    // Folder where roms and related assets are stored.
    public string Folder { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;
    // Traditionally the core used to emulate this system. Example: "Sega - Dreamcast/Naomi"
    public string Core { get; init; } = string.Empty;
    public string Extensions { get; init; } = string.Empty;

    public override string ToString()
    {
        return $"{nameof(Name)}:{Name}, {nameof(IgdbId)}: {IgdbId}, {nameof(Folder)}: {Folder}, {nameof(Slug)}: {Slug}";
        // {nameof(Platform)}: {Platform}, {nameof(System)}: {System}
    }

    //[Obsolete("Use Slug instead")]
    //System code: Example: "nswitch"
    //public string System { get; set; }
    //[Obsolete("Use Slug instead")]
    //public string Platform { get; set; }

}