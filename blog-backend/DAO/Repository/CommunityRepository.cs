using blog_backend.DAO.Database;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.CommunityEntities;
using blog_backend.Entity.PostEntities;
using blog_backend.Enums;
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
            .Include(community => community.Memberships)
            .ThenInclude(membership => membership.User)
            .Include(community => community.Posts)
            .ThenInclude(post => post.Tags)
            .Include(community => community.Posts)
            .ThenInclude(post => post.Comments)
            .ThenInclude(comment => comment.User).AsQueryable()
            .FirstOrDefaultAsync(community => community.Id == communityId);
    }

    public Task<bool> IsUserSubscribedToCommunity(Guid userId, Guid communityId)
    {
        return Task.FromResult(_databaseContext.CommunityMemberships
            .Any(membership => membership.UserId == userId && membership.CommunityId == communityId));
    }

    public async Task SubscribeUserToCommunity(Community community, User user)
    {
        community.SubscribersCount++;

        if (community.Memberships == null)
        {
            community.Memberships = new List<CommunityMembership>();
        }

        community.Memberships.Add(new CommunityMembership
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
        _databaseContext.CommunityMemberships.Remove(membership!);
        _databaseContext.SaveChangesAsync();
        return Task.CompletedTask;
    }
}