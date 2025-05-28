namespace OccasionMessageAdmin.Shared.Models;

public class Holiday
{
    public int HolidayID { get; set; }
    public string HolidayName { get; set; } = null!;
    public string? Description { get; set; }
    public short? YearOccurrence { get; set; }
    public byte CalendarType { get; set; } = 1;
    public int DayId { get; set; }
    public string? CountryCode { get; set; }
    public int? RegionCode { get; set; }
    public string? LanguageCode { get; set; }
    public bool IsGlobal { get; set; } = false;
    public short? HolidayType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
