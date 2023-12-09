using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Middleware;
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
    public Task LogoutUser()
    {
        var tokenId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.SerialNumber)?.Value;
        return _accountService.LogoutUser(tokenId ?? string.Empty);
    }

    [HttpPost("register")]
    [HandleExceptions] 
    public ActionResult<User> Register([FromBody] AuthorizationDTO request)
    {
        var token = _accountService.RegisterUser(request);
        return Ok(token.Result);
    }

    [HttpPut("profile")]
    [Authorize]
    [HandleExceptions] 
    public async Task<IActionResult> EditUser([FromBody] EditAccountDTO user)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        await _accountService.EditUser(user, userEmail ?? string.Empty, userId ?? string.Empty);
        return Ok();
    }

    [HttpPost("login")]
    [HandleExceptions] 
    public ActionResult<TokenDTO> Login([FromBody] LoginDTO request)
    {
        var token = _accountService.LoginUser(request);
        return Ok(new TokenDTO { Token = token });
    }
}