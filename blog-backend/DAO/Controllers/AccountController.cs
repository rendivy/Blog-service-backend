using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

[ApiController]
public class AccountController : GlobalController
{
    private readonly AccountService _accountService;

    public AccountController(IAccountRepository accountRepository, GenerateTokenService tokenService)
    {
        _accountService = new AccountService(accountRepository, tokenService);
    }


    [HttpGet("profile")]
    [Authorize]
    public ActionResult<User?> GetUserInfo()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return Ok(_accountService.GetUserInfo(userId ?? string.Empty).Result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task LogoutUser()
    {
        var tokenId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.SerialNumber)?.Value;
        await _accountService.LogoutUser(tokenId ?? string.Empty);
    }

    [HttpPost("register")]
    public ActionResult<User> Register([FromBody] AuthorizationDTO request)
    {
        try
        {
            var token = _accountService.RegisterUser(request);
            return Ok(token.Result);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message };
            return BadRequest(error);
        }
    }


    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> EditUser([FromBody] EditAccountDTO user)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            await _accountService.EditUser(user, userEmail ?? string.Empty, userId ?? string.Empty);
            return Ok();
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }


    [HttpPost("login")]
    public ActionResult<TokenDTO> Login([FromBody] LoginDTO request)
    {
        try
        {
            var token = _accountService.LoginUser(request);
            return Ok(new TokenDTO { Token = token });
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
}