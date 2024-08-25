namespace Tattoo.Management.Models.Configuration;

public record HttpClientConfiguration
{
    public required string Name { get; set; }
    public string RelativePath { get; set; } = "";
    public bool ClientTypeRemote { get; set; } = true;
    public int TimeOutSeconds { get; set; } = 60;
    public Dictionary<string, string>? DefaultHeaders { get; set; }
}