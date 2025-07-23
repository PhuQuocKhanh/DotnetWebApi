using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Services
{
    public interface IRetryPolly
    { 
        Task<string> GetExternalDataAsync();
    }
    public class RetryPolly : IRetryPolly
    {
        private readonly HttpClient _httpClient;

        public RetryPolly(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExternalDataAsync()
        {
            // Gửi HTTP GET đến URL giả lập lỗi 503
            var response = await _httpClient.GetAsync("https://httpstat.us/503");

            // Nếu thành công (status 200)
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                // Ném lỗi 503 nếu gặp Service Unavailable
                throw new HttpRequestException("Service is temporarily unavailable.", null, System.Net.HttpStatusCode.ServiceUnavailable);
            }
            else
            {
                // Ném lỗi cho các mã khác
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}