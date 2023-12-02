using blog_backend.DAO.Repository;
using blog_backend.Service.Repository;

namespace blog_backend.Configurator;

public static class RepositoryConfigurator
{
    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagsRepository, TagsRepository>();
    }
}