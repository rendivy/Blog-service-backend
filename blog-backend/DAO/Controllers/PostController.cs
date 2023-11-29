using System.Security.Claims;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
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
    

    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost([FromRoute] Guid postId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _postService.LikePost(new Guid(userId), postId);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _postService.CreatePost(postDto, new Guid(userId));
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
}