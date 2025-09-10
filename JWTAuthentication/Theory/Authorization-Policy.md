-- Authorization Policy là gì? -- 
- Authorization Policy (chính sách phân quyền) trong ASP.NET Core là một cơ chế mạnh mẽ và linh hoạt, định nghĩa một tập hợp các yêu cầu và quy tắc để xác định liệu một người dùng có được phép truy cập các tài nguyên hoặc thực hiện các hành động cụ thể trong một ứng dụng hay không.
- Các chính sách này gói gọn logic phân quyền phức tạp, cho phép các nhà phát triển thực thi kiểm soát truy cập chi tiết hơn ngoài các kiểm tra dựa trên role đơn giản.
- Một Authorization Policy là một tập hợp các yêu cầu mà người dùng phải đáp ứng để truy cập một tài nguyên hoặc thực hiện một hành động trong ứng dụng ASP.NET Core. Các chính sách này trừu tượng hóa logic phân quyền, giúp nó có thể tái sử dụng và dễ bảo trì hơn.
- Cách Hoạt Động:
  1. Define a Policy (Định nghĩa Chính sách): Chỉ định các yêu cầu mà người dùng phải đáp ứng.
  2. Register the Policy (Đăng ký Chính sách): Cấu hình chính sách trong service container của ứng dụng.
  3. Apply the Policy (Áp dụng Chính sách): Sử dụng thuộc tính [Authorize(Policy = "PolicyName")] trên các controller hoặc action để thực thi chính sách.

-- Cách Triển khai Authorization Policies trong ASP.NET Core:
- Chúng ta sẽ tạo năm chính sách sau:
  1. Only Authorize No Role (Chỉ xác thực, không cần role): 
    - Bất kỳ người dùng đã xác thực nào cũng có thể truy cập endpoint, bất kể role của họ.
  2. Authorize with Admin Role (Phân quyền với role Admin): 
    - Chỉ những người dùng có role “Admin” mới có thể truy cập endpoint.
  3. Authorize with User Role (Phân quyền với role User): 
    - Chỉ những người dùng có role “User” mới có thể truy cập endpoint.
  4. Authorize with Both Admin and User Roles (Phân quyền với cả hai role Admin và User): 
    - Chỉ những người dùng có cả hai role “Admin” và “User” mới có thể truy cập endpoint.
  5. Authorize with Either Admin or User Role (Phân quyền với một trong hai role Admin hoặc User): 
    - Người dùng có role “Admin” hoặc “User” đều có thể truy cập endpoint.

