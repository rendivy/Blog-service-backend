using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface IPostRepository
{
    //getPostDetails 
    
    public Task<Post?> GetPostDetails(Guid postId);
    
    public Task CreatePost(Post post);

    public Task LikePost(Guid postId, Guid userId);
    
    public Task UnlikePost(Post post, User user);


}