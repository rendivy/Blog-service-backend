using blog_backend.DAO.Repository;
using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;


[ApiController]
[Route("putUser")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    [HttpGet(Name="getAllUsers")]
    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }
    
    [HttpPost(Name = "inputUser")]
    public void InputUser(UserAuthorizationDto user)
    {
        _userRepository.AddUser(user);
    }
}