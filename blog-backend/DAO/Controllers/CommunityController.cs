using System.ComponentModel;
using System.Security.Claims;
using blog_backend.DAO.Model;
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
    [Description("Get all communities")]
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

    [HttpPost("community/{communityId}/subscribe")]
    [Authorize]
    [Description("Subscribe user to community")]
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
    [Description("Unsubscribe user from community")]
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
    [Description("Get user role in community")]
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
    [Description("Create post in community")]
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
    [Description("Create community")]
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
    [Description("Get communities where user is subscribed with greatest role")]
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