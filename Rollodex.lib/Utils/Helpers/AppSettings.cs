namespace Rolodex.Lib.Utils.Helpers;

public class AppSettings
{
    public string Secret { get; set; }

    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    public int RefreshTokenTTL { get; set; }
    public string SecurityIv { get; set; }
    public string Securitykey { get; set; }
    public int MaximumAdminCount { get; set; }
    public int TokenExpirationTimeHours { get; set; }
    public string BaseUrl { get; set; }
    public string GSecretKey { get; set; }
    public string VehicleRepoUrl { get;set; }
    public string VehiclePlateUrl { get; set; }
    public string Vas2netUrl { get; set; }
    public  List<string> MetricSettings { get; set; }
    public List<string> SearchFlow { get; set; }
    public List<string> DataDomains { get; set; }
    public string Vas2netBVNUrl { get; set; }
}