using blog_backend.Common;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

[ApiController]
[Route(EndpointRouteConstants.API)]
public class AccountController : Controller
{
    private readonly AccountService _accountService;

    public AccountController(IAccountRepository accountRepository, GenerateTokenService tokenService)
    {
        _accountService = new AccountService(accountRepository, tokenService);
    }

    [HttpPost(EndpointRouteConstants.LOGOUT)]
    [Authorize]
    public async Task LogoutUser()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        if (token != null)
        {
            await _accountService.LogoutUser(token);
        }
    }


    [HttpPost(EndpointRouteConstants.REGISTER)]
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


    [HttpPut(EndpointRouteConstants.PROFILE)]
    [Authorize]
    public async Task EditUser(EditAccountDTO user)
    {
        await _accountService.EditUser(user);
    }


    [HttpPost(EndpointRouteConstants.LOGIN)]
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