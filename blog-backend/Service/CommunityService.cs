using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommunityService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IAccountRepository _accountRepository;

    public CommunityService(ICommunityRepository communityRepository, IAccountRepository accountRepository)
    {
        _communityRepository = communityRepository;
        _accountRepository = accountRepository;
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

        return _communityRepository.SubscribeUserToCommunity(community, user);
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

    public Task CreateCommunityAsync(CreateCommunityDTO communityDto, string userId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var user = _accountRepository.GetUserById(userId).Result;
        var community = CommunityMapper.Map(communityDto, user!);
        return _communityRepository.CreateCommunityAsync(community);
    }
}