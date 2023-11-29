using System.ComponentModel.DataAnnotations;

namespace blog_backend.DAO.Model;

public class CreateCommunityDTO
{
    [Required] 
    public string Name { get; set; }
    public string Description { get; set; }
    [Required] 
    public bool IsClosed { get; set; } = false;
}