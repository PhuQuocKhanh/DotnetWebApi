-- What is Logging? --
- Logging (ghi log) là quá trình ghi lại thông tin về quá trình thực thi của một ứng dụng. 
- Nó giúp lập trình viên chẩn đoán lỗi, theo dõi hành vi của ứng dụng và phân tích hiệu năng. 
- Các bản ghi (log) này có thể bao gồm lỗi (errors), cảnh báo (warnings), thông báo thông tin (informational messages), chi tiết gỡ lỗi (debugging details), và các sự kiện nghiêm trọng (critical events). 
- Việc ghi log cung cấp dữ liệu quan trọng cho lập trình viên và quản trị viên để hiểu được cách ứng dụng hoạt động trong các môi trường khác nhau, từ đó dễ dàng xác định và khắc phục sự cố.

-- What is Logging in ASP.NET Core? --
- Trong ASP.NET Core, logging là một tính năng được tích hợp sẵn trong framework, cho phép lập trình viên ghi lại thông tin về hành vi của ứng dụng trong quá trình runtime (thời gian chạy). 
- Hệ thống logging của ASP.NET Core được thiết kế có cấu trúc và dễ mở rộng, hỗ trợ ghi log theo nhiều cấp độ (Trace, Debug, Information, Warning, Error, Critical) và cho phép định tuyến log đến nhiều "logging provider" khác nhau.
- ASP.NET Core hỗ trợ cả các nhà cung cấp log tích hợp sẵn (built-in) như Console, Debug, EventSource... và các provider bên thứ ba như Serilog, NLog, log4net... Việc tích hợp logging giúp ứng dụng dễ dàng giám sát, phân tích và xử lý sự cố theo thời gian thực hoặc sau khi triển khai.

-- What Are the Logging Providers Supported in ASP.NET Core? --
- ASP.NET Core hỗ trợ nhiều loại logging provider khác nhau (bao gồm cả mặc định và bên thứ ba). 
- Mỗi provider sẽ ghi log ra một nơi khác nhau như console, debug, file hoặc cơ sở dữ liệu.
1. Các Provider tích hợp sẵn (Built-in Providers)
  - Console Provider:
    - Ghi log ra console (thường là terminal hoặc cửa sổ Output trong Visual Studio). Thích hợp cho quá trình phát triển và debug cục bộ.
  - Debug Provider: 
    - Ghi log ra cửa sổ Debug Output của Visual Studio hoặc trình gỡ lỗi bất kỳ. Hữu ích khi debug trong môi trường local.
  - EventLog Provider: 
    - Ghi log vào Windows Event Log (chỉ khả dụng trên hệ điều hành Windows). 
    - Thích hợp cho các hệ thống triển khai on-premises trên Windows Server.
  - Azure App Service Provider: 
    - Ghi log vào hệ thống logging tích hợp sẵn của Azure khi ứng dụng được host trên Azure App Service. 
    - Cho phép xem log trực tiếp từ Azure Portal.
- Các Provider của bên thứ ba (Third-Party Providers)
- ASP.NET Core hỗ trợ tích hợp với các thư viện logging phổ biến để cung cấp thêm tính năng và khả năng kiểm soát log tốt hơn, như:
  - Serilog: Thư viện logging bên thứ ba phổ biến với các tính năng nâng cao như structured logging, custom sinks và hỗ trợ nhiều định dạng output khác nhau.
  - NLog: Tương tự như Serilog, NLog nổi bật với tính linh hoạt cao và hỗ trợ nhiều tính năng nâng cao.
  - Log4Net: Một thư viện logging lâu đời và được sử dụng rộng rãi trong hệ sinh thái .NET. Hỗ trợ cấu hình mạnh mẽ và tùy biến sâu.
- Lưu ý: 
  - ASP.NET Core cũng cho phép bạn tự tạo Custom Logging Provider. 
  - Trong các phần sau, chúng ta sẽ tìm hiểu cách triển khai một Logging Provider tùy chỉnh trong ứng dụng ASP.NET Core Web API.

-- What are the Different Log Levels in ASP.NET Core? --
- ASP.NET Core định nghĩa một tập hợp các mức log (log levels) chuẩn để phân loại mức độ nghiêm trọng và tầm quan trọng của các thông điệp log. Việc phân loại này giúp lập trình viên dễ dàng lọc log và nhanh chóng xác định các sự cố quan trọng. 
- Trong ASP.NET Core, LogLevel là một enum được định nghĩa trong namespace Microsoft.Extensions.Logging.
- Các mức log tiêu chuẩn bao gồm:
1. Trace:
  - Mức log chi tiết nhất. 
  - Thường dùng trong quá trình phát triển hoặc khi cần debug chuyên sâu. Bao gồm mọi thông tin có thể thu thập được để theo dõi luồng xử lý.
2. Debug: 
  - Log chứa thông tin hữu ích trong quá trình phát triển và gỡ lỗi, nhưng không quá chi tiết như Trace. 
  - Thường không được bật trong môi trường production.
3. Information:
  - Ghi lại luồng hoạt động chính của ứng dụng, chẳng hạn như các sự kiện quan trọng (ví dụ: người dùng đăng nhập, xử lý đơn hàng thành công...). 
  - Những log này thường có giá trị lâu dài để theo dõi hệ thống.
4. Warning:
  - Cảnh báo về các tình huống có thể gây lỗi trong tương lai. 
  - Không gây gián đoạn hoạt động hiện tại nhưng cần được lưu ý và theo dõi.
5. Error:
  - Ghi lại các lỗi phát sinh trong quá trình thực thi. 
  - Đây là những lỗi nghiêm trọng hơn, cần được xử lý, nhưng ứng dụng vẫn có thể tiếp tục chạy.
6. Critical:
  - Mức nghiêm trọng nhất. 
  - Ghi lại các lỗi gây sập hệ thống hoặc có nguy cơ mất dữ liệu. Cần được xử lý khẩn cấp.
7. None:
  - Mức đặc biệt dùng để vô hiệu hóa toàn bộ hệ thống log.

-- ILogger<T> in ASP.NET Core Web API --
- Trong ASP.NET Core, ILogger<T> là một interface generic do namespace Microsoft.Extensions.Logging cung cấp, giúp tích hợp logging vào ứng dụng một cách đơn giản và hiệu quả. 
- Tham số generic T đại diện cho lớp đang sử dụng logger, cho phép tự động chèn tên lớp vào mỗi log để dễ dàng truy vết nguồn gốc log hơn. 
- Điều này đặc biệt hữu ích trong việc xác định chính xác vị trí phát sinh log trong toàn bộ ứng dụng.
- Các đặc điểm chính:
  - Ghi log theo nhiều cấp độ khác nhau: Trace, Debug, Information, Warning, Error, Critical.
  - Tự động chèn tên lớp (T) vào log để dễ dàng truy nguyên nguồn gốc log.
  - Tích hợp với nhiều provider khác nhau: Console, Debug, File, Database hoặc các hệ thống logging bên thứ ba như Serilog, NLog,…

- Các phương thức logging theo cấp độ:
1. LogTrace()
  - Dùng để ghi lại các log chi tiết cao, thường áp dụng trong quá trình debug. Ví dụ: ghi nhận việc đi vào/thoát ra khỏi phương thức, giá trị biến,… Thường chỉ bật trong môi trường dev/test do lượng log lớn.
2. LogDebug()
  - Dùng để ghi thông tin chẩn đoán mức trung bình. Phù hợp với các log ghi lại trạng thái nội bộ hoặc các bước xử lý trong ứng dụng, chủ yếu phục vụ việc debug.
3. LogInformation()
  - Ghi lại các sự kiện bình thường trong ứng dụng, như: khởi động dịch vụ, xử lý thành công request, hoàn thành thao tác chính,… Mức này thường được bật trong production để theo dõi hoạt động của hệ thống.
4. LogWarning()
  - Thông báo một vấn đề tiềm ẩn hoặc sự kiện bất thường, nhưng chưa nghiêm trọng.
  - Ví dụ: dịch vụ ngoài phản hồi chậm, file cấu hình phụ bị thiếu,…
5. LogError()
  - Ghi nhận lỗi khi có thao tác bị thất bại. 
  - Ví dụ: lỗi kết nối database, lỗi API trả mã trạng thái không hợp lệ,… Mặc dù ứng dụng vẫn tiếp tục chạy, nhưng lỗi này cần được xem xét và xử lý.
6. LogCritical()
  - Dành cho các lỗi nghiêm trọng có thể khiến ứng dụng crash hoặc mất chức năng quan trọng.
  - Những log này báo hiệu sự cố hệ thống cần được xử lý ngay lập tức.

-- Performance Implications of Logging in ASP.NET Core --
- Mặc dù logging là một phần quan trọng, nó vẫn có thể tạo ra một số tác động đến hiệu năng. 
- Thông thường, chi phí này khá nhỏ, nhưng việc ghi log quá nhiều (ví dụ: lạm dụng mức Trace hoặc Debug) có thể làm chậm ứng dụng và tăng mức tiêu thụ tài nguyên. 
- Do đó, cần lưu ý:
  - Sử dụng mức log phù hợp cho từng môi trường (ví dụ: Information hoặc Warning trong môi trường production).
  - Tránh ghi log bên trong các vòng lặp chặt hoặc trong các đoạn code yêu cầu hiệu năng cao.
  - Cân nhắc logging bất đồng bộ và offload log sang hệ thống lưu trữ/xử lý bên ngoài.
  - Nếu ghi log ra file, lập kế hoạch xoay vòng (log rotation) và thời gian lưu giữ log hợp lý — vì lưu trữ quá nhiều log có thể làm đầy ổ đĩa.
  - Ghi log ra các tài nguyên bên ngoài (như file hoặc database) có thể tốn nhiều chi phí I/O. Buffering hoặc batching các thao tác ghi log sẽ giúp cải thiện hiệu năng.
- Lưu ý: Các thư viện logging bên thứ ba như Serilog hoặc NLog có thể cải thiện hiệu năng nhờ hỗ trợ batching và asynchronous logging. Tuy nhiên, chúng cũng làm tăng độ phức tạp hệ thống.

-- Can we store the log in a file or database using the Default Logging Provider? --
- Mặc định, các logging provider tích hợp sẵn trong ASP.NET Core (như Console, Debug) không hỗ trợ trực tiếp việc ghi log vào file hoặc database.
- Để lưu log vào file hoặc database, thông thường bạn sẽ:
  - Dùng Logging Framework bên thứ ba
  - Tích hợp các thư viện như Serilog, NLog, hoặc Log4Net.
  - Các thư viện này hỗ trợ ghi log ra nhiều đích khác nhau: file, database, dịch vụ logging từ xa...
- Tự viết Logging Provider
  - Tạo một lớp triển khai ILoggerProvider và ILogger.
  - Cách này cho phép bạn điều hướng log tới bất kỳ nơi lưu trữ nào, nhưng yêu cầu nhiều công sức phát triển và bảo trì hơn.

