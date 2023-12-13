using System.ComponentModel;
using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Enums;
using blog_backend.Enums;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class CommunityController : GlobalController
{
    private readonly CommunityService _communityService;

    public CommunityController(CommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpGet("community")]
    public ActionResult<List<CommunityShortDTO>> GetCommunityList()
    {
        try
        {
            var communities = _communityService.GetCommunityList();
            return Ok(communities.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
    
    [HttpGet("community/{communityId}/post")]
    public async Task<IActionResult> GetCommunityPostList(Guid communityId,
        [FromQuery] List<string>? tags,
        [FromQuery] SortingEnum? sorting = SortingEnum.CreateAsc,
        [FromQuery] int page = 1,
        [FromQuery] int size = 5)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var community = await _communityService.GetPostsWithPagination(communityId, tags, sorting, userId, page, size);
            return Ok(community);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [HttpPost("community/{communityId}/subscribe")]
    [Authorize]
    public async Task<IActionResult> SubscribeUserToCommunity([FromRoute] string communityId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _communityService.SubscribeUserToCommunity(communityId, userId!);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO
            {
                Message = e.Message,
                Status = BadRequest().StatusCode.ToString()
            };
            return BadRequest(error);
        }
    }

    [HttpDelete("community/{communityId}/subscribe")]
    [Authorize]
    public async Task<IActionResult> UnSubscribeUserToCommunity([FromRoute] string communityId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _communityService.UnSubscribeUserToCommunity(communityId, userId!);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO
            {
                Message = e.Message,
                Status = BadRequest().StatusCode.ToString()
            };
            return BadRequest(error);
        }
    }


    [HttpGet("community/{communityId}/role")]
    [Authorize]
    public ActionResult<string> GetUserRoleInCommunity([FromRoute] string communityId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = _communityService.GetUserRoleInCommunity(communityId, userId!);
            return Ok(role?.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [HttpGet("community/{id}")]
    [Description("Get community by id")]
    public ActionResult<CommunityDetailsDTO> GetCommunityDetails([FromRoute] string id)
    {
        try
        {
            var community = _communityService.GetCommunityDetails(id);
            return Ok(community.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [Authorize]
    [HttpPost("community/{communityId}/post")]
    public async Task<IActionResult> CreatePostInCommunity([FromRoute] string communityId,
        [FromBody] CreatePostDTO postDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) await _communityService.CreatePostInCommunity(userId, postDto, communityId);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO
            {
                Message = e.Message,
                Status = BadRequest().StatusCode.ToString()
            };
            return BadRequest(error);
        }
    }


    [HttpPost("community")]
    [Authorize]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityDTO communityDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            await _communityService.CreateCommunityAsync(communityDto, userId!);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO
            {
                Message = e.Message,
                Status = BadRequest().StatusCode.ToString()
            };
            return BadRequest(error);
        }
    }


    [HttpGet("community/my")]
    [Authorize]
    public ActionResult<List<CommunityShortDTO>> GetUserCommunityList()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var communities = _communityService.GetUserCommunityList(userId!);
            return Ok(communities.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
}