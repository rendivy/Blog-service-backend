using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface IAuthRepository
{
     public User Register(AuthorizationDTO request);
     
     public User? GetUserByEmail(string email);
     
}