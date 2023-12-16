using blog_backend.DAO.Model;
using blog_backend.Entity.AccountEntities;
using blog_backend.Enums;

namespace blog_backend.Service.Mappers.AuthorMapper;

public static class AuthorMapper
{
    public static AuthorDTO Map(User author, int likes, int size)
    {
        return new AuthorDTO(fullName: author.FullName, birthDate: author.DateOfBirth, gender: author.Gender,
            posts: size, likes: likes, created: author.CreateTime);
    }
}