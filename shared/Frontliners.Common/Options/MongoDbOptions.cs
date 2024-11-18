namespace Frontliners.Common.Options;
public sealed class MongoDbOptions
{
    public string ConnectionString { get; set; }=null!;
    public string Database { get; set; }=null!;
}