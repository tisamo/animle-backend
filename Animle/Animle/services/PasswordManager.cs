using System.Security.Cryptography;

public class PasswordManager
{
    // Method to hash and salt a password
    public string HashPassword(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]); // Generate salt

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); // PBKDF2 hashing
        byte[] hash = pbkdf2.GetBytes(20); // 20 bytes hash size

        // Combine the salt and hash
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Convert to base64 string for storage
        string hashedPassword = Convert.ToBase64String(hashBytes);
        return hashedPassword;
    }

    // Method to verify password against hashed and salted version
    public bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        // Convert stored hashed password back to byte array
        byte[] hashBytes = Convert.FromBase64String(storedPassword);

        // Extract salt from stored hash
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Compute hash using entered password and stored salt
        var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Compare computed hash with stored hash
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
                return false;
        }
        return true;
    }
}
