using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using blog_backend.DAO.Database;
using blog_backend.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace blog_backend.Service;

public class GenerateTokenService
{
    public static readonly string SecretKey = GenerateSecretKey();
    private readonly BlogDbContext _dbContext;

    public GenerateTokenService(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    
    public void SaveExpiredToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        _dbContext.ExpiredTokens.Add(new ExpiredToken
        {
            Token = token,
            ExpiryDate = jsonToken?.ValidTo ?? DateTime.UtcNow 
        });

        _dbContext.SaveChanges();
    }


    public static string GenerateSecretKey()
    {
        const int keyLength = 32;
        using var rng = new RNGCryptoServiceProvider();
        byte[] tokenData = new byte[keyLength];
        rng.GetBytes(tokenData);
        return Convert.ToBase64String(tokenData);
    }
}