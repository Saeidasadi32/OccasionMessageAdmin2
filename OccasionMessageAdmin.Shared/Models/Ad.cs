namespace OccasionMessageAdmin.Shared.Models;

public class Ad
{
    public int AdID { get; set; }
    public string AdTitle { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? CountryCode { get; set; }
    public int? RegionCode { get; set; }
    public string? LanguageCode { get; set; }
    public byte? Gender { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
