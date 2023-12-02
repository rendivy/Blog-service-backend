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

    public Task<string?> GetUserRoleInCommunity(Guid userId, Guid communityId)
    {
        var membership = _databaseContext.CommunityMemberships
            .FirstOrDefault(membership => membership.UserId == userId && membership.CommunityId == communityId);
        return Task.FromResult(membership?.RoleEnum.ToString() ?? null);
    }

    public async Task SaveChangesAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public Task CreatePostInCommunity(Community community, Post post)
    {
        community.Posts?.Add(post);
        post.CommunityId = community.Id;
        post.CommunityName = community.Name;
        return Task.CompletedTask;
    }

    public async Task<List<Community>> GetCommunityList()
    {
        return await Task.FromResult(_databaseContext.Communities.ToList());
    }

    public Task<List<Community>> GetUserCommunityList(Guid userId)
    {
        return Task.FromResult(_databaseContext.Communities
            .Include(c => c.Memberships)!
            .ThenInclude(m => m.User)
            .Where(c => c.Memberships!.Any(m => m.UserId == userId))
            .ToList());
    }

    public Task CreateCommunityAsync(Community community)
    {
        _databaseContext.Communities.AddAsync(community);
        return Task.CompletedTask;
    }

    public async Task<Community?> GetCommunityById(Guid? communityId)
    {
        return await _databaseContext.Communities
            .Include(c => c.Memberships)
            .ThenInclude(m => m.User)
            .Include(c => c.Posts)
            .ThenInclude(p => p.Tags)
            .Include(c => c.Posts)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.User) // Assuming you have a navigation property named User in your Comment class
            .FirstOrDefaultAsync(c => c.Id == communityId);
    }

    public Task<bool> IsUserSubscribedToCommunity(Guid userId, Guid communityId)
    {
        return Task.FromResult(_databaseContext.CommunityMemberships
            .Any(membership => membership.UserId == userId && membership.CommunityId == communityId));
    }

    public Task SubscribeUserToCommunity(Community community, User user)
    {
        community.SubscribersCount++;
        community.Memberships?.Add(new CommunityMembership
        {
            UserId = user.Id,
            RoleEnum = RoleEnum.Subscriber
        });
        return Task.CompletedTask;
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