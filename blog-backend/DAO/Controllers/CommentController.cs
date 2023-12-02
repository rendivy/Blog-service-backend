using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class CommentController : GlobalController
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }


    [HttpGet]
    [Route("comment/{commentId}/tree")]
    public ActionResult<List<ErrorDTO>> GetAllNestedComments([FromRoute] string commentId)
    {
        try
        {
            var commentTree = _commentService.GetNestedComments(commentId).Result;
            if (commentTree.Count == 0) return Ok("No nested comment");
            return Ok(commentTree);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO {Message = e.Message, Status = BadRequest().StatusCode.ToString()};
            return BadRequest(error);
        }
    }


    [HttpPost]
    [Authorize]
    [Route("comment/{postId}")]
    public async Task<IActionResult> CreateComment([FromRoute] Guid postId, [FromBody] AddCommentDTO addCommentDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _commentService.CreateComment(addCommentDto, postId.ToString(), userId);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO {Message = e.Message, Status = BadRequest().StatusCode.ToString()};
            return BadRequest(error);
        }
    }
}