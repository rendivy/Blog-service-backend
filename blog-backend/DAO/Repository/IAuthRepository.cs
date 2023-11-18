using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Repository;

public interface IAuthRepository
{
     public User Register(AuthorizationDTO request);
     
     public User? GetUserByEmail(string email);
     
}