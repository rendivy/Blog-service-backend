using blog_backend.DAO.Repository;
using blog_backend.Dao.Repository.Model;
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
    
    [HttpPost(Name = "inputUser")]
    public void InputUser(UserAuthorizationDTO user)
    {
        _userRepository.AddUser(user);
    }
}