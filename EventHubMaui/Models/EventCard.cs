namespace EventHubMaui.Models;

public class EventCard
{
    public int EventId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int RegisteredCount { get; set; }
    public bool IsPublished { get; set; }

    public string DateText => EventDate.ToString("dd.MM.yyyy HH:mm");
    public string ParticipantsText => $"{RegisteredCount}/{MaxParticipants}";
}
