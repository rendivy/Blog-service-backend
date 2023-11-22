using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface IAccountRepository
{
     public User Register(AuthorizationDTO request);
     
     public void EditUser(EditAccountDTO user);

     public User? GetUserById(Guid id);
     
     public User? GetUserByEmail(string email);
     
}