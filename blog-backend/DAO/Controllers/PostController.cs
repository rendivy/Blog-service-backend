using System.Security.Claims;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Controllers;

[ApiController]
[Route("api/post")]
public class PostController : Controller
{
    private readonly BlogDbContext _blogDbContext;
    private readonly PostService _postService;

    public PostController(BlogDbContext blogDbContext, PostService postService)
    {
        _blogDbContext = blogDbContext;
        _postService = postService;
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var post = await _postService.GetPostDetails(id, new Guid(userId));
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [Authorize]
    [HttpDelete("{postId}/like")]
    public async Task<IActionResult> DeleteLikePost([FromRoute] Guid postId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _postService.UnlikePost(new Guid(userId), postId);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }

    //TODO отправлять dto с hasLike 

    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost([FromRoute] Guid postId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        var post = await _blogDbContext.Posts
            .Include(p => p.LikedUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        var user = await _blogDbContext.User
            .Include(u => u.LikedPosts)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (post == null || user == null)
        {
            return NotFound();
        }

        if (post.LikedUsers != null && post.LikedUsers.Contains(user))
        {
            var error = new ErrorDTO
                { Message = "You already have liked this post.", Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }

        post.LikedUsers.Add(user);
        user.LikedPosts.Add(post);
        post.Likes++;
        await _blogDbContext.SaveChangesAsync();
        return Ok();
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userName = _blogDbContext.User.FirstOrDefault(u => u.Id.ToString() == userId).FullName;
        var post = new Post
        {
            Title = postDto.Title,
            Description = postDto.Description,
            ReadingTime = postDto.ReadingTime,
            Image = postDto.Image,
            Author = userName,
            AuthorId = new Guid(userId),
            Tags = await _blogDbContext.Tags.Where(e => postDto.Tags.Contains(e.Id)).ToListAsync()
        };
        await _blogDbContext.Posts.AddAsync(post);
        await _blogDbContext.SaveChangesAsync();
        return Ok();
    }
}