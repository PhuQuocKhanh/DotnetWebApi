-- How Do We Implement Custom Logging Provider in ASP.NET Core? --
- ASP.NET Core cung cấp một framework logging linh hoạt và dễ mở rộng, cho phép lập trình viên dễ dàng tích hợp các cơ chế logging tùy chỉnh theo nhu cầu. 
- Bằng cách hiện thực (implement) các interface ILogger và ILoggerProvider, chúng ta có thể lưu trữ log tới nhiều nơi khác nhau như file system, cơ sở dữ liệu hoặc các dịch vụ bên ngoài, đồng thời vẫn tận dụng được các tính năng sẵn có như log levels, structured logging và scope management.
- Trong ví dụ này, chúng ta sẽ tìm hiểu cách tạo custom logging provider trong ứng dụng ASP.NET Core Web API, để ghi log đồng thời vào file system và SQL Server database thông qua Entity Framework Core. Ứng dụng sẽ minh họa các bước:
  - Định nghĩa class custom logger và custom logger provider.
  - Cấu hình log levels và bộ lọc log (filtering).
  - Sử dụng Dependency Injection để tích hợp custom logging provider.
  - Lưu trữ log vào hai backend khác nhau: file system và SQL Server database.

-- What is BeginScope? -- 
- BeginScope là một phương thức được định nghĩa trong interface ILogger, dùng để tạo logging scope (phạm vi log).
- Logging scope là một khối mã có ngữ cảnh riêng, trong đó một số thông tin log nhất định (ví dụ: Customer ID, Unique ID) sẽ tự động được thêm vào tất cả các log bên trong phạm vi đó.
- Điều này rất hữu ích khi muốn gom nhóm các log liên quan đến cùng một tác vụ hoặc request, đặc biệt trong môi trường phân tán hoặc đa tenant.
- Ví dụ: Nếu bạn bắt đầu một scope mới với User ID hoặc Transaction ID cụ thể, mọi log trong scope đó sẽ tự động chứa thông tin này. Khi phân tích log, bạn sẽ dễ dàng truy ngược chuỗi sự kiện về cùng một user hoặc request.

-- Vai trò của ILoggerProvider
- ILoggerProvider định nghĩa hợp đồng để tạo logger instance.
- Mỗi provider chịu trách nhiệm tạo và quản lý logger trong hệ thống logging ASP.NET Core.
- Mỗi logger được gắn với một category (thường là namespace hoặc tên class) và dùng để ghi log ở nhiều cấp độ (Trace, Debug, Information, Warning, Error, Critical).
- Khi ASP.NET Core tạo logger cho một category, nó sẽ gọi CreateLogger của từng ILoggerProvider đã đăng ký. Provider sẽ trả về một ILogger (ví dụ CustomLogger) để thực hiện việc ghi log thực tế.

-- Steps to Implement Custom Logger in ASP.NET Core Web API: --
- Chúng tôi đã thực hiện các bước sau để tạo một custom logging provider cho ASP.NET Core, cho phép ghi log vào hệ thống file và SQL Server thông qua Entity Framework Core (EF Core).
- Các bước chính bao gồm:
1. Định nghĩa Model và DbContext cho Log:
  - Tạo entity LogEntry để biểu diễn một bản ghi log, đồng thời cấu hình LoggingDbContext để thao tác với cơ sở dữ liệu.
2. Cài đặt CustomLogger:
  - Triển khai một lớp ILogger tùy chỉnh, chịu trách nhiệm format thông điệp log và ghi chúng vào cả file hệ thống lẫn SQL Server.
  - Trong quá trình này, chúng tôi sử dụng IServiceScopeFactory để lấy một instance của LoggingDbContext theo scope phù hợp, đảm bảo an toàn và quản lý vòng đời đúng cách.
3. Cài đặt CustomLoggerProvider:
  - Xây dựng một lớp ILoggerProvider tùy chỉnh, chịu trách nhiệm khởi tạo và cung cấp các instance của CustomLogger.
4. Đăng ký vào Pipeline của ASP.NET Core:
  - Thiết lập EF Core, cấu hình CustomLoggerProvider, và đảm bảo rằng log được ghi đúng vào cả hai đích: file hệ thống và cơ sở dữ liệu SQL Server.
