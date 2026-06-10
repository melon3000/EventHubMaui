using SQLite;

namespace EventHubMaui.Models;

public class AppUser
{
    [PrimaryKey, AutoIncrement]
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    [Indexed]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
