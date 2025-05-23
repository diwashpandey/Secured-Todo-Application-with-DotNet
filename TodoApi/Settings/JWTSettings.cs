namespace TodoApi.Settings;

public class JWTSettings
{
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}