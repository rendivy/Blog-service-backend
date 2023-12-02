using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;


public class TagController : GlobalController
{
    private readonly TagsService _tagsService;

    public TagController(TagsService tagsService)
    {
        _tagsService = tagsService;
    }
    
    [HttpPost("tags")]
    public async Task<ActionResult> CreateTag([FromBody] CreateTagDTO tagDto)
    {
        try
        {
            await _tagsService.CreateTag(tagDto);
            return Ok();
        }
        catch (Exception e)
        {
            var statusCode = BadRequest().StatusCode.ToString();
            return BadRequest(new ErrorDTO { Message = e.Message, Status = statusCode });
        }
    }


    [HttpGet("tags")]
    public ActionResult<List<TagDTO>> GetTags()
    {
        try
        {
            return Ok(_tagsService.GetTags().Result);
        }
        catch (Exception e)
        {
            var statusCode = BadRequest().StatusCode.ToString();
            return BadRequest(new ErrorDTO { Message = e.Message, Status = statusCode });
        }
    }
}