using blog_backend.DAO.Utils;

namespace blog_backend.DAO.Model;

public class UserAccountDto
{
    public UserAccountDto(Guid id, string fullName, GenderEnum gender, DateTime createTime, string email,
        string phoneNumber, DateTime dateOfBirth)
    {
        Id = id;
        FullName = fullName;
        Gender = gender;
        CreateTime = createTime;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
    }

    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    public string FullName { get; set; }
    public GenderEnum Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}