using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.CommunityEntities;
using blog_backend.Entity.PostEntities;

namespace blog_backend.Service.Repository;

public interface ICommunityRepository
{
    
    public Task<string?> GetUserRoleInCommunity(Guid userId, Guid communityId);
    
    
    public Task SaveChangesAsync();
    
    public Task CreatePostInCommunity(Community community, Post post);
    
    public Task<List<Community>> GetCommunityList();
    
    public Task<List<Community>> GetUserCommunityList(Guid userId);
    
    public Task CreateCommunityAsync(Community community);

    public Task<bool> IsUserSubscribedToCommunity(Guid userId, Guid communityId);
    
    public Task<Community?> GetCommunityById(Guid? communityId);
    
    public Task SubscribeUserToCommunity(Community community, User user);
    
    public Task UnSubscribeUserFromCommunity(Community community, User user);
}