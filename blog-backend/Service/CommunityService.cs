using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.DAO.Utils;
using blog_backend.Entity;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommunityService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IPostRepository _postRepository;

    public CommunityService(ICommunityRepository communityRepository, IAccountRepository accountRepository,
        IPostRepository postRepository)
    {
        _communityRepository = communityRepository;
        _accountRepository = accountRepository;
        _postRepository = postRepository;
    }

    public Task UnSubscribeUserToCommunity(string communityId, string userId)
    {
        if (communityId == null) throw new ArgumentNullException(nameof(communityId));
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var community = _communityRepository.GetCommunityById(new Guid(communityId)).Result;
        var user = _accountRepository.GetUserById(userId).Result;

        if (community == null || user == null)
        {
            throw new ArgumentException("Community or user not found");
        }

        if (!_communityRepository.IsUserSubscribedToCommunity(user.Id, community.Id).Result)
        {
            throw new ArgumentException("User is not subscribed to this community");
        }

        return _communityRepository.UnSubscribeUserFromCommunity(community, user);
    }

    public Task<CommunityDetailsDTO> GetCommunityDetails(string communityId)
    {
        if (communityId == null) throw new ArgumentNullException(nameof(communityId));
        var community = _communityRepository.GetCommunityById(new Guid(communityId)).Result;
        if (community == null) throw new ArgumentException("Community not found");
        return Task.FromResult(CommunityMapper.MapToDetails(community));
    }


    public Task CreatePostInCommunity(string userId, CreatePostDTO post, string communityId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var user = _accountRepository.GetUserById(userId).Result;
        if (user == null) throw new ArgumentException("User not found");
        var userRole = _communityRepository.GetUserRoleInCommunity(user.Id, new Guid(communityId)).Result;
        var communities = _communityRepository.GetUserCommunityList(user.Id).Result;
        if (userRole is not "Administrator")
        {
            throw new ArgumentException("User is not admin of this community");
        }

        var tags = _postRepository.GetTags(post).Result;
        _communityRepository.CreatePostInCommunity(communities.First(c => c.Id == new Guid(communityId)),
            PostMapper.Map(post, tags, user.FullName, userId));
        _communityRepository.SaveChangesAsync();
        return Task.CompletedTask;
    }


    public Task<List<CommunityListDTO>> GetUserCommunityList(string userId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var user = _accountRepository.GetUserById(userId).Result;
        if (user == null) throw new ArgumentException("User not found");
        var communities = _communityRepository.GetUserCommunityList(user.Id).Result;
        return Task.FromResult(communities.Select(community =>
            CommunityMapper.MapToList(community, userId,
                _communityRepository.GetUserRoleInCommunity(user.Id, community.Id).Result!)).ToList());
    }


    public Task SubscribeUserToCommunity(string communityId, string userId)
    {
        if (communityId == null) throw new ArgumentNullException(nameof(communityId));
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var community = _communityRepository.GetCommunityById(new Guid(communityId)).Result;
        var user = _accountRepository.GetUserById(userId).Result;

        if (community == null || user == null)
        {
            throw new ArgumentException("Community or user not found");
        }

        if (_communityRepository.IsUserSubscribedToCommunity(user.Id, community.Id).Result)
        {
            throw new ArgumentException("User is already subscribed to this community");
        }

        _communityRepository.SubscribeUserToCommunity(community, user);
        _communityRepository.SaveChangesAsync();
        return Task.CompletedTask;
    }


    public Task<string>? GetUserRoleInCommunity(string communityId, string userId)
    {
        if (communityId == null) throw new ArgumentNullException(nameof(communityId));
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var community = _communityRepository.GetCommunityById(new Guid(communityId)).Result;
        var user = _accountRepository.GetUserById(userId).Result;

        if (community == null)
        {
            throw new ArgumentException("Community not found");
        }

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        if (!_communityRepository.IsUserSubscribedToCommunity(user.Id, community.Id).Result)
        {
            return null;
        }

        return _communityRepository.GetUserRoleInCommunity(user.Id, community.Id);
    }

    public async Task<List<CommunityShortDTO>> GetCommunityList()
    {
        var communities = await _communityRepository.GetCommunityList();
        return communities.Select(CommunityMapper.MapToShort).ToList();
    }

    public Task CreateCommunityAsync(CreateCommunityDTO communityDto, string userId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var user = _accountRepository.GetUserById(userId).Result;
        var community = CommunityMapper.Map(communityDto, user!);
        return _communityRepository.CreateCommunityAsync(community);
    }
}