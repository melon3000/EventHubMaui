using SQLite;

namespace EventHubMaui.Models;

public class EventRegistration
{
    [PrimaryKey, AutoIncrement]
    public int RegistrationId { get; set; }

    [Indexed]
    public int UserId { get; set; }

    [Indexed]
    public int EventId { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.Now;
}
