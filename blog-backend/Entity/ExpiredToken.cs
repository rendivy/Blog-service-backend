namespace blog_backend.Entity;

public class ExpiredToken
{
    public Guid Id { get; set; }
    public DateTime ExpiryDate { get; set; }
}