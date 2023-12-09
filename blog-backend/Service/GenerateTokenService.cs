using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using blog_backend.DAO.Database;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using Microsoft.IdentityModel.Tokens;

namespace blog_backend.Service;

public class GenerateTokenService
{
    private readonly IConfiguration _configuration;
    private readonly BlogDbContext _dbContext;
    private readonly RedisRepository _redisRepository;
    

    public GenerateTokenService(BlogDbContext dbContext, IConfiguration configuration, RedisRepository redisRepository)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _redisRepository = redisRepository;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
        var tokenId = Guid.NewGuid().ToString();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.SerialNumber, tokenId),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha384)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    private string ComputeTokenHash(string token)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }


    public void SaveExpiredToken(string tokenId)
    {
        _redisRepository.AddExpiredToken(tokenId);
    }

    private DateTime? GetTokenExpiryDate(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        return jsonToken?.ValidTo;
    }
}