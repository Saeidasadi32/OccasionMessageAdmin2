namespace OccasionMessageAdmin.Shared.Models;

public class OccasionMessageChannel
{
    public int ChannelMessageID { get; set; }
    public int MessageID { get; set; }
    public byte Channel { get; set; }
    public byte MessageType { get; set; }
    public string MessageText { get; set; } = null!;
    public string? MediaURL { get; set; }
    public int? AdChannelID { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public OccasionMessage? OccasionMessage { get; set; }
}