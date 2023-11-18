using blog_backend.DAO.Repository;
using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly GenerateJwt _tokenJwt;
    private readonly BlogDbContext _dbContext;

    public AuthController(IUserRepository userRepository, BlogDbContext dbContext, GenerateJwt tokenJwt)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
        _tokenJwt = tokenJwt;
    }


    [HttpPost("login")]
    public ActionResult<User> Login(UserAuthorizationDto request)
    {
       
        var user = _userRepository.GetUserByEmail(request.Email);
        if (user == null)
        {
            return BadRequest("User not found");
        }
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Invalid password");
        }
        var token = _tokenJwt.GenerateToken(user);
        return Ok(new { Token = token });
    }
}