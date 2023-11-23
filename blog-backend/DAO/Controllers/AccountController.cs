using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

[ApiController]
[Route("api")]
public class AccountController : Controller
{
    private readonly AccountService _accountService;
    private readonly IConfiguration _configuration;
    
    public AccountController(IAccountRepository accountRepository, GenerateTokenService tokenService, IConfiguration configuration)
    {
        _configuration = configuration;
        _accountService = new AccountService(accountRepository, tokenService);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task LogoutUser()
    { 
        var tokenId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.SerialNumber)?.Value;
        await _accountService.LogoutUser(tokenId);
    }


    [HttpPost("register")]
    public ActionResult<User> Register(AuthorizationDTO request)
    {
        try
        {
            var token = _accountService.RegisterUser(request);
            return Ok(new TokenDTO { Token = token });
        }
        catch (ArgumentException e)
        {
            var error = new ErrorDTO { Message = e.Message };
            return BadRequest(error);
        }
    }


    [HttpPut("profile")]
    [Authorize]
    public async Task EditUser(EditAccountDTO user)
    {
        await _accountService.EditUser(user);
    }


    [HttpPost("login")]
    public ActionResult<TokenDTO> Login(LoginDTO request)
    {
        try
        {
            var token = _accountService.LoginUser(request);
            return Ok(new TokenDTO { Token = token });
        }
        catch (ArgumentException e)
        {
            var error = new ErrorDTO { Message = e.Message };
            return BadRequest(error);
        }
    }
}