using System.Net;
using blog_backend.DAO.Model;
using Newtonsoft.Json;

namespace blog_backend.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var error = new ErrorDTO
        {
            Message = exception.Message,
            Status = context.Response.StatusCode.ToString()
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(error));
    }
}