namespace Rollodex.lib.Models;

public class AppSettings
{
    public string Secret { get; set; }
    public int RefreshTokenTTL { get; set; }
    public string SecurityIv { get; set; }
    public string Securitykey { get; set; }
    public int MaximumAdminCount { get; set; }
    public int TokenExpirationTimeHours { get; set; }
    public string BaseUrl { get; set; }
}