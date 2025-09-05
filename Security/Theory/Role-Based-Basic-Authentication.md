-- Xác thực cơ bản dựa trên vai trò là gì? -- 
- Xác thực cơ bản dựa trên vai trò là một cơ chế bảo mật nơi người dùng tự xác thực bằng cách sử dụng Basic Authentication (tên người dùng và mật khẩu, thường được gửi trong header Authorization của HTTP). 
- Sau khi xác thực, hệ thống sẽ phân quyền cho người dùng truy cập tài nguyên dựa trên các vai trò được gán cho họ, chẳng hạn như Admin, User, Manager, v.v.
- Basic Authentication là một lược đồ xác thực đơn giản được tích hợp sẵn trong HTTP, trong đó thông tin xác thực được gửi dưới dạng một chuỗi được mã hóa base64.
- Role-Based Authorization (Phân quyền dựa trên vai trò) kiểm tra xem người dùng thuộc những vai trò nào và cấp hoặc từ chối quyền truy cập vào các endpoint API dựa trên các vai trò đó.

-- Xác thực cơ bản dựa trên vai trò hoạt động như thế nào? --
- Chúng ta hãy cùng tìm hiểu cách thức hoạt động của Xác thực cơ bản dựa trên vai trò. Vui lòng tham khảo sơ đồ sau để hiểu rõ hơn.
- Nó sẽ hoạt động như sau:
  1. Yêu cầu từ Client (Client Request): 
    - Client gửi một yêu cầu HTTP với header Authorization chứa một token Basic Authentication (chuỗi username:password đã được mã hóa base64).
  2. Xác thực (Authentication): 
    - Server giải mã và xác thực thông tin đăng nhập.
  3. Lấy vai trò (Role Retrieval): 
    - Sau khi xác thực, server lấy các vai trò của người dùng từ một nguồn dữ liệu (ví dụ: cơ sở dữ liệu).
  4. Phân quyền (Authorization): 
    - Server kiểm tra xem các vai trò của người dùng có cho phép truy cập vào tài nguyên API được yêu cầu hay không.
    - Nếu được cấp quyền, server sẽ xử lý yêu cầu.
    - Nếu không được cấp quyền, server sẽ trả về phản hồi HTTP 401 Unauthorized hoặc 403 Forbidden.

-- Khi nào nên sử dụng Xác thực cơ bản dựa trên vai trò? -- 
- Sử dụng Xác thực cơ bản dựa trên vai trò khi:
  - Bạn cần một cách đơn giản để bảo mật API, đặc biệt là các hệ thống nội bộ, thử nghiệm hoặc cũ (legacy).
  - Người dùng có thể được gán các vai trò cố định (như "Admin", "Manager", "User").
  - Không cần các phương thức xác thực nâng cao (như OAuth, JWT, hoặc Single Sign-On).
  - Bảo mật là cần thiết, nhưng không phải là mối quan tâm hàng đầu (vì Basic Auth gửi thông tin xác thực được mã hóa, không phải mã hóa mật mã, nó nên luôn được sử dụng qua HTTPS).

