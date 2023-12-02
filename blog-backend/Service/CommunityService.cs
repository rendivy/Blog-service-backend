using blog_backend.DAO.Model;
using blog_backend.DAO.Utils;
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
        var userRole = _communityRepository.GetUserRoleInCommunity(user.Id, new Guid(communityId))?.Result;
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
        var status = Task.FromResult(communities.Select(community =>
            CommunityMapper.MapToList(community, userId,
                _communityRepository.GetUserRoleInCommunity(user.Id, community.Id).Result!)).ToList());
        if (!status.IsCompleted) throw new ArgumentException("Unknown error");
        return status;
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

        var status = _communityRepository.SubscribeUserToCommunity(community, user);
        if (!status.IsCompleted) throw new ArgumentException("Unknown error");
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

    public async Task<CommunityDTO> GetPostsWithPagination(Guid communityId, List<string>? tags, SortingEnum? sorting,
        int page = 1, int size = 5)
    {
        var community = await _communityRepository.GetCommunityById(communityId);
        if (community == null) throw new ArgumentException("Community not found");

        var postsList = community.Posts!;

        if (tags != null && tags.Any())
        {
            postsList = postsList.Where(post => post.Tags?.Any(tag => tags.Contains(tag.Name)) ?? false).ToList();
        }

        var sortedPostsList = sorting switch
        {
            SortingEnum.CreateDesc => postsList.OrderByDescending(post => post.CreateTime).ToList(),
            SortingEnum.CreateAsc => postsList.OrderBy(post => post.CreateTime).ToList(),
            SortingEnum.LikeAsc => postsList.OrderBy(post => post.Likes).ToList(),
            SortingEnum.LikeDesc => postsList.OrderByDescending(post => post.Likes).ToList(),
            _ => postsList
        };
        var paginatedData = sortedPostsList.Skip((page - 1) * size).Take(size).ToList();
        var pagination = new PaginationDTO
        {
            Page = page,
            Size = size,
            Current = paginatedData.Count,
        };
        return CommunityMapper.MapCommunityDto(paginatedData.Select(PostMapper.MapDetails).ToList(), pagination);
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
        _communityRepository.CreateCommunityAsync(community);
        _communityRepository.SaveChangesAsync();
        return Task.CompletedTask;
    }
}