-- Result Filter trong ASP.NET Core Web API là gì? -- 
- Result Filter là một bộ lọc trong ASP.NET Core cho phép chúng ta chặn và thao tác với kết quả của một phương thức action ngay trước khi nó được ghi vào response và ngay sau khi việc thực thi kết quả hoàn tất. 
- Dưới đây là hai giai đoạn mà Result Filters được thực thi trong Pipeline Xử lý Request của ASP.NET Core:
  1. Xử lý trước khi thực thi kết quả (Pre-Result Processing): 
    - Chạy logic trước khi action result được thực thi 
    (ví dụ: sửa đổi đối tượng kết quả, thay đổi định dạng response, thiết lập các header tùy chỉnh, v.v.).
  2. Xử lý sau khi thực thi kết quả (Post-Result Processing): 
    - Chạy logic sau khi action result đã được thực thi 
    (ví dụ: ghi log cho response, kiểm toán (auditing), hoặc áp dụng các biến đổi đầu ra tùy chỉnh).
- Result Filters là lựa chọn lý tưởng để xử lý các kịch bản yêu cầu thao tác với response sau khi phương thức action chạy nhưng trước khi kết quả được gửi đến client.


-- Result Filters hoạt động như thế nào trong ASP.NET Core Web API? -- 
- ASP.NET Core cung cấp hai interface chính để tạo Result Filters:
  1. IResultFilter: Dành cho các hoạt động đồng bộ, với hai phương thức:
    - OnResultExecuting(ResultExecutingContext context)
    - OnResultExecuted(ResultExecutedContext context)
  2. IAsyncResultFilter: Dành cho các hoạt động bất đồng bộ, với một phương thức:
    - Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
- Bạn có thể triển khai trực tiếp một trong hai interface này hoặc sử dụng lớp cơ sở ResultFilterAttribute, vốn hỗ trợ việc sử dụng dựa trên attribute và cung cấp các phương thức ảo (virtual methods) để ghi đè. Result Filters thực thi tại hai điểm trong pipeline của response:
  1. OnResultExecuting (trước khi thực thi kết quả): Cho phép kiểm tra và sửa đổi kết quả trước khi nó được gửi đến client.
  2. OnResultExecuted (sau khi thực thi kết quả): Cho phép xử lý sau đó, chẳng hạn như ghi log hoặc kiểm toán, sau khi kết quả đã được gửi đi.

-- Lợi ích của Result Filters trong ASP.NET Core: -- 
- Dưới đây là những lợi ích của việc sử dụng Result Filters trong một ứng dụng ASP.NET Core:
  1. Tách biệt các mối quan tâm (Separation of Concerns): 
    - Giữ logic liên quan đến response ra khỏi controller, giúp controller chỉ tập trung vào các quy tắc nghiệp vụ và thực thi action.
  2. Tái sử dụng mã (Code Reusability): 
    - Triển khai xử lý response chung một lần và áp dụng nó trên toàn cục hoặc chọn lọc bằng cách sử dụng các attribute.
  3. Tính nhất quán (Consistency): 
    - Đảm bảo rằng tất cả các response API tuân theo cùng một quy tắc về header, định dạng, caching hoặc ghi log.
  4. Khả năng bảo trì (Maintainability): 
    - Dễ dàng cập nhật hành vi của response bằng cách thay đổi logic của filter thay vì phải sửa đổi nhiều action hoặc controller.
