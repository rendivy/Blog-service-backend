using blog_backend.Enums;

namespace blog_backend.DAO.Model;

public class AuthorDTO
{
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; }
}