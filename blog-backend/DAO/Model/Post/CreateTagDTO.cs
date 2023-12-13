using System.ComponentModel.DataAnnotations;

namespace blog_backend.DAO.Model;

public class CreateTagDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
}