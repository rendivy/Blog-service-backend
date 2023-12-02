using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

[Route("api/post")]
public class PostController : GlobalController
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var post = await _postService.GetPostDetails(id, new Guid(userId));
        if (post == null)
        {
            var error = new ErrorDTO { Message = "Post not found", Status = NotFound().StatusCode.ToString() };
            return NotFound(error);
        }

        return Ok(post);
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