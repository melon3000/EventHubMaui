using SQLite;

namespace EventHubMaui.Models;

public class EventItem
{
    [PrimaryKey, AutoIncrement]
    public int EventId { get; set; }

    [Indexed]
    public int CategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public string Location { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int MaxParticipants { get; set; }

    public bool IsPublished { get; set; } = true;
}
