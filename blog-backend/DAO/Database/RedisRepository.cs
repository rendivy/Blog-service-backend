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

    public async Task<bool> AddExpiredToken(string tokenId)
    {
        var key = $"ExpiredToken:{tokenId}"; 
        return await _database.StringSetAsync(key, "expired", TimeSpan.FromDays(1));
    }

    public async Task<bool> IsTokenExpired(string tokenId)
    {
        var key = $"ExpiredToken:{tokenId}";
        return await _database.KeyExistsAsync(key);
    }
}