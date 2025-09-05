using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AESClientApp
{
    // Dịch vụ thực hiện các hoạt động mã hóa và giải mã AES.
    public class AesEncryptionService
    {
        private readonly byte[] _key; // Lưu trữ khóa mã hóa AES.
        private readonly byte[] _iv;  // Lưu trữ vector khởi tạo (IV) của AES.
        // Constructor để khởi tạo dịch vụ mã hóa với một khóa và IV.
        public AesEncryptionService(string base64Key, string base64IV)
        {
            // Xác thực rằng khóa (key) được cung cấp không được null hoặc rỗng.
            if (string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentException("Key không được là null hoặc rỗng.", nameof(base64Key));
            // Xác thực rằng IV được cung cấp không được null hoặc rỗng.
            if (string.IsNullOrWhiteSpace(base64IV))
                throw new ArgumentException("IV không được là null hoặc rỗng.", nameof(base64IV));
            // Chuyển đổi chuỗi khóa đã mã hóa Base64 thành một mảng byte cho AES.
            _key = Convert.FromBase64String(base64Key);
            // Chuyển đổi chuỗi IV đã mã hóa Base64 thành một mảng byte cho AES.
            _iv = Convert.FromBase64String(base64IV);
        }
        // Mã hóa một chuỗi văn bản thuần túy (plaintext) bằng mã hóa AES.
        public string EncryptString(string plainText)
        {
            // Xác thực rằng văn bản thuần túy không phải là null.
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));
            // Tạo một đối tượng của thuật toán AES.
            using (Aes aesAlg = Aes.Create())
            {
                // Gán khóa đã được khởi tạo trước đó cho đối tượng AES.
                aesAlg.Key = _key;
                // Gán IV đã được khởi tạo trước đó cho đối tượng AES.
                aesAlg.IV = _iv;
                // Tạo một đối tượng mã hóa (encryptor) để biến đổi văn bản thuần túy thành bản mã (ciphertext).
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Chuyển đổi chuỗi văn bản thuần túy thành một mảng byte sử dụng mã hóa UTF-8.
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                // Thực hiện hoạt động mã hóa trên các byte của văn bản thuần túy.
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                // Chuyển đổi mảng byte đã mã hóa thành một chuỗi được mã hóa Base64 và trả về.
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        // Giải mã một chuỗi bản mã (ciphertext) trở lại thành văn bản thuần túy (plaintext) bằng giải mã AES.
        public string DecryptString(string cipherText)
        {
            // Xác thực rằng bản mã không phải là null.
            if (cipherText == null)
                throw new ArgumentNullException(nameof(cipherText));
            // Tạo một đối tượng của thuật toán AES.
            using (Aes aesAlg = Aes.Create())
            {
                // Gán khóa đã được khởi tạo trước đó cho đối tượng AES.
                aesAlg.Key = _key;
                // Gán IV đã được khởi tạo trước đó cho đối tượng AES.
                aesAlg.IV = _iv;
                // Tạo một đối tượng giải mã (decryptor) để biến đổi bản mã trở lại thành văn bản thuần túy.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Chuyển đổi chuỗi bản mã đã mã hóa Base64 thành một mảng byte.
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                // Thực hiện hoạt động giải mã trên các byte của bản mã.
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                // Chuyển đổi mảng byte đã giải mã thành một chuỗi văn bản thuần túy sử dụng mã hóa UTF-8 và trả về.
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}