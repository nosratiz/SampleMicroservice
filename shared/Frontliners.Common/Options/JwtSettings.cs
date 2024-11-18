namespace Frontliners.Common.Options;

public sealed class JwtSettings
{
    public string SecretKey { get; set; }=null!;
    public string ValidIssuer { get; set; }=null!;
    public string ValidAudience { get; set; }=null!;
    public int ExpireDays { get; set; }
}