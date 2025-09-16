-- Minimal API trong ASP.NET Core-- 
- Minimal API là cách tiếp cận mới, gọn nhẹ để xây dựng HTTP API trong ASP.NET Core (ra mắt từ .NET 6), nhấn mạnh vào sự đơn giản, nhanh chóng và giảm tối đa code lặp. 
- Mục tiêu là giúp lập trình viên tạo API một cách nhanh gọn, đặc biệt phù hợp cho các dự án nhỏ hoặc microservices, mà không cần đến toàn bộ hạ tầng phức tạp như trong ASP.NET Core MVC (controller, attribute, cấu hình routing phức tạp,...). 
- Với Minimal API, ta có thể định nghĩa trực tiếp endpoint trong file Program.cs bằng cú pháp ngắn gọn theo hướng hàm (functional).

-- Đặc điểm chính của Minimal API: -- 
  - Lightweight (Gọn nhẹ): Không cần tạo controller riêng hay cấu hình MVC routing. Chỉ cần viết các đoạn code nhỏ để định nghĩa method HTTP (GET, POST, PUT, DELETE) và logic xử lý tương ứng.
  - Functional Approach (Tiếp cận hàm): Thay vì mô hình controller/action hướng đối tượng, Minimal API khuyến khích cách viết hàm lambda hoặc delegate để xử lý request/response trực tiếp.
  - Reduced Dependencies (Giảm phụ thuộc): Không cần load toàn bộ MVC framework nếu không cần. Vẫn chạy trên pipeline middleware của ASP.NET Core, nhưng chỉ bật những gì cần thiết.
  - Focused Use Case (Tập trung đúng nhu cầu): Thích hợp khi chỉ cần xây dựng API đơn giản, nhanh gọn — ví dụ microservices, serverless function, hoặc backend có ít endpoint.

-- Những điểm quan trọng về các phương thức này --
- Ánh xạ trực tiếp Route và Handler:
  - Các phương thức này định nghĩa route và hàm xử lý request ngay trong một vị trí duy nhất (tệp Program.cs), loại bỏ nhu cầu phải tạo các lớp controller riêng biệt.
- Cơ chế Binding tham số tự động: 
  - Minimal API tự động bind các tham số từ route (ví dụ: {id}) và nội dung request body (ví dụ: JSON biểu diễn một Employee) vào tham số hoặc đối tượng trong phương thức.
- Kiểu trả về và kết quả:
  - Bạn có thể trả về trực tiếp các đối tượng POCO (ASP.NET Core sẽ tự động serialize thành JSON), hoặc sử dụng các helper như Results.Ok(), Results.NotFound(), Results.Created() để trả về mã trạng thái HTTP tương ứng kèm dữ liệu.
- Tích hợp Middleware: 
  - Các endpoint được định nghĩa theo cách này vẫn tham gia đầy đủ vào pipeline middleware của ASP.NET Core, cho phép áp dụng xác thực, phân quyền, logging, xử lý ngoại lệ, và các middleware khác trước hoặc sau khi thực thi handler.

-- Xác thực Request trong Minimal APIs -- 
  - Trong Minimal APIs, việc xác thực dữ liệu request đầu vào là rất quan trọng để đảm bảo API chỉ xử lý những thông tin hợp lệ. Xác thực giúp ngăn chặn dữ liệu sai gây lỗi hoặc làm hỏng trạng thái ứng dụng. Có hai cách chính để thực hiện validation trong Minimal APIs:
  - Data Annotation Attributes: Trang trí (decorate) các thuộc tính của model bằng các attribute như [Required], [StringLength], [Range], …
→ Cách này đơn giản, có sẵn trong .NET, phù hợp với nhu cầu xác thực cơ bản.
  - Thư viện ngoài (ví dụ FluentValidation): Phù hợp cho các kịch bản xác thực nâng cao, tùy biến nhiều.
  - Trong ví dụ này, ta sẽ tập trung vào xác thực dựa trên Data Annotation – cách phổ biến và dễ áp dụng nhất cho dự án từ cơ bản đến trung cấp.

