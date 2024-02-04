namespace PolyhydraGames.Api.SystemDefinitions;

public class SystemDefinition
{
    public string Name { get; set; }
    //IGDB ID.
    public string IgdbId { get; set; }
    //Folder where roms and such would be stored. Should correspond to all files suich as art as well
    public string Folder { get; set; }

    public string Slug { get; set; }
    //Traditionally the core that is used to emulate this system. Example: "Sega - Dreamcast/Naomi"
    public string Core { get; set; }
    public string Extensions { get; set; }

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