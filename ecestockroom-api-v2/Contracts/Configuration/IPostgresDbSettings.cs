namespace ecestockroom_api_v2.Contracts.Configuration;

public interface IMongoDbSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}