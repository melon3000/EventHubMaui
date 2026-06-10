using SQLite;

namespace EventHubMaui.Models;

public class EventCategory
{
    [PrimaryKey, AutoIncrement]
    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
