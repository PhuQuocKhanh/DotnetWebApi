-- Action Filter là gì? -- 
- Trong ASP.NET Core Web API, một Action Filter là một thành phần cho phép chúng ta chèn logic tùy chỉnh vào các thời điểm:
  1. Xử lý trước khi Action thực thi (Pre-Action Processing): 
    - Trước khi một action method chạy (OnActionExecuting) để xác thực dữ liệu đầu vào, sửa đổi tham số, thiết lập context cần thiết, hoặc ngắt chuỗi xử lý (short-circuit).
  2. Xử lý sau khi Action thực thi (Post-Action Processing): 
    - Sau khi một action method hoàn tất (OnActionExecuted) để sửa đổi kết quả trả về (action result), thực hiện ghi log, hoặc dọn dẹp tài nguyên.
- Action Filters cung cấp một cách thức gọn gàng, tái sử dụng được để đóng gói các mối quan tâm xuyên suốt (cross-cutting concerns) liên quan đến việc thực thi action, chẳng hạn như ghi log, xác thực, chuyển đổi dữ liệu, hoặc giám sát.

-- Khi nào nên sử dụng Action Filters trong ASP.NET Core Web API? -- 
- Chúng ta nên sử dụng Action Filters trong ASP.NET Core Web API khi cần triển khai các mối quan tâm xuyên suốt đòi hỏi thực thi logic tùy chỉnh ngay trước và/hoặc sau khi một action method chạy. 
- Chúng rất lý tưởng cho các kịch bản như xác thực đầu vào, ghi log, kiểm tra quyền, định dạng response, hoặc đo lường hiệu suất, nơi mà hành vi này cần được áp dụng nhất quán trên nhiều action hoặc controller mà không làm lộn xộn business logic. 
- Action Filters giúp giữ cho các controller gọn gàng, thúc đẩy tái sử dụng code, và tập trung hóa các xử lý chung liên quan đến việc thực thi action.

-- Action Filters hoạt động như thế nào trong ASP.NET Core Web API? -- 
- ASP.NET Core cung cấp hai interface để tạo action filter:
- IActionFilter: Dành cho các hoạt động đồng bộ (synchronous) với hai phương thức:
  1. OnActionExecuting(ActionExecutingContext context)
  2. OnActionExecuted(ActionExecutedContext context)

- IAsyncActionFilter: Dành cho các hoạt động bất đồng bộ (asynchronous) với một phương thức:
  1. Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)

- Bạn có thể triển khai một trong hai interface này hoặc kế thừa từ lớp cơ sở ActionFilterAttribute, lớp này cung cấp các triển khai mặc định và cho phép sử dụng dễ dàng dưới dạng attribute. 
- Action Filters thực thi tại hai điểm quan trọng trong pipeline xử lý request HTTP:
  1. OnActionExecuting (Trước khi Action thực thi)
    - Phương thức này chạy ngay trước khi action method được gọi.
    - Bạn có thể kiểm tra hoặc sửa đổi các tham số đầu vào, thực hiện validation, hoặc thậm chí ngắt pipeline bằng cách thiết lập một response tùy chỉnh.
  2. OnActionExecuted (Sau khi Action thực thi)
    - Phương thức này chạy ngay sau khi action method đã kết thúc.
    - Bạn có thể kiểm tra hoặc sửa đổi action result được trả về, thực hiện ghi log, hoặc xử lý các tác vụ hậu xử lý.

-- Khi nào nên sử dụng IActionFilter? -- 
- Chúng ta nên sử dụng interface IActionFilter khi logic của action filter tùy chỉnh đơn giản và đồng bộ, chỉ liên quan đến các hoạt động nhanh, tốn CPU (CPU-bound) mà không cần await các tác vụ bất đồng bộ. 
- Cách tiếp cận này lý tưởng khi chúng ta muốn kiểm soát rõ ràng hành vi trước và sau khi thực thi action mà không cần xử lý sự phức tạp của bất đồng bộ, chẳng hạn như ghi log, xác thực đầu vào, hoặc sửa đổi các tham số action một cách đơn giản.
