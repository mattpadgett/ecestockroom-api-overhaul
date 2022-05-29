namespace ecestockroom_api_v2.Contracts.Configuration;

public interface IPostgresDbSettings
{
    string ConnectionString { get; set; }
}