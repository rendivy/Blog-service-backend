using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using blog_backend.Entity;
using Microsoft.IdentityModel.Tokens;

namespace blog_backend.Service;

public class GenerateJwt
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(GenerateSecretKey());
    
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Name, user.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    private static string GenerateSecretKey()
    {
        const int keyLength = 32;
        using var rng = new RNGCryptoServiceProvider();
        byte[] tokenData = new byte[keyLength];
        rng.GetBytes(tokenData);
        return Convert.ToBase64String(tokenData);
    }
}