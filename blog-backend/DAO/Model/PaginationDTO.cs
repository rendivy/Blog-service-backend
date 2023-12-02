namespace blog_backend.DAO.Model;

public class PaginationDTO
{
    public int Size { get; set; }
    public int Page { get; set; }
    public int Current { get; set; }
}