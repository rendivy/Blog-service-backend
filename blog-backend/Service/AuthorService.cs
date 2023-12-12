using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service;

public class AuthorService
{
    private readonly BlogDbContext _dbContext;

    public AuthorService(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AuthorDTO>> GetAuthorList()
    {
        var authors = await _dbContext.User.ToListAsync();

        var authorList = new List<AuthorDTO>();

        foreach (var author in authors)
        {
            var posts = await _dbContext.Posts
                .Where(p => p.AuthorId.ToString() == author.Id.ToString())
                .ToListAsync();

            if (posts.Count <= 0) continue;
            {
                var authorDto = new AuthorDTO
                {
                    FullName = author.FullName,
                    BirthDate = author.DateOfBirth,
                    Posts = posts.Count,
                    Created = author.CreateTime,
                    Gender = author.Gender,
                    Likes = posts.Sum(p => p.Likes)
                };

                authorList.Add(authorDto);
            }
        }

        return await Task.FromResult(authorList);
    }

}