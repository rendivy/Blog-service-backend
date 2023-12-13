using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Enums;
using blog_backend.Entity.PostEntities;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommunityService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly PostService _postService;
    private readonly IPostRepository _postRepository;

    public CommunityService(ICommunityRepository communityRepository, IAccountRepository accountRepository,
        IPostRepository postRepository, ICommentRepository commentRepository, PostService postService)
    {
        _communityRepository = communityRepository;
        _accountRepository = accountRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _postService = postService;
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
        var status = _communityRepository.CreatePostInCommunity(communities.First(c => c.Id == new Guid(communityId)),
            PostMapper.Map(post, tags, user.FullName, userId));
        if (status.IsCompleted)
        {
            return _communityRepository.SaveChangesAsync();
        }

        throw new ArgumentException("Unknown error");
    }


    public async Task<List<CommunityListDTO>> GetUserCommunityList(string userId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));

        var user = await _accountRepository.GetUserById(userId);
        if (user == null) throw new ArgumentException("User not found");

        var communities = await _communityRepository.GetUserCommunityList(user.Id);

        var userList = new List<CommunityListDTO>();
        foreach (var community in communities)
        {
            var userRoleInCommunity = await _communityRepository.GetUserRoleInCommunity(user.Id, community.Id);
            var communityDto = CommunityMapper.MapToList(community, userId, userRoleInCommunity);
            userList.Add(communityDto);
        }

        return userList;
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
        string userId, int page = 1, int size = 5)
    {
        var community = await _communityRepository.GetCommunityById(communityId);
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        if (community == null) throw new ArgumentException("Community not found");
        if (community.IsClosed)
        {
            var userRole = await _communityRepository.GetUserRoleInCommunity(new Guid(userId), communityId);
            if (userRole == null)
            {
                throw new ArgumentException("This community is closed and user are not subscribed to it");
            }
        }


        var postsList = community.Posts!.AsQueryable();
        if (tags is { Count: > 0 })
        {
            postsList = postsList.Where(post => post.Tags!.Any(tag => tags.Contains(tag.Id.ToString())));
        }

        var sortedPostsList = sorting switch
        {
            SortingEnum.CreateDesc => postsList.OrderByDescending(post => post.CreateTime),
            SortingEnum.CreateAsc => postsList.OrderBy(post => post.CreateTime),
            SortingEnum.LikeAsc => postsList.OrderBy(post => post.Likes),
            SortingEnum.LikeDesc => postsList.OrderByDescending(post => post.Likes),
            _ => throw new ArgumentException("No argument exception!")
        };
        var postWithDetails = new List<PostDetailsDTO>();
        foreach (var post in sortedPostsList)
        {
            var postDetails = await _postService.GetPostDetails(post.Id, new Guid(userId));
            if (postDetails != null) postWithDetails.Add(postDetails);
        }

        var paginatedData = postWithDetails.Skip((page - 1) * size).Take(size);
        var pagination = new PaginationDTO
        {
            Page = page,
            Size = size,
            Current = paginatedData.Count(),
        };
        return CommunityMapper.MapCommunityDto(postWithDetails, pagination);
    }


    public IQueryable<Post> getPostsFromCommunity(Guid communityId)
    {
        var community = _communityRepository.GetCommunityById(communityId).Result;
        if (community == null) throw new ArgumentException("Community not found");
        return community.Posts!.AsQueryable();
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
        var status = _communityRepository.CreateCommunityAsync(community);
        if (status.IsCompletedSuccessfully)
        {
            return _communityRepository.SaveChangesAsync();
        }

        throw new ArgumentException("Unknown error");
    }
}