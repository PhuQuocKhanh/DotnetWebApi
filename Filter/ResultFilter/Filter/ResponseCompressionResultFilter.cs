using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResultFilter.Filter
{
   // Result Filter bất đồng bộ tùy chỉnh để nén các response JSON lớn hơn 100 KB
    public class ResponseCompressionResultFilter : IAsyncResultFilter
    {
        // Phương thức này chạy xung quanh việc thực thi kết quả một cách bất đồng bộ
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Lấy HTTP response hiện tại từ context
            var response = context.HttpContext.Response;
            // Sao lưu luồng body gốc của response (luồng mạng)
            var originalBodyStream = response.Body;

            // Tạo một MemoryStream để tạm thời giữ nội dung response (đệm nó)
            using var bufferStream = new MemoryStream();
            // Thay thế luồng body của response bằng luồng đệm để
            // action result ghi vào bộ đệm này thay vì trực tiếp đến client
            response.Body = bufferStream;

            // Tiếp tục với filter tiếp theo hoặc thực thi action; điều này sẽ ghi vào bufferStream
            var executedContext = await next();

            // Đặt lại vị trí của buffer về đầu để có thể đọc nội dung response đã được đệm
            bufferStream.Seek(0, SeekOrigin.Begin);

            // Kiểm tra xem response có nên được nén không:
            // - Header Content-Type tồn tại và chứa "application/json" (không phân biệt chữ hoa/thường)
            // - Kích thước response đã đệm lớn hơn 100 KB (100 * 1024 byte)
            if (response.ContentType != null &&
                response.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) &&
                bufferStream.Length > 100 * 1024)
            {
                // Thêm header Content-Encoding để báo cho client biết response đã được nén bằng gzip
                response.Headers["Content-Encoding"] = "gzip";
                
                // Tạo một MemoryStream mới để chứa dữ liệu đã nén
                using var compressedStream = new MemoryStream();
                // Sử dụng GZipStream để nén nội dung response đã đệm
                // Tham số thứ ba "true" giữ cho compressedStream mở sau khi gzipStream được giải phóng
                using (var gzipStream = new GZipStream(compressedStream, CompressionLevel.Fastest, true))
                {
                    // Sao chép tất cả dữ liệu từ bufferStream vào gzipStream để nén nó
                    await bufferStream.CopyToAsync(gzipStream);
                }

                // Đặt lại vị trí của compressedStream về đầu để đọc
                compressedStream.Seek(0, SeekOrigin.Begin);
                
                // Khôi phục luồng body gốc của response để có thể ghi đến client
                response.Body = originalBodyStream;
                // Thiết lập header Content-Length bằng kích thước của dữ liệu đã nén (tùy chọn nhưng được khuyến nghị)
                response.ContentLength = compressedStream.Length;
                // Ghi dữ liệu đã nén vào luồng body gốc của response (luồng mạng)
                await compressedStream.CopyToAsync(response.Body);
            }
            else
            {
                // Nếu không cần nén:
                // Khôi phục luồng body gốc
                response.Body = originalBodyStream;
                // Sao chép nội dung gốc đã đệm vào luồng gốc (không nén)
                await bufferStream.CopyToAsync(response.Body);
            }
        }
    }
}