using blog_backend.DAO.Database;
using blog_backend.DAO.Utils;
using blog_backend.Entity;
using blog_backend.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Repository;

public class CommunityRepository : ICommunityRepository
{
    private readonly BlogDbContext _databaseContext;

    public CommunityRepository(BlogDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<string>? GetUserRoleInCommunity(Guid userId, Guid communityId)
    {
        var membership = _databaseContext.CommunityMemberships
            .FirstOrDefault(membership => membership.UserId == userId && membership.CommunityId == communityId);
        return Task.FromResult(membership?.RoleEnum.ToString() ?? null);
    }

    public async Task CreateCommunityAsync(Community community)
    {
        _databaseContext.Communities.Add(community);
         await _databaseContext.SaveChangesAsync();
    }

    public async Task<Community?> GetCommunityById(Guid communityId)
    {
        return await _databaseContext.Communities
            .Include(c => c.Memberships)!
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c => c.Id == communityId);
    }
    
    public Task<bool> IsUserSubscribedToCommunity(Guid userId, Guid communityId)
    {
        return  Task.FromResult(_databaseContext.CommunityMemberships
            .Any(membership => membership.UserId == userId && membership.CommunityId == communityId));
    }

    public async Task SubscribeUserToCommunity(Community community, User user)
    {
        community.SubscribersCount++;
        community.Memberships?.Add(new CommunityMembership
        {
            UserId = user.Id,
            RoleEnum = RoleEnum.Subscriber
        });
        await _databaseContext.SaveChangesAsync();
    }
    
    public Task UnSubscribeUserFromCommunity(Community community, User user)
    {
        community.SubscribersCount--;
        var membership = community.Memberships?.FirstOrDefault(membership => membership.UserId == user.Id);
        if (membership != null)
        {
            community.Memberships?.Remove(membership);
        }
        return Task.CompletedTask;
    }
}