namespace OccasionMessageAdmin.Shared.Models;

public class CalendarDay
{
    public int DayId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public byte CalendarType { get; set; } = 1;
    public int CalendarYear { get; set; }
    public int CalendarMonth { get; set; }
    public int CalendarDayValue { get; set; }
}