using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Comment;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class CommentController : GlobalController
{
    private readonly CommentService _commentService;
    private const string CommentNotFound = "Comment not found";
    private const string UnauthorizedUser = "You are not the author of this comment";

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }


    [HttpGet]
    [Route("comment/{commentId}/tree")]
    public ActionResult<List<ErrorDTO>> GetAllNestedComments([FromRoute] string commentId)
    {
        var commentTree = _commentService.GetNestedComments(commentId).Result;
        if (commentTree.Count == 0) return Ok("No nested comment");
        return Ok(commentTree);
    }

    [HttpDelete]
    [Authorize]
    [Route("comment/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] string commentId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _commentService.DeleteComment(commentId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            switch (e.Message)
            {
                case CommentNotFound:
                    return NotFound(new ErrorDTO { Message = e.Message, Status = NotFound().StatusCode.ToString() });
                case UnauthorizedUser:
                    return BadRequest(new ErrorDTO
                        { Message = e.Message, Status = BadRequest().StatusCode.ToString() });
            }

            return BadRequest(new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() });
        }
    } 
    
    
    
    [HttpPut("comment/{commentId}")]
    [Authorize]
    public async Task<IActionResult> EditComment([FromRoute] string commentId, [FromBody] EditCommentDTO editCommentDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _commentService.EditComment(editCommentDto, commentId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            switch (e.Message)
            {
                case CommentNotFound:
                    return NotFound(new ErrorDTO { Message = e.Message, Status = NotFound().StatusCode.ToString() });
                case UnauthorizedUser:
                    return BadRequest(new ErrorDTO
                        { Message = e.Message, Status = BadRequest().StatusCode.ToString() });
            }

            return BadRequest(new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() });
        }
    }


    [HttpPost]
    [Authorize]
    [Route("comment/{postId}")]
    public async Task<IActionResult> CreateComment([FromRoute] Guid postId, [FromBody] AddCommentDTO addCommentDto)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        await _commentService.CreateComment(addCommentDto, postId.ToString(), userId);
        return Ok();
    }
}