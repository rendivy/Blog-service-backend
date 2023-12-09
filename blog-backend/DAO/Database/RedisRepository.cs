using StackExchange.Redis;

namespace blog_backend.DAO.Database;

public class RedisRepository
{
    private readonly IDatabase _database;

    public RedisRepository(string connectionString)
    {
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _database = connection.GetDatabase();
    }
    
    public void AddExpiredToken(string tokenId)
    {
        var key = $"ExpiredToken:{tokenId}";
        _database.StringSet(key, "expired", TimeSpan.FromDays(1));
    }
    
    public bool IsTokenExpired(string tokenId)
    {
        var key = $"ExpiredToken:{tokenId}";
        return _database.KeyExists(key);
    }

    
}