using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface ICommunityRepository
{
    
    public Task<string>? GetUserRoleInCommunity(Guid userId, Guid communityId);
    
    public Task CreateCommunityAsync(Community community);

    public Task<bool> IsUserSubscribedToCommunity(Guid userId, Guid communityId);
    
    public Task<Community?> GetCommunityById(Guid communityId);
    
    public Task SubscribeUserToCommunity(Community community, User user);
    
    public Task UnSubscribeUserFromCommunity(Community community, User user);
}