using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Library.Services.HashProvider
{
    public class Hasher : IHasher
    {
        // salting: https://en.wikipedia.org/wiki/Salt_(cryptography)
        private const int SaltLength = 128 / 8;

        // subkey: the bits of the 'actual' password
        private const int SubkeyLength = 256 / 8;

        public string Hash(string password)
        {
            byte[] salt = new byte[SaltLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var subkeyBytes = HashAndSalt(password, salt);

            var result = new byte[SaltLength + SubkeyLength];

            // copy salt to final hash (bytes 0 - 15)
            Array.Copy(salt, 0, result, 0, SaltLength);

            // copy subkey to final hash (bytes 16 - 47)
            Array.Copy(subkeyBytes, 0, result, SaltLength, SubkeyLength);

            return Convert.ToBase64String(result);
        }

        public bool Verify(string password, string hash)
        {
            var decoded = Convert.FromBase64String(hash);

            // obtain salt from hash
            var salt = new byte[SaltLength];
            Array.Copy(decoded, 0, salt, 0, SaltLength);

            // obtain subkey from hash
            var subkey = new byte[SubkeyLength];
            Array.Copy(decoded, SaltLength, subkey, 0, SubkeyLength);

            return AreByteArraysEqual(subkey, HashAndSalt(password, salt));
        }

        private byte[] HashAndSalt(string password, byte[] salt)
        {
            // from https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-2.2

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: SubkeyLength);
        }

        private bool AreByteArraysEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }

            return true;
        }
    }
}
