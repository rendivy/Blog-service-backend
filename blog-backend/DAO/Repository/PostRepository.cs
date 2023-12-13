using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.PostEntities;
using blog_backend.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Repository;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _databaseContext;

    public PostRepository(BlogDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    private async Task<Post?> GetPostById(Guid postId)
    {
        return await _databaseContext.Posts
            .Include(p => p.LikedUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }


    public Task<Post?> GetPostDetails(Guid postId)
    {
        return _databaseContext.Posts
            .Include(p => p.Tags)
            .Include(post => post.LikedUsers)
            .Include(comment => comment.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<List<Tag>> GetTags(CreatePostDTO postDto)
    {
        return await _databaseContext.Tags.Where(e => postDto.Tags.Contains(e.Id)).ToListAsync();
    }

    public async Task CreatePost(Post post)
    {
        await _databaseContext.Posts.AddAsync(post);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<Post>> GetPostsByAuthor(string authorId)
    {
        return await _databaseContext.Posts.Where(p => p.AuthorId.ToString() == authorId).ToListAsync();
    }

    public async Task LikePost(Post post, User user)
    {
        post.LikedUsers?.Add(user);
        user.LikedPosts?.Add(post);
        post.Likes++;
        await _databaseContext.SaveChangesAsync();
    }

    public async Task UnlikePost(Post post, User user)
    {
        post.LikedUsers?.Remove(user);
        user.LikedPosts?.Remove(post);
        post.Likes--;
        await _databaseContext.SaveChangesAsync();
    }
}