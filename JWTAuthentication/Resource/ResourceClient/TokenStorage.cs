using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceClient
{
    public class TokenStorage
    {
        // Lưu trữ access token hiện tại được sử dụng cho các request API đã xác thực.
        public string AccessToken { get; set; } = string.Empty;

        // Lưu trữ refresh token hiện tại được sử dụng để lấy các access token mới.
        public string RefreshToken { get; set; } = string.Empty;

        // Lưu trữ client identifier liên quan đến các token.
        public string ClientId { get; set; } = string.Empty;
    }
}