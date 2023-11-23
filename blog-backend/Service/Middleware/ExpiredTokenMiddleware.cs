using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using blog_backend.DAO.Database;

namespace blog_backend.Service.Middleware;

public class ExpiredTokenMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExpiredTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, BlogDbContext dbContext)
    {
        var tokenId = context.User.FindFirstValue(ClaimTypes.SerialNumber);
        var token = await dbContext.ExpiredTokens.FindAsync(Guid.Parse(tokenId));
        if (token != null)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return;
        }

        await _next(context);
    }
}