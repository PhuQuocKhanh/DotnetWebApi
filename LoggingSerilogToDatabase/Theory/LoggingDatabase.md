-- Logging to SQL Server Database using Serilog in ASP.NET Core Web API --
1. Khi nào nên ghi log vào bảng cơ sở dữ liệu?
- Việc ghi log vào bảng trong cơ sở dữ liệu trở nên hữu ích khi bạn cần lưu trữ log một cách lâu dài và tập trung.
- Bằng cách lưu trữ log trong SQL Server, bạn có thể dễ dàng truy vấn, lọc và phân tích log thông qua các câu lệnh SQL.
- Điều này đặc biệt quan trọng trong việc phân tích xu hướng dài hạn, theo dõi truy vết (audit trail), hoặc xử lý sự cố trong các hệ thống phân tán.
- Tuy nhiên, việc ghi log vào cơ sở dữ liệu có thể gây ảnh hưởng đến hiệu năng, vì vậy thường chỉ áp dụng cho các log quan trọng hoặc các tình huống cần phân tích chi tiết.

-- Logging to a SQL Server Database with Serilog --
- Logging (ghi log) là một phần không thể thiếu trong bất kỳ ứng dụng sẵn sàng cho môi trường production nào. 
- Việc sử dụng Serilog cùng với SQL Server cho phép lưu trữ log một cách bền vững, phục vụ cho việc phân tích, giám sát và khắc phục sự cố sau này. 
- Mặc dù Entity Framework Core (EF Core) là một ORM mạnh mẽ để thao tác dữ liệu, Serilog không sử dụng EF Core mà sử dụng riêng một thư viện ghi log – Serilog.Sinks.MSSqlServer – để ghi trực tiếp log vào SQL Server.
- Để ghi log vào SQL Server bằng Serilog, bạn cần cấu hình một sink (đích ghi log) để gửi dữ liệu log trực tiếp đến một bảng trong cơ sở dữ liệu. 
- Thư viện Serilog.Sinks.MSSqlServer chính là công cụ giúp thực hiện điều này, cho phép ghi log từ ứng dụng ASP.NET Core vào SQL Server một cách trực tiếp và hiệu quả.

-- Understanding the Auto-Created Logs Table Structure --
- Khi Serilog tạo bảng Logs, nó sẽ định nghĩa các cột để lưu trữ thông tin chi tiết về từng sự kiện log. 
- Việc hiểu rõ ý nghĩa từng cột giúp bạn thao tác hiệu quả hơn với dữ liệu log đã lưu. 
- Dưới đây là mô tả các cột mặc định do Serilog.Sinks.MSSqlServer tạo:
  1. Id: Khóa chính tự tăng, định danh duy nhất cho mỗi bản ghi (thường là kiểu int tự tăng).
  2. Message: Chuỗi log đã được render, bao gồm các giá trị được nội suy.
  3. MessageTemplate: Mẫu log gốc dùng để sinh ra Message. Mẫu này chứa các placeholder sẽ được thay thế bằng giá trị thực tế khi render.
  4. Level: Mức độ log (ví dụ: Trace, Debug, Information, Warning, Error, Critical). Dùng để lọc log theo mức độ nghiêm trọng.
  5. TimeStamp: Thời điểm sự kiện log xảy ra.
  5. Exception: Thông tin ngoại lệ (nếu có), bao gồm thông điệp, stack trace và inner exceptions. 
                Nếu có exception, cột này chứa kết quả của ToString() từ đối tượng exception.
  6. Properties: Chứa các thuộc tính bổ sung ở dạng XML hoặc JSON (log dạng structured).

-- Logging into a SQL Server Database with Serilog in an ASP.NET Core Web API Application with Examples --
- Tuỳ chỉnh bảng Logs được Serilog tạo
- Trong một số trường hợp, bạn cần tuỳ biến bảng log để đáp ứng nhu cầu ứng dụng, ví dụ thêm các cột như:
  - UniqueId: Định danh duy nhất để liên kết các bản ghi log.
  - ServerIP: Địa chỉ IP của máy chủ sinh ra log (hữu ích trong hệ thống phân tán).