using System.Security.Cryptography;
using System.Text;

namespace EventHubMaui.Services;

public static class PasswordHelper
{
    public static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
