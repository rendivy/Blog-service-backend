using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;

namespace blog_backend.DAO.Repository;

public interface IUserRepository
{
     List<User> GetAllUsers();
     
     void AddUser(UserAuthorizationDTO user);
}