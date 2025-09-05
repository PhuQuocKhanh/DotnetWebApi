using System.Security.Cryptography;
namespace KeyGeneratorService
{
    // Lớp chính của chương trình KeyGeneratorService
    public class Program
    {
        // Điểm khởi đầu của ứng dụng console
        static void Main(string[] args)
        {
            // Gọi phương thức để tạo khóa và IV cho AES
            GenerateAesKeyAndIV();
            // Đợi người dùng nhấn một phím để ngăn console đóng lại ngay lập tức
            Console.ReadKey();
        }
        // Phương thức để tạo một khóa AES và Vector khởi tạo (IV)
        private static void GenerateAesKeyAndIV()
        {
            // Tạo một đối tượng AES mới để tạo khóa.
            using (Aes aesAlg = Aes.Create())
            {
                // Đặt kích thước của khóa mã hóa là 256 bit, cung cấp khả năng bảo mật mạnh
                aesAlg.KeySize = 256;
                // Tạo một khóa ngẫu nhiên dựa trên kích thước khóa đã đặt ở trên
                aesAlg.GenerateKey();
                // Tạo một vector khởi tạo (IV) ngẫu nhiên
                aesAlg.GenerateIV();
                // Chuyển đổi khóa đã tạo thành chuỗi base64 để dễ đọc và lưu trữ hơn
                string key = Convert.ToBase64String(aesAlg.Key);
                // Chuyển đổi IV đã tạo thành chuỗi base64 để dễ đọc và lưu trữ hơn
                string iv = Convert.ToBase64String(aesAlg.IV);
                // Xuất khóa AES đã được mã hóa base64 ra console
                Console.WriteLine("AES Key (Base64): " + key);
                // Xuất IV của AES đã được mã hóa base64 ra console
                Console.WriteLine("AES IV (Base64): " + iv);
            }
        }
    }
}