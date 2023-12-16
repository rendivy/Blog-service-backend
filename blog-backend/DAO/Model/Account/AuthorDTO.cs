using blog_backend.Enums;

namespace blog_backend.DAO.Model;

public class AuthorDTO
{
    public AuthorDTO()
    {
    }

    public AuthorDTO(string fullName, DateTime birthDate, GenderEnum gender, int posts, int likes, DateTime created)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Posts = posts;
        Likes = likes;
        Created = created;
    }

    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; }
}