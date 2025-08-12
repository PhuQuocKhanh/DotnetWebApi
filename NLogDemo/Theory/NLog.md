-- What is NLog? --
- NLog là một thư viện logging miễn phí, mã nguồn mở, hiệu năng cao, được thiết kế cho nhiều nền tảng .NET như .NET Core, .NET Framework, Mono và Xamarin.
- NLog hỗ trợ logging có cấu trúc, logging bất đồng bộ và lọc log, giúp tối ưu cho việc debug, theo dõi hiệu năng và giám sát lỗi trong các ứng dụng ASP.NET Core Web API.
-- Các tính năng chính của NLog: --
- Multiple Targets (Đa điểm đích): Có thể ghi log ra nhiều nơi như file, cơ sở dữ liệu, console, v.v.
- Flexible Log Levels (Mức log linh hoạt): Hỗ trợ các mức log như Trace, Debug, Information, Warning, Error và Critical.
- Structured Logging (Logging có cấu trúc): Cho phép ghi dữ liệu log dạng giàu thông tin và có cấu trúc (ví dụ: JSON).
- Asynchronous Logging (Logging bất đồng bộ): Tách việc ghi log sang luồng nền để cải thiện hiệu năng ứng dụng.

- ClearProviders(): Xóa toàn bộ các logging provider mặc định (console, debug, v.v.) để chỉ dùng NLog.
- UseNLog(): Tích hợp NLog làm logging provider chính. Mọi lệnh ghi log (ILogger) trong ứng dụng sẽ chạy qua NLog.

-- Structured Logging là gì? -- 
- Structured Logging (ghi log có cấu trúc) là phương pháp ghi log dưới dạng dữ liệu có cấu trúc 
- (ví dụ: đối tượng JSON) thay vì chỉ ghi plain text (chuỗi thuần).
- Mỗi log entry sẽ được lưu dưới dạng cặp key–value, giúp việc lọc, tìm kiếm và phân tích log dễ dàng hơn.
- Structured logging đặc biệt hữu ích khi:
  - Log cần được xử lý bởi các hệ thống quản lý log (log management system)
  - Hoặc cần tìm kiếm log dựa trên một dữ liệu cụ thể.

-- NLOG với Asynchronous Logging trong ASP.NET Core Web API -- 
- Asynchronous logging giúp cải thiện hiệu năng ứng dụng bằng cách chuyển tác vụ ghi log sang một luồng nền (background thread) riêng biệt. 
- Điều này đặc biệt hữu ích trong các tình huống log với tần suất cao, nơi logging đồng bộ (synchronous) có thể làm chậm luồng xử lý chính của ứng dụng. 
- Khi sử dụng asynchronous logging, ứng dụng có thể tiếp tục xử lý mà không cần chờ mỗi log message được ghi xong.

-- Quản lý vòng đời file log (Log File Retention) -- 
- Để tránh file log chiếm quá nhiều dung lượng ổ đĩa, NLog hỗ trợ archive (lưu trữ) log dựa trên thời gian (daily, hourly, ...) hoặc dung lượng file.
- Archiving trong NLog là quá trình đóng file log hiện tại, sau đó đổi tên hoặc di chuyển nó sang thư mục archive theo lịch định sẵn.
- Ví dụ cấu hình để:
  - Lưu trữ log hàng ngày (archiveEvery="Day")
  - Giữ tối đa 7 file archive (maxArchiveFiles="7")
  - Tự động lưu trữ nếu file vượt quá 10MB (archiveAboveSize="10485760")