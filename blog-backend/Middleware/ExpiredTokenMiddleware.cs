using System.Net;
using System.Security.Claims;
using blog_backend.DAO.Database;

namespace blog_backend.Middleware;

public class ExpiredTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RedisRepository _redisRepository;

    public ExpiredTokenMiddleware(RequestDelegate next, RedisRepository redisRepository)
    {
        _next = next;
        _redisRepository = redisRepository;
    }

    public async Task Invoke(HttpContext context)
    {
        var tokenId = context.User.FindFirstValue(ClaimTypes.SerialNumber);
        if (tokenId != null)
        {
            var isTokenExpired = _redisRepository.IsTokenExpired(tokenId);
            if (isTokenExpired)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }
        await _next(context);
    }
}