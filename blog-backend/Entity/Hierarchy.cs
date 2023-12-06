using System.ComponentModel.DataAnnotations;

namespace blog_backend.Entity;

public class Hierarchy
{
    [Key]
    public int Id { get; set; }
    public int ObjectId { get; set; }
    public int ParentId { get; set; } = 0; 
    public int ChangeId { get; set; }
    public int RegionCode { get; set; }
    public int? AreaCode { get; set; }
    public int? CityCode { get; set; }
    public int? PlaceCode { get; set; }
    public int? PlanCode { get; set; }
    public int? StreetCode { get; set; }
    public int? PrevId { get; set; }
    public int? NextId { get; set; }
    public string UpdatedDate { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
}