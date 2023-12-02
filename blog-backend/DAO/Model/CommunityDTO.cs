namespace blog_backend.DAO.Model;

public class CommunityDTO
{
    public List<PostDTO> Posts { get; set; } = new();
    public PaginationDTO Pagination { get; set; } = new();
}