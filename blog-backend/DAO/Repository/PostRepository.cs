using blog_backend.DAO.Database;
using blog_backend.Entity;
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
    

    public async Task<Post?> GetPostDetails(Guid postId)
    {
        return await _databaseContext.Posts.Include(p => p.Tags).Include(post => post.LikedUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public Task CreatePost(Post post)
    {
        throw new NotImplementedException();
    }

    public Task LikePost(Guid postId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task UnlikePost(Post post, User user)
    {
        post.LikedUsers?.Remove(user);
        user.LikedPosts?.Remove(post);
        post.Likes--;
        await _databaseContext.SaveChangesAsync();
    }
 
}