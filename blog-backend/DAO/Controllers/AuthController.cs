using blog_backend.Common;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;


[ApiController]
[Route(EndpointRouteConstants.API)]
public class AuthController : Controller
{
    private readonly IAuthRepository _authRepository;
    private readonly GenerateTokenService _tokenService;
    private readonly BlogDbContext _dbContext;

    public AuthController(IAuthRepository authRepository, BlogDbContext dbContext, GenerateTokenService tokenService)
    {
        _authRepository = authRepository;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }
    
    
    [HttpPost(EndpointRouteConstants.REGISTER)]
    public ActionResult<User> Register(AuthorizationDTO request)
    {
        if (_authRepository.GetUserByEmail(request.Email) != null)
        {
            return BadRequest("User already exists");
        }
        var user = _authRepository.Register(request);
        _dbContext.User.Add(user);
        _dbContext.SaveChanges();
        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }


    [HttpPost(EndpointRouteConstants.LOGIN)]
    public ActionResult<TokenDTO> Login(LoginDTO request)
    {
       
        var user = _authRepository.GetUserByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        { 
            var error = new ErrorDTO { ErrorMessage = "Invalid email or password" };
            return BadRequest(error);
        }
        var token = _tokenService.GenerateToken(user);
        return Ok(new TokenDTO { Token = token });
    }
}