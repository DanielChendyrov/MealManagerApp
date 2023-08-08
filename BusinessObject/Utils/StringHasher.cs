using System.Security.Cryptography;
using System.Text;

namespace BusinessObject.Utils;

public class StringHasher
{
    public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
    {
        // If salt is not specified, generate it.
        if (saltBytes == null)
        {
            // Define min and max salt sizes.
            int minSaltSize = 4;
            int maxSaltSize = 8;

            // Generate a random number for the size of the salt.
            Random random = new();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            var rng = RandomNumberGenerator.Create();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
        }

        // Convert plain text into a byte array.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        // Allocate array, which will hold plain text and salt.
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

        // Copy plain text bytes into resulting array.
        for (int i = 0; i < plainTextBytes.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainTextBytes[i];
        }

        // Append salt bytes to the resulting array.
        for (int i = 0; i < saltBytes.Length; i++)
        {
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
        }

        // Make sure hashing algorithm name is specified.
        hashAlgorithm ??= "";
        HashAlgorithm hash = hashAlgorithm.ToUpper() switch
        {
            "SHA384" => SHA384.Create(),
            "SHA512" => SHA512.Create(),
            _ => MD5.Create(),
        };

        // Compute hash value of our plain text with appended salt.
        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

        // Create array which will hold hash and original salt bytes.
        byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

        // Copy hash bytes into resulting array.
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashWithSaltBytes[i] = hashBytes[i];
        }

        // Append salt bytes to the result.
        for (int i = 0; i < saltBytes.Length; i++)
        {
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
        }

        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        // Return the result.
        return hashValue;
    }

    public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
    {
        // Convert base64-encoded hash value into a byte array.
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        // We must know size of hash (without salt).
        int hashSizeInBytes;

        // Make sure that hashing algorithm name is specified.
        hashAlgorithm ??= "";
        var hashSizeInBits = hashAlgorithm.ToUpper() switch
        {
            "SHA384" => 384,
            "SHA512" => 512,
            // Must be MD5
            _ => 128,
        };

        // Convert size of hash from bits to bytes.
        hashSizeInBytes = hashSizeInBits / 8;

        // Make sure that the specified hash value is long enough.
        if (hashWithSaltBytes.Length < hashSizeInBytes)
            return false;

        // Allocate array to hold original salt bytes retrieved from hash.
        byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

        // Copy salt from the end of the hash to the new array.
        for (int i = 0; i < saltBytes.Length; i++)
        {
            saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];
        }

        // Compute a new hash string.
        string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

        // If the computed hash matches the specified hash,
        // the plain text value must be correct.
        return hashValue == expectedHashString;
    }
}
