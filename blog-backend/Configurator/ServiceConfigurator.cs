using blog_backend.Service;

namespace blog_backend.Configurator;

public static class ServiceConfigurator
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
    }
}