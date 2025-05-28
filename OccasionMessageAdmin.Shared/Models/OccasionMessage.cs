namespace OccasionMessageAdmin.Shared.Models;

public class OccasionMessage
{
    public int MessageID { get; set; }
    public int HolidayID { get; set; }
    public string LanguageCode { get; set; } = null!;
    public byte? Gender { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public string? CountryCode { get; set; }
    public int? RegionCode { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Holiday? Holiday { get; set; }
}