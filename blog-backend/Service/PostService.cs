using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Enums;
using blog_backend.Enums;
using blog_backend.Service.Extensions;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;
using Microsoft.EntityFrameworkCore;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly BlogDbContext _blogDbContext;
    private readonly ICommunityRepository _communityRepository;

    public PostService(IPostRepository postRepository, IAccountRepository accountRepository,
        ICommunityRepository communityRepository, BlogDbContext blogDbContext)
    {
        _postRepository = postRepository;
        _accountRepository = accountRepository;
        _communityRepository = communityRepository;
        _blogDbContext = blogDbContext;
    }


    public async Task<List<PostDetailsDTO>> GetPostWithPagination(Guid userId, int size, int page, string author,
        int? maximumReadingTime, int? minimumReadingTime, SortingEnum sorting, bool onlyMyCommunities)
    {
        var posts = _blogDbContext.Posts.AsQueryable();
        var user = await userId.ToString().GetUserById(_blogDbContext);
        //sort by author name
        if (author != null)
        {
            posts = posts.Where(p => p.Author == author);
        }

        if (minimumReadingTime != null)
        {
            posts = posts.Where(p => p.ReadingTime >= minimumReadingTime);
        }

        if (maximumReadingTime != null)
        {
            posts = posts.Where(p => p.ReadingTime <= maximumReadingTime);
        }
        
        if (onlyMyCommunities)
        {
            var userCommunities = _blogDbContext.Communities.Where(c => c.Memberships.Any(s => s.UserId == user.Id));
            posts = posts.Where(p => userCommunities.Any(c => c.Id == p.CommunityId));
        }
        
        posts = sorting switch
        {
            SortingEnum.CreateDesc => posts.OrderByDescending(post => post.CreateTime),
            SortingEnum.CreateAsc => posts.OrderBy(post => post.CreateTime),
            SortingEnum.LikeAsc => posts.OrderBy(post => post.Likes),
            SortingEnum.LikeDesc => posts.OrderByDescending(post => post.Likes),
            _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null)
        };
        
        var closedCommunityIds = _blogDbContext.Communities.Where(c => c.IsClosed).Select(c => c.Id).ToList();
        var userCommunityIds = _blogDbContext.CommunityMemberships.Where(cm => cm.UserId == userId).Select(cm => cm.CommunityId).ToList();

        posts = posts.Where(p => !closedCommunityIds.Contains(p.CommunityId.Value) || userCommunityIds.Contains(p.CommunityId.Value));
        
        

        var postsList = await posts.ToListAsync();
        var postWithDetails = new List<PostDetailsDTO>();
        foreach (var post in postsList)
        {
            var details = await GetPostDetails(post.Id, userId);
            if (details != null)
            {
                postWithDetails.Add(details);
            }
        }

        return postWithDetails;
    }

    public async Task<PostDetailsDTO?> GetPostDetails(Guid postId, Guid userId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        if (post == null)
        {
            return null;
        }

        var community = await _communityRepository.GetCommunityById(post.CommunityId);
        if (post.CommunityId != null)
        {
            if (community.IsClosed)
            {
                var isUserMember = _blogDbContext.CommunityMemberships.Any(cm =>
                    cm.UserId == userId && cm.CommunityId == community.Id);
                if (!isUserMember)
                {
                    throw new Exception("User is not a member of this community");
                }
            }
        }
        
        var comment = post.Comments.Where(c => c.CommentParent == null).Select(CommentMapper.Map).ToList();

        var postDto = new PostDetailsDTO
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            AuthorId = post.AuthorId,
            CommunityName = community?.Name,
            CommunityId = community?.Id,
            Author = post.Author,
            AddressId = post.AddressId,
            Comments = comment,
            Likes = post.LikedUsers.Count,
            HasLike = post.LikedUsers.Any(u => u.Id == userId),
            Tags = post.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                CreateTime = t.CreateTime
            }).ToList()
        };
        return postDto;
    }


    public async Task CreatePost(CreatePostDTO postDto, Guid userId)
    {
        var user = await _accountRepository.GetUserById(userId.ToString());
        var tags = await _postRepository.GetTags(postDto);
        var post = PostMapper.Map(postDto, tags, user.FullName, userId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        await _postRepository.CreatePost(post);
    }


    public async Task LikePost(Guid userId, Guid postId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        var user = await _accountRepository.GetUserById(userId.ToString());
        var community = post?.CommunityId != null
            ? await _communityRepository.GetCommunityById(post.CommunityId.Value)
            : null;
        if (community is { IsClosed: true })
        {
            var isUserMember = _blogDbContext.CommunityMemberships.Any(cm =>
                cm.UserId == userId && cm.CommunityId == community.Id);
            if (!isUserMember)
            {
                throw new Exception("User is not a member of this community");
            }
        }
        var isUserLiked = post?.LikedUsers?.Any(u => u.Id == userId);

        if (isUserLiked == null || isUserLiked.Value)
        {
            throw new Exception("User already liked this post");
        }

        if (post == null || user == null)
        {
            throw new Exception("Post or user not found");
        }

        await _postRepository.LikePost(post, user);
    }


    public async Task UnlikePost(Guid userId, Guid postId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        var user = await _accountRepository.GetUserById(userId.ToString());
        var community = post?.CommunityId != null
            ? await _communityRepository.GetCommunityById(post.CommunityId.Value)
            : null;
        if (community is { IsClosed: true })
        {
            var isUserMember = _blogDbContext.CommunityMemberships.Any(cm =>
                cm.UserId == userId && cm.CommunityId == community.Id);
            if (!isUserMember)
            {
                throw new Exception("User is not a member of this community");
            }
        }
        var isUserLiked = post?.LikedUsers?.Any(u => u.Id == userId);
        if (isUserLiked == null || !isUserLiked.Value)
        {
            throw new Exception("User not liked this post");
        }

        if (post == null || user == null)
        {
            throw new Exception("Post or user not found");
        }

        await _postRepository.UnlikePost(post, user);
    }
}