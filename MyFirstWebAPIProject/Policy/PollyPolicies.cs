using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;

namespace MyFirstWebAPIProject.Policy
{
   // Class static dùng để chứa các chính sách của Polly.
    public static class PollyPolicies
    {
        // Phương thức trả về một chính sách thử lại bất đồng bộ cho các phản hồi HTTP.
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                // Xử lý các lỗi HTTP tạm thời như timeout, lỗi mạng, các mã lỗi 5xx.
                .HandleTransientHttpError()
                
                // Tùy chọn: Có thể mở comment dưới đây để chỉ định các mã trạng thái cụ thể cần retry
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)

                // Cấu hình retry tối đa 3 lần với thời gian chờ tăng dần (Exponential Backoff):
                // Lần 1: 2^1 = 2 giây
                // Lần 2: 2^2 = 4 giây
                // Lần 3: 2^3 = 8 giây
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
        }
    }
}