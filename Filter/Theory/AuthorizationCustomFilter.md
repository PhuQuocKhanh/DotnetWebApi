-- Custom Authorization Filter là gì? -- 
- Trong ASP.NET Core Web API, bộ lọc ủy quyền (authorization filters) là các thành phần được thực thi sớm trong pipeline xử lý request HTTP để xác định xem người dùng có quyền truy cập vào một endpoint API cụ thể (một phương thức action của controller) hay không. Bước này xảy ra sau khi xác thực (authentication - xác minh danh tính người dùng) nhưng trước khi phương thức của controller thực sự chạy.
- Framework cung cấp sẵn một thuộc tính [Authorize] cho phép chúng ta thực thi ủy quyền dựa trên các tiêu chí đơn giản như vai trò (roles), chính sách (policies), hoặc claim. 
- Ví dụ, bạn có thể giới hạn một endpoint để chỉ những người dùng có vai trò "Admin" mới có thể truy cập.
- Tuy nhiên, nhiều ứng dụng trong thực tế đòi hỏi các quy tắc nghiệp vụ phức tạp và tùy chỉnh hơn mà [Authorize] không thể biểu diễn trực tiếp. Đây chính là lúc Custom Authorization Filters phát huy tác dụng.

-- Khi nào nên sử dụng Custom Authorization Filters? -- 
- Ủy quyền là một vấn đề bảo mật cốt lõi trong bất kỳ Web API nào. 
- Mặc dù ASP.NET Core cung cấp các cơ chế ủy quyền tích hợp, có những tình huống cụ thể mà Custom Authorization Filters trở nên cần thiết.

1. Khi các thuộc tính ủy quyền có sẵn không đủ đáp ứng
- [Authorize] mặc định hỗ trợ các quy tắc đơn giản. Tuy nhiên, nhu cầu thực tế thường phức tạp hơn:
  - Kiểm tra nhiều điều kiện động cùng lúc (ví dụ: vai trò + trạng thái gói đăng ký + thời gian trong ngày).
  - Áp dụng các quy tắc nghiệp vụ không phù hợp với vai trò hay claim.
  - Ủy quyền có điều kiện phụ thuộc vào tham số hoặc header của request.
2. Khi ủy quyền phụ thuộc vào dữ liệu bên ngoài hoặc logic phức tạp
- Đôi khi bạn cần:
  - Truy vấn cơ sở dữ liệu để xác minh trạng thái tài khoản.
  - Gọi một API bên ngoài để kiểm tra quyền truy cập.
  - Thực thi các quy tắc truy cập nhạy cảm về thời gian.
3. Khi bạn cần logic ủy quyền tập trung
- Thay vì lặp lại cùng một logic kiểm tra ở nhiều controller, một bộ lọc tùy chỉnh đóng gói logic đó lại một lần, giúp:
  - Tái sử dụng mã (nguyên tắc DRY).
  - Dễ bảo trì và cập nhật logic ủy quyền.
  - Giữ cho controller và action gọn gàng hơn.
4. Khi bạn muốn tùy chỉnh phản hồi khi ủy quyền thất bại
- [Authorize] mặc định trả về lỗi 401 hoặc 403 chung chung. Với bộ lọc tùy chỉnh, bạn có thể:
  - Trả về thông báo lỗi tùy chỉnh giải thích lý do truy cập bị từ chối.
  - Ghi log (log) các nỗ lực truy cập trái phép với nhiều chi tiết hơn.
  - Thực hiện các hành động đặc biệt khi ủy quyền thất bại.
5. Khi bạn cần thực thi kiểm soát truy cập dựa trên ngữ cảnh
- Các yêu cầu ủy quyền có thể phụ thuộc vào tài nguyên cụ thể đang được truy cập:
  - Chỉ cho phép người dùng truy cập tài nguyên mà họ sở hữu.
  - Giới hạn truy cập dựa trên tham số truy vấn hoặc header.
  - Xác minh địa chỉ IP hoặc hạn chế theo vị trí địa lý.