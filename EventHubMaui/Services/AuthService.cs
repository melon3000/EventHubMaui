using EventHubMaui.Models;

namespace EventHubMaui.Services;

public class AuthService
{
    private readonly DatabaseService _database;

    public AppUser? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser is not null;
    public bool IsAdmin => CurrentUser?.Role == UserRole.Admin;

    public AuthService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<AppUser?> LoginAsync(string email, string password)
    {
        await _database.InitAsync();

        string normalizedEmail = (email ?? string.Empty).Trim().ToLowerInvariant();
        string hash = PasswordHelper.Hash(password ?? string.Empty);

        // ВАЖНО: фильтруем в памяти, чтобы SQLite не генерировал trim() и не падал.
        var users = await _database.Db.Table<AppUser>().ToListAsync();
        var user = users.FirstOrDefault(u =>
            u.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase)
            && u.PasswordHash == hash);

        CurrentUser = user;
        return user;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password)
    {
        await _database.InitAsync();

        string normalizedName = (fullName ?? string.Empty).Trim();
        string normalizedEmail = (email ?? string.Empty).Trim().ToLowerInvariant();
        string normalizedPassword = password ?? string.Empty;

        if (normalizedName.Length < 2)
            return (false, "Sisesta korrektne nimi.");

        if (!normalizedEmail.Contains("@"))
            return (false, "Sisesta korrektne e-post.");

        if (normalizedPassword.Length < 5)
            return (false, "Parool peab olema vähemalt 5 märki.");

        var users = await _database.Db.Table<AppUser>().ToListAsync();
        if (users.Any(u => u.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase)))
            return (false, "Sellise e-postiga konto on juba olemas.");

        var user = new AppUser
        {
            FullName = normalizedName,
            Email = normalizedEmail,
            PasswordHash = PasswordHelper.Hash(normalizedPassword),
            Role = UserRole.User
        };

        await _database.Db.InsertAsync(user);
        CurrentUser = user;
        return (true, "Konto on loodud.");
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
