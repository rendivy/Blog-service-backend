using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using blog_backend.DAO.Database;
using blog_backend.Entity;
using Microsoft.IdentityModel.Tokens;

namespace blog_backend.Service;

public class GenerateTokenService
{
    private readonly IConfiguration _configuration;
    private readonly BlogDbContext _dbContext;

    public GenerateTokenService(BlogDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
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
        var guidTokenId = Guid.TryParse(tokenId, out var guid) ? guid : Guid.Empty;
        _dbContext.ExpiredTokens.Add(new ExpiredToken
        {
            Id = guidTokenId,
            ExpiryDate = DateTime.UtcNow
        });

        _dbContext.SaveChanges();
    }

    private DateTime? GetTokenExpiryDate(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        return jsonToken?.ValidTo;
    }
}