using System.Security.Cryptography;
using System.Text;

namespace RandomDrugScreenUI.Utilities;

/// <summary>
/// Utility for generating SHA256 password hashes compatible with the authentication system.
/// Use this for testing or creating new user passwords.
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Generates a SHA256 hash for a password.
    /// This matches the hashing used by AuthenticationService.
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Base64-encoded SHA256 hash</returns>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies if a password matches a hash.
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="hash">Base64-encoded SHA256 hash</param>
    /// <returns>True if password matches hash</returns>
    public static bool VerifyPassword(string password, string hash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == hash;
    }
}

// Example usage (for testing/development):
// var hash = PasswordHasher.HashPassword("mypassword123");
// Console.WriteLine($"Hash: {hash}");
// 
// Then update the emp table:
// UPDATE emp SET password = '{hash}' WHERE name = 'username'
