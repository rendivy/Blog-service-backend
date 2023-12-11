using AutoMapper;
using blog_backend.ApplicationMapper;
using blog_backend.DAO.IService;
using blog_backend.Service;

namespace blog_backend.Configuration;

public static class ServiceConfiguration
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<AccountService>();
        services.AddScoped<CommentService>();
        services.AddScoped<CommunityService>();
        services.AddScoped<PostService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<TagsService>();
        services.AddScoped<GenerateTokenService>();
        services.AddScoped<IGarService, GarService>();
    }
}