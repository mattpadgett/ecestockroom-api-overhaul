namespace ecestockroom_api_v2.Contracts.Configuration;

public class PostgresDbSettings : IPostgresDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}