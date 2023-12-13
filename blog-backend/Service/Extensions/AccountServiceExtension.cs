using blog_backend.DAO.Database;
using blog_backend.Entity.AccountEntities;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service.Extensions;

public static class AccountServiceExtension
{
    public static async Task<User?> GetUserByEmail(this string userEmail, BlogDbContext dbContext)
    {
        return await dbContext.User.FirstOrDefaultAsync(u => u.Email == userEmail);
    }
    
    
    public static async Task<User?> GetUserById(this string id, BlogDbContext dbContext)
    {
        return await dbContext.User.FirstOrDefaultAsync(u => u.Id.ToString() == id);
    }
}