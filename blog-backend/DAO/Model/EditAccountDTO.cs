using System.ComponentModel.DataAnnotations;
using blog_backend.DAO.Utils;

namespace blog_backend.DAO.Model;

public class EditAccountDTO
{
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public GenderEnum Gender { get; set; } = GenderEnum.Male;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BirthDate { get; set; } = DateTime.Now;
    
}