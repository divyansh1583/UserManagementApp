using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.Interfaces.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementAPI.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService(IConfiguration configuration)
        {
            string keyString = configuration["Encryption:Key"];
            string ivString = configuration["Encryption:IV"];

            // Ensure the key is exactly 32 bytes (256 bits)
            _key = new byte[32];
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
            Array.Copy(keyBytes, _key, Math.Min(keyBytes.Length, 32));

            // Ensure the IV is exactly 16 bytes (128 bits)
            _iv = new byte[16];
            byte[] ivBytes = Encoding.UTF8.GetBytes(ivString);
            Array.Copy(ivBytes, _iv, Math.Min(ivBytes.Length, 16));
        }

        public byte[] Encrypt(string data)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                    return ms.ToArray();
                }
            }
        }

        public string Decrypt(byte[] encryptedData)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedData))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
