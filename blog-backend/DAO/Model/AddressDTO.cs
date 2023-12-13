namespace blog_backend.DAO.Model;

public class AddressDTO
{
    public long ObjectId { get; set; }
    public Guid ObjectGuid { get; set; }
    public string Text { get; set; } = null!;
    public string Level { get; set; }
    public string? ObjectLevelText { get; set; } = null!;
}