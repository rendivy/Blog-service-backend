namespace blog_backend.DAO.Model;

public class CreatePostDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReadingTime { get; set; }
    public string Image { get; set; } = string.Empty;
    public string? AddressId { get; set; } = null;
    public List<Guid> Tags { get; set; } = new();
}

