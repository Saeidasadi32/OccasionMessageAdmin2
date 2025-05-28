using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OccasionMessageAdmin.Shared.Models;

public class MessageLog
{
    public long LogID { get; set; }
    public Guid SenderID { get; set; }
    public Guid ReciverID { get; set; }
    public int ChannelMessageID { get; set; }
    public DateTime SentAt { get; set; }
    public byte Channel { get; set; }
    public byte Status { get; set; } = 1;
}