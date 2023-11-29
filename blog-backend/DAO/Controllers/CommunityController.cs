using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

[ApiController]
[Route("api")]
public class CommunityController : Controller
{
    private readonly CommunityService _communityService;

    public CommunityController(CommunityService communityService)
    {
        _communityService = communityService;
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
    
    
    [HttpGet("community/{communityId}/role")]
    [Authorize]
    public ActionResult<string> GetUserRoleInCommunity([FromRoute] string communityId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = _communityService.GetUserRoleInCommunity(communityId, userId!);
            return Ok(role.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
    


    [HttpGet("community/{id}")]
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
}