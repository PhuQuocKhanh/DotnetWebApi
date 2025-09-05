using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AESServerAPP.Models
{
    public class AesEncryptionService
    {
        private readonly KeyManagementService _keyManagementService;
        public AesEncryptionService(KeyManagementService keyManagementService)
        {
            _keyManagementService = keyManagementService;
        }

        // Phương thức chuyển đổi văn bản gốc thành văn bản đã mã hóa.
        public async Task<string> EncryptStringAsync(string clientId, string plainText)
        {
            var client = await _keyManagementService.GetKeyAndIVAsync(clientId);
            if (client == null)
                throw new ArgumentException("Client Id không hợp lệ");

            byte[] key = Convert.FromBase64String(client.Key);
            byte[] iv = Convert.FromBase64String(client.IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        // Phương thức chuyển đổi văn bản đã mã hóa trở lại văn bản gốc.
        public async Task<string> DecryptStringAsync(string clientId, string cipherText)
        {
            var client = await _keyManagementService.GetKeyAndIVAsync(clientId);
            if (client == null)
                throw new ArgumentException("Client Id không hợp lệ");
            
            byte[] key = Convert.FromBase64String(client.Key);
            byte[] iv = Convert.FromBase64String(client.IV);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}