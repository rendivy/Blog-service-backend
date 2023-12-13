namespace blog_backend.DAO.Model;

public class PostDTO
{
    public List<PostDetailsDTO> Posts { get; set; } = new();
    public PaginationDTO Pagination { get; set; } = new();
}