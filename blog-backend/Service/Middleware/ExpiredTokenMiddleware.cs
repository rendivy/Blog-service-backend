using System.Net;
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
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (!string.IsNullOrEmpty(token))
        {
            var expiredToken =
                dbContext.ExpiredTokens.FirstOrDefault(t => t.Token == token && t.ExpiryDate >= DateTime.UtcNow);

            if (expiredToken != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}