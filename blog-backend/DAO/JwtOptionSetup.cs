using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace blog_backend.DAO;

public class JwtOptionSetup : IConfigureOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        throw new NotImplementedException();
    }
}