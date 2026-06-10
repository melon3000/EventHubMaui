using EventHubMaui.Models;
using SQLite;

namespace EventHubMaui.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _db;

    public SQLiteAsyncConnection Db => _db ?? throw new InvalidOperationException("Database not initialized.");

    public async Task InitAsync()
    {
        if (_db is not null)
            return;

        var path = Path.Combine(FileSystem.AppDataDirectory, "eventhub_working.db3");
        _db = new SQLiteAsyncConnection(path);

        await Db.CreateTableAsync<AppUser>();
        await Db.CreateTableAsync<EventCategory>();
        await Db.CreateTableAsync<EventItem>();
        await Db.CreateTableAsync<EventRegistration>();

        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        var users = await Db.Table<AppUser>().ToListAsync();
        if (users.Count > 0)
            return;

        await Db.InsertAsync(new AppUser
        {
            FullName = "Administraator",
            Email = "admin@eventhub.ee",
            PasswordHash = PasswordHelper.Hash("admin123"),
            Role = UserRole.Admin
        });

        await Db.InsertAsync(new AppUser
        {
            FullName = "Milan Petrovski",
            Email = "user@eventhub.ee",
            PasswordHash = PasswordHelper.Hash("user123"),
            Role = UserRole.User
        });

        var categories = new List<EventCategory>
        {
            new() { Name = "Kontsert", Description = "Muusika ja live show" },
            new() { Name = "Teater", Description = "Etendused ja lavalised üritused" },
            new() { Name = "Sport", Description = "Aktiivsed ja spordiüritused" },
            new() { Name = "Haridus", Description = "Töötoad, seminarid ja koolitused" }
        };

        foreach (var c in categories)
            await Db.InsertAsync(c);

        var all = await Db.Table<EventCategory>().ToListAsync();
        int concert = all.First(x => x.Name == "Kontsert").CategoryId;
        int theatre = all.First(x => x.Name == "Teater").CategoryId;
        int sport = all.First(x => x.Name == "Sport").CategoryId;
        int edu = all.First(x => x.Name == "Haridus").CategoryId;

        var events = new List<EventItem>
        {
            new()
            {
                CategoryId = concert,
                Title = "Suveõhtu kontsert",
                Description = "Kaasaegne muusikaõhtu välilaval. Üritusel esinevad kohalikud artistid ja DJ-d.",
                EventDate = DateTime.Now.AddDays(5).Date.AddHours(19),
                Location = "Tallinn, Vabaduse väljak",
                ImageUrl = "https://images.unsplash.com/photo-1501281668745-f7f57925c3b4?w=1200",
                MaxParticipants = 120
            },
            new()
            {
                CategoryId = theatre,
                Title = "Improteatri õhtu",
                Description = "Meeleolukas improetendus, kus publik saab mõjutada sündmuste kulgu.",
                EventDate = DateTime.Now.AddDays(8).Date.AddHours(18),
                Location = "Tallinn, Kultuurikatel",
                ImageUrl = "https://images.unsplash.com/photo-1503095396549-807759245b35?w=1200",
                MaxParticipants = 80
            },
            new()
            {
                CategoryId = sport,
                Title = "Linna jooksupäev",
                Description = "Tervisespordi päev algajatele ja edasijõudnutele. Valikus 3 km ja 10 km distants.",
                EventDate = DateTime.Now.AddDays(12).Date.AddHours(10),
                Location = "Tallinn, Kadrioru park",
                ImageUrl = "https://images.unsplash.com/photo-1476480862126-209bfaa8edc8?w=1200",
                MaxParticipants = 250
            },
            new()
            {
                CategoryId = edu,
                Title = ".NET MAUI töötuba",
                Description = "Praktiline töötuba, kus õpitakse MAUI rakenduse lehti, SQLite andmebaasi ja C# loogikat.",
                EventDate = DateTime.Now.AddDays(16).Date.AddHours(14),
                Location = "Tallinna Tööstushariduskeskus",
                ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?w=1200",
                MaxParticipants = 30
            }
        };

        foreach (var e in events)
            await Db.InsertAsync(e);
    }
}
