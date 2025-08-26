-- Exception Filter trong ASP.NET Core Web API là gì? -- 
- Exception Filter là một loại bộ lọc đặc biệt chỉ thực thi khi có một ngoại lệ chưa được xử lý xảy ra trong pipeline xử lý request của ASP.
- NET Core, cụ thể là trong quá trình:
  - Thực thi phương thức action
  - Thực thi các bộ lọc action (Action Filters)
  - Thực thi các bộ lọc kết quả (Result Filters)
  - Thực thi các bộ lọc tài nguyên (Resource Filters)
- Exception Filters bắt các ngoại lệ này và cung cấp một cách xử lý tập trung bằng cách:
  - Ghi log (Logging) chi tiết lỗi để chẩn đoán.
  - Trả về các thông báo lỗi tùy chỉnh và mã trạng thái HTTP (HTTP status codes).
  - Ngăn chặn việc rò rỉ chi tiết ngoại lệ nhạy cảm cho client.
  - Thực hiện logic dự phòng hoặc phục hồi nếu cần thiết.

-- Khi nào Exception Filters chạy? -- 
- Exception Filters là một trong những tuyến phòng thủ cuối cùng trong pipeline của bộ lọc. 
- Chúng được kích hoạt khi một ngoại lệ chưa được xử lý xảy ra trong:
  - Quá trình thực thi của một phương thức action.
  - Quá trình thực thi của các bộ lọc khác như Action Filters, Result Filters, hoặc Resource Filters.
  - Chúng chạy sau khi action bắt đầu nhưng trước khi phản hồi được gửi lại cho client, tạo cơ hội để xử lý ngoại lệ, thiết lập một phản hồi phù hợp và ngắt mạch (short-circuit) pipeline. 
  - Exception Filters không bắt các ngoại lệ được ném ra trong middleware, chỉ bắt trong pipeline của MVC (action/filter).

-- Các loại Exception Filters -- 
- Bạn có thể tạo ra:
  - Exception Filters đồng bộ (Synchronous): 
    - Triển khai giao diện IExceptionFilter (dành cho logic nhanh, không bất đồng bộ).
  - Exception Filters bất đồng bộ (Asynchronous): 
    - Triển khai giao diện IAsyncExceptionFilter (dành cho việc ghi log đến các kho lưu trữ từ xa, v.v.).
  - Bộ lọc dựa trên Attribute: 
    - Kế thừa từ ExceptionFilterAttribute để dễ dàng sử dụng như một attribute.

-- Tại sao chúng ta cần Exception Filters? -- 
1. Xử lý lỗi tập trung: 
  - Thay vì thêm các khối try-catch trong mỗi action của controller, chúng ta xử lý ngoại lệ một cách toàn cục ở một nơi duy nhất.
2. Phản hồi API nhất quán: 
  - Chúng ta có thể trả về một định dạng phản hồi lỗi thống nhất (ví dụ: JSON với mã lỗi, thông báo, và mã trạng thái) cho tất cả các client API.
3. Bảo mật: 
  - Ngăn chặn stack traces hoặc các chi tiết nội bộ nhạy cảm bị rò rỉ ra bên ngoài client.
4. Ghi log & Giám sát: 
  - Tạo điều kiện thuận lợi cho việc ghi lại các log lỗi chi tiết để gỡ lỗi hoặc kiểm tra.
5. Code sạch hơn: 
  - Giữ cho các phương thức action của chúng ta sạch sẽ và tập trung vào logic nghiệp vụ mà không cần lo lắng về việc xử lý ngoại lệ.
- Exception Filters là yếu tố cần thiết để xây dựng các ASP.NET Core Web API mạnh mẽ, an toàn và thân thiện với người dùng. 
- Chúng cung cấp logic tập trung, có thể tái sử dụng để xử lý lỗi, ghi log và trả về các phản hồi lỗi nhất quán, cải thiện đáng kể độ tin cậy của API và trải nghiệm của nhà phát triển.

-- Khi nào nên chọn cái nào? -- 
- Sử dụng Middleware khi bạn muốn xử lý ngoại lệ một cách nhất quán, toàn cục trên toàn bộ ứng dụng, bao gồm cả các route không phải MVC hoặc các file tĩnh.
- Sử dụng Exception Filters nếu bạn muốn xử lý lỗi dành riêng cho MVC với quyền truy cập vào context của action, hoặc muốn xử lý ngoại lệ trên cơ sở từng controller hoặc từng action.
