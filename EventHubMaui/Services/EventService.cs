using EventHubMaui.Models;

namespace EventHubMaui.Services;

public class EventService
{
    private readonly DatabaseService _database;

    public EventService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<List<EventCategory>> GetCategoriesAsync()
    {
        await _database.InitAsync();
        return (await _database.Db.Table<EventCategory>().ToListAsync()).OrderBy(c => c.Name).ToList();
    }

    public async Task<List<EventCard>> GetEventsAsync(int? categoryId = null, bool includeHidden = false)
    {
        await _database.InitAsync();

        var events = await _database.Db.Table<EventItem>().ToListAsync();
        var categories = await _database.Db.Table<EventCategory>().ToListAsync();
        var registrations = await _database.Db.Table<EventRegistration>().ToListAsync();

        if (!includeHidden)
            events = events.Where(e => e.IsPublished).ToList();

        if (categoryId is not null && categoryId > 0)
            events = events.Where(e => e.CategoryId == categoryId.Value).ToList();

        return events
            .OrderBy(e => e.EventDate)
            .Select(e => new EventCard
            {
                EventId = e.EventId,
                CategoryId = e.CategoryId,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == e.CategoryId)?.Name ?? "Muu",
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                Location = e.Location,
                ImageUrl = e.ImageUrl,
                MaxParticipants = e.MaxParticipants,
                RegisteredCount = registrations.Count(r => r.EventId == e.EventId),
                IsPublished = e.IsPublished
            })
            .ToList();
    }

    public async Task<EventItem?> GetEventAsync(int id)
    {
        await _database.InitAsync();
        var events = await _database.Db.Table<EventItem>().ToListAsync();
        return events.FirstOrDefault(e => e.EventId == id);
    }

    public async Task<(bool Success, string Message)> RegisterToEventAsync(int userId, int eventId)
    {
        await _database.InitAsync();

        var eventItem = await GetEventAsync(eventId);
        if (eventItem is null)
            return (false, "Üritust ei leitud.");

        var registrations = await _database.Db.Table<EventRegistration>().ToListAsync();

        if (registrations.Any(r => r.UserId == userId && r.EventId == eventId))
            return (false, "Oled juba sellele üritusele registreeritud.");

        if (registrations.Count(r => r.EventId == eventId) >= eventItem.MaxParticipants)
            return (false, "Üritusel ei ole enam vabu kohti.");

        await _database.Db.InsertAsync(new EventRegistration
        {
            UserId = userId,
            EventId = eventId,
            RegisteredAt = DateTime.Now
        });

        return (true, "Registreerimine õnnestus.");
    }

    public async Task<List<EventCard>> GetMyRegistrationsAsync(int userId)
    {
        await _database.InitAsync();

        var registrations = await _database.Db.Table<EventRegistration>().ToListAsync();
        var myEventIds = registrations.Where(r => r.UserId == userId).Select(r => r.EventId).ToHashSet();
        var events = await GetEventsAsync(includeHidden: true);

        return events.Where(e => myEventIds.Contains(e.EventId)).ToList();
    }

    public async Task CancelRegistrationAsync(int userId, int eventId)
    {
        await _database.InitAsync();

        var registrations = await _database.Db.Table<EventRegistration>().ToListAsync();
        var registration = registrations.FirstOrDefault(r => r.UserId == userId && r.EventId == eventId);

        if (registration is not null)
            await _database.Db.DeleteAsync(registration);
    }

    public async Task SaveEventAsync(EventItem item)
    {
        await _database.InitAsync();

        if (item.EventId == 0)
            await _database.Db.InsertAsync(item);
        else
            await _database.Db.UpdateAsync(item);
    }

    public async Task DeleteEventAsync(int eventId)
    {
        await _database.InitAsync();

        var events = await _database.Db.Table<EventItem>().ToListAsync();
        var item = events.FirstOrDefault(e => e.EventId == eventId);
        if (item is null)
            return;

        var registrations = await _database.Db.Table<EventRegistration>().ToListAsync();
        foreach (var r in registrations.Where(r => r.EventId == eventId))
            await _database.Db.DeleteAsync(r);

        await _database.Db.DeleteAsync(item);
    }

    public async Task SaveCategoryAsync(EventCategory category)
    {
        await _database.InitAsync();

        if (category.CategoryId == 0)
            await _database.Db.InsertAsync(category);
        else
            await _database.Db.UpdateAsync(category);
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        await _database.InitAsync();

        var categories = await _database.Db.Table<EventCategory>().ToListAsync();
        var category = categories.FirstOrDefault(c => c.CategoryId == categoryId);

        if (category is not null)
            await _database.Db.DeleteAsync(category);
    }
}
