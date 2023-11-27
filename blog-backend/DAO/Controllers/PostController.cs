using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Controllers;


[ApiController]
public class PostController : Controller
{
    private readonly BlogDbContext _blogDbContext;

    public PostController(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }


    [Authorize]
    [HttpPost("api/post")]
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