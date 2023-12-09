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
        await _tagsService.CreateTag(tagDto); 
        return Ok();
        
    }


    [HttpGet("tags")]
    public ActionResult<List<TagDTO>> GetTags()
    {
        return Ok(_tagsService.GetTags().Result);
    }
}