namespace OccasionMessageAdmin.Shared.Models;

public class AdChannel
{
    public int AdChannelID { get; set; }
    public int AdID { get; set; }
    public byte Channel { get; set; }
    public byte MessageType { get; set; }
    public string MessageText { get; set; } = null!;
    public string? MediaURL { get; set; }
    public string? TargetURL { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Ad? Ad { get; set; }
}
