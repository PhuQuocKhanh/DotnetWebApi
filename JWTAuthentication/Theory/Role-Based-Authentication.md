-- Vai trò và cách thức hoạt động của Role-Based Authentication --
- Role-Based Authentication (RBA), hay còn gọi là Role-Based Access Control (RBAC), là một mô hình bảo mật giúp phân quyền truy cập cho người dùng dựa trên vai trò của họ trong một hệ thống hoặc tổ chức. 
- Hiểu đơn giản, thay vì cấp quyền truy cập trực tiếp cho từng người dùng, chúng ta sẽ gộp các quyền này lại thành một "vai trò" (role), sau đó gán vai trò đó cho người dùng.
- Một vai trò (role) chính là một tập hợp các quyền (permission) được xác định trước, ví dụ: "Admin", "Seller", hay "Customer". 
- Mỗi vai trò sẽ có một bộ quyền hạn riêng biệt để truy cập và thực hiện các thao tác trong ứng dụng.

-- Cách thức hoạt động -- 
- Quy trình của RBA diễn ra theo các bước sau:
1. Định nghĩa vai trò (Define Roles):
  - Đầu tiên, bạn cần xác định các vai trò cần thiết trong hệ thống, tương ứng với các cấp độ truy cập khác nhau.
  - Ví dụ:
    - Admin: Có quyền cao nhất, có thể quản lý sản phẩm, xem tất cả đơn hàng, và quản lý người dùng.
    - Seller (Người bán): Chỉ được phép thêm và quản lý sản phẩm của mình, xem các đơn hàng của riêng mình.
    - Customer (Khách hàng): Chỉ có thể xem sản phẩm, đặt hàng, và xem lịch sử đơn hàng của bản thân.
2. Gán vai trò cho người dùng (Assign Roles to Users):
  - Sau khi tạo vai trò, bạn sẽ gán một hoặc nhiều vai trò cho mỗi người dùng, tùy thuộc vào nhiệm vụ của họ. 
  - Ví dụ, người dùng A có thể được gán vai trò "Seller", còn người dùng B sẽ có vai trò "Customer".

3. Thực thi quyền truy cập (Enforce Access Control):
  - Khi một người dùng thực hiện một hành động (ví dụ: truy cập một trang quản lý hay bấm nút "Xóa sản phẩm"), hệ thống sẽ kiểm tra xem vai trò của người dùng đó có được phép thực hiện hành động này hay không.
    - Nếu vai trò của người dùng có quyền tương ứng, hành động sẽ được thực thi.
    - Nếu không, hệ thống sẽ từ chối truy cập và thông báo lỗi.

-- Ưu điểm -- 
- RBA mang lại nhiều lợi ích, đặc biệt là trong các hệ thống lớn và phức tạp:
  - Dễ quản lý: 
    - Khi cần thay đổi quyền, bạn chỉ cần chỉnh sửa quyền của vai trò đó, và tất cả người dùng được gán vai trò đó sẽ tự động được cập nhật. 
    - Bạn không cần phải thay đổi quyền cho từng người dùng một.
  - Tăng cường bảo mật: 
    - Giúp giảm thiểu rủi ro khi một người dùng có quá nhiều quyền hạn không cần thiết.
  - Giảm thiểu lỗi: 
    - Việc phân quyền theo vai trò rõ ràng giúp giảm thiểu các lỗi liên quan đến việc cấp quyền sai.
  - Mở rộng dễ dàng: 
    - Khi cần thêm một nhóm người dùng mới, bạn chỉ cần tạo một vai trò mới và gán nó cho các người dùng đó.

-- Tại sao chúng ta cần xác thực mã thông báo dựa trên vai trò? -- 
- Xác thực mã thông báo dựa trên vai trò (Role-Based Token Authentication) là sự kết hợp giữa kiểm soát truy cập dựa trên vai trò (Role-Based Access Control - RBAC) và xác thực dựa trên mã thông báo (Token-Based Authentication). 
- Phương pháp này thường được triển khai bằng JSON Web Tokens (JWT). 
- Nó mang lại nhiều lợi thế quan trọng trong các hệ thống hiện đại, đặc biệt là trong kiến trúc vi dịch vụ (microservices) và các ứng dụng phân tán.
- Các lợi ích chính
1. Khả năng mở rộng (Scalability)
  - Vì thông tin về vai trò và quyền hạn đã được mã hóa ngay trong mã thông báo (token), máy chủ không cần phải liên tục truy vấn cơ sở dữ liệu để kiểm tra vai trò của người dùng. 
  - Điều này đặc biệt hiệu quả trong các môi trường phân tán hoặc phi trạng thái (stateless), nơi mỗi yêu cầu (request) từ người dùng là một thực thể độc lập. 
  - Khi một dịch vụ nhận được token, nó có thể xác thực và trích xuất thông tin vai trò ngay lập tức mà không cần gọi đến một dịch vụ quản lý người dùng tập trung.

2. Giảm tải cho máy chủ xác thực (Reduce Load on the Authentication Server)
- Với JWT, chính máy khách (client) sẽ nắm giữ mã thông báo. 
- Máy chủ chỉ cần thực hiện việc xác thực mã thông báo và các thông tin (claims) của nó (bao gồm cả vai trò). 
- Điều này giúp đơn giản hóa logic xác thực và giảm đáng kể tải lên máy chủ xác thực trung tâm. 
- Máy chủ xác thực chỉ làm nhiệm vụ cấp token ban đầu, sau đó các dịch vụ khác có thể tự xử lý các yêu cầu tiếp theo.

3. Bảo mật và xác minh vai trò (Security and Role Verification)
- Việc nhúng vai trò vào token có nghĩa là mọi dịch vụ hay thành phần nào nhận được token đều có thể ngay lập tức biết người dùng có thể làm gì mà không cần liên lạc với một máy chủ xác thực tập trung.
- Miễn là token được ký (signed) và chữ ký của nó được xác minh (verified), bạn có thể tin tưởng vào các thông tin (claims) bên trong.
- Chữ ký số đảm bảo rằng token không bị thay đổi bởi kẻ tấn công.

4. Tính linh hoạt (Flexibility)
- Xác thực mã thông báo dựa trên vai trò cho phép chúng ta dễ dàng hỗ trợ các kịch bản phức tạp:
  - Người dùng đa vai trò (multi-role users): Một người dùng có thể vừa là "quản trị viên" vừa là "người dùng thường".
  - Vai trò phân cấp (hierarchical roles): Ví dụ, vai trò "quản lý" tự động có tất cả các quyền của vai trò "nhân viên".
  - Kiểm soát truy cập dựa trên quyền (permissions-based access): Thay vì gán vai trò, chúng ta có thể gán trực tiếp các quyền cụ thể, chẳng hạn như post:create, user:delete, v.v. Tất cả những thông tin này đều có thể được mã hóa vào một token nhỏ gọn và an toàn.

5. Kiến trúc phi trạng thái (Stateless Architecture)
- Mã thông báo cho phép triển khai kiến trúc máy chủ phi trạng thái. 
- JWT là một mã thông báo tự chứa (self-contained), mang theo tất cả thông tin cần thiết, giúp loại bỏ nhu cầu lưu trữ phiên (session) trên máy chủ. 
- Mỗi yêu cầu đều chứa token, cho phép máy chủ xử lý mà không cần phải ghi nhớ trạng thái của người dùng từ các yêu cầu trước đó. 
- Điều này giúp hệ thống trở nên mạnh mẽ (robust) và dễ dàng mở rộng theo chiều ngang (horizontal scaling).