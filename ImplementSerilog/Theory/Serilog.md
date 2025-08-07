-- What is Serilog in ASP.NET Core? --
- Logging (ghi log) là một thành phần thiết yếu trong bất kỳ ứng dụng web nào có độ tin cậy cao. 
- Trong ASP.NET Core Web API, Serilog ngày càng được các lập trình viên ưa chuộng nhờ tính đơn giản, linh hoạt và khả năng hỗ trợ nhiều tính năng nâng cao như logging có cấu trúc (structured logging) và logging bất đồng bộ (asynchronous logging). 
- Serilog là một thư viện logging bên thứ ba phổ biến, hiệu năng cao dành cho các ứng dụng .NET (bao gồm ASP.NET Core Web API), cung cấp cho lập trình viên một cách mạnh mẽ và linh hoạt để thu thập, lưu trữ và phân tích dữ liệu log.
- Khác với các framework logging truyền thống (thường ghi log dưới dạng văn bản đơn thuần), Serilog hỗ trợ logging có cấu trúc 
  - ví dụ như log dưới dạng JSON hoặc các cặp key-value — giúp dữ liệu log trở nên có ý nghĩa hơn và dễ dàng truy vấn, phân tích hơn.
  - Ngoài ra, Serilog hỗ trợ rất nhiều sink (đích đến của log), cho phép ghi log ra file, cơ sở dữ liệu, hoặc các hệ thống lưu trữ khác một cách dễ dàng.
-- Cài đặt các gói NuGet cần thiết cho Serilog --
- Trước khi bắt đầu viết mã, chúng ta cần cài đặt các gói Serilog cần thiết. Các gói này cho phép ghi log đến nhiều đích khác nhau (gọi là sinks) và hỗ trợ cấu hình thông qua file appsettings.json. 
- Cụ thể bao gồm:
  1. Serilog.AspNetCore: Tích hợp Serilog vào ASP.NET Core, thay thế provider ghi log mặc định của framework.
  2. Serilog.Sinks.Console: Cho phép xuất log ra màn hình console – rất hữu ích trong quá trình phát triển để theo dõi log theo thời gian thực.
  3. Serilog.Sinks.File: Cho phép ghi log vào file trên đĩa – phù hợp cho lưu trữ lâu dài, phục vụ audit hoặc phân tích sau khi xảy ra sự cố.
  4. Serilog.Settings.Configuration: Hỗ trợ cấu hình Serilog thông qua file appsettings.json, giúp dễ dàng thay đổi thiết lập log mà không cần sửa mã nguồn.
  5. Serilog.Sinks.Async: Bọc các sink bằng cơ chế xử lý bất đồng bộ (asynchronous), giúp cải thiện hiệu năng bằng cách đẩy việc ghi log sang luồng nền.

-- What is a Sink and what are commonly provided sinks by Serilog in ASP.NET Core? --
- Trong Serilog, "Sink" là nơi tiếp nhận và lưu trữ các log được ghi ra từ ứng dụng. 
- Khi ứng dụng tạo ra log, Sink sẽ quyết định log đó sẽ được ghi đến đâu.
- Ví dụ: trong quá trình phát triển, bạn có thể muốn xem log ngay trên Console, hoặc ghi log ra file để lưu trữ lâu dài, hoặc gửi log đến cơ sở dữ liệu hay các dịch vụ logging trên cloud như Seq, Elasticsearch, Application Insights...
- Khi cấu hình Serilog, bạn có thể chỉ định một hoặc nhiều Sink để điều hướng dữ liệu log đến các đích khác nhau. Mỗi đích như vậy được gọi là một Sink. Dưới đây là một số Sink phổ biến trong ASP.NET Core khi sử dụng Serilog:
  - Console Sink: Ghi log ra màn hình console. Rất hữu ích trong quá trình phát triển khi cần theo dõi log theo thời gian thực.
  - File Sink: Ghi log vào file trên ổ đĩa. Hỗ trợ các tính năng như rolling file (tạo file log mới theo chu kỳ thời gian hoặc kích thước) để quản lý dung lượng và lưu trữ log hiệu quả.
  - Debug Sink: Gửi log đến cửa sổ Output của trình gỡ lỗi (ví dụ: Output trong Visual Studio). Hữu ích khi chạy và debug ứng dụng.
  - Database Sink (ví dụ: SQL Server, PostgreSQL, MongoDB): Ghi log vào cơ sở dữ liệu. Thích hợp với các môi trường cần truy vấn log có cấu trúc hoặc cần lưu trữ log lâu dài phục vụ phân tích.
  - Asynchronous Sink: Bọc (wrap) các Sink khác để xử lý log theo kiểu bất đồng bộ (async). Điều này giúp tăng hiệu năng cho ứng dụng khi cần ghi log với tần suất cao.

-- What is a Sink and what are commonly provided sinks by Serilog in ASP.NET Core? --
- Sink trong Serilog là nơi tiếp nhận và lưu trữ log đầu ra (output). Khi ứng dụng tạo log, sink quyết định log đó sẽ được ghi vào đâu. Ví dụ:
  - Ghi log ra Console (phục vụ debug lúc phát triển)
  - Ghi vào file (lưu trữ lâu dài)
  - Gửi log đến cơ sở dữ liệu hoặc hệ thống log tập trung như Seq, Elasticsearch, Azure Application Insights,…

-- What is Structured Logging? --
- Structured Logging (Ghi log có cấu trúc) trong ASP.NET Core là hình thức ghi log bằng cách ghi lại các đối tượng và thuộc tính của chúng dưới dạng cấu trúc dữ liệu (thường là JSON), thay vì chỉ ghi chuỗi văn bản thuần túy (plain text). 
- Điều này giúp các hệ thống log có thể tìm kiếm, lọc và phân tích dữ liệu dễ dàng hơn.
- Với Serilog, việc này rất đơn giản: chỉ cần truyền trực tiếp đối tượng vào log message và sử dụng cú pháp đặc biệt để Serilog giữ lại cấu trúc của đối tượng.

-- What is the @ operator in LogInformation? -- 
- Trong Serilog, @ được dùng để serialize đối tượng dưới dạng dữ liệu có cấu trúc (structured data).
- Ví dụ: @Book sẽ log toàn bộ các thuộc tính của object Book như một đối tượng JSON, thay vì gọi ToString().

-- Why Use Structured Logging? --
- Có thể lọc log theo thuộc tính (ví dụ: theo Id, Author) thay vì đọc chuỗi log thuần túy.
- Hữu ích khi log được gửi đến các hệ thống như Seq, Elasticsearch, hay Application Insights — nơi hỗ trợ truy vấn và hiển thị dữ liệu có cấu trúc.

-- When to use structured logging? -- 
- Khi cần ghi log chi tiết của một thực thể hoặc sự kiện.
- Ví dụ: Khi thêm sách → log toàn bộ object Book; khi lấy danh sách sách → log toàn bộ danh sách Books.

-- What is Asynchronous Logging? --
- Asynchronous Logging (ghi log bất đồng bộ) là quá trình ghi lại các log theo cách không chặn luồng chính, nghĩa là quá trình xử lý chính của ứng dụng không cần chờ ghi log xong mới tiếp tục thực thi. 
- Thay vào đó, các log được đưa vào hàng đợi và xử lý ở luồng nền hoặc tác vụ chạy nền.
- Trong cách ghi log truyền thống (synchronous logging), mỗi lần ghi log có thể khiến ứng dụng bị chậm lại vì phải chờ ghi vào ổ đĩa, database hoặc gọi đến dịch vụ từ xa. 
- Điều này đặc biệt dễ xảy ra khi hệ thống xử lý tải cao.
- Asynchronous logging giúp khắc phục bằng cách tách việc ghi log sang luồng khác, tăng hiệu năng hệ thống, đặc biệt là với các ứng dụng web hoặc API có lượng truy cập lớn.

-- How Do We Implement Asynchronous Logging with Serilog in ASP.NET Core Web API? --
- Trong ASP.NET Core, Serilog không hỗ trợ ghi log bất đồng bộ mặc định. 
- Tuy nhiên, bạn có thể bật tính năng này thông qua package Serilog.Sinks.Async (giả sử bạn đã cài đặt package này).
