using blog_backend.Common;
using blog_backend.DAO.Repository;
using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;


[ApiController]
[Route(EndpointRouteConstants.API)]
public class AuthController : Controller
{
    private readonly IAuthRepository _authRepository;
    private readonly GenerateJwt _tokenJwt;
    private readonly BlogDbContext _dbContext;

    public AuthController(IAuthRepository authRepository, BlogDbContext dbContext, GenerateJwt tokenJwt)
    {
        _authRepository = authRepository;
        _dbContext = dbContext;
        _tokenJwt = tokenJwt;
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
        var token = _tokenJwt.GenerateToken(user);
        return Ok(new { Token = token });
    }


    [HttpPost(EndpointRouteConstants.LOGIN)]
    [ProducesResponseType(typeof(TokenDTO), 200)]
    [ProducesResponseType( typeof(ErrorDTO), 400)]
    public ActionResult<TokenDTO> Login(LoginDTO request)
    {
       
        var user = _authRepository.GetUserByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        { 
            var error = new ErrorDTO { ErrorMessage = "Invalid email or password" };
            return BadRequest(error);
        }
        var token = _tokenJwt.GenerateToken(user);
        return Ok(new TokenDTO { Token = token });
    }
}