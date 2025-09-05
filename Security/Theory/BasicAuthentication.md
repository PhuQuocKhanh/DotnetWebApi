-- Basic Authentication là gì --
- Xác thực Cơ bản (Basic Authentication) là một trong những phương pháp đơn giản và dễ hiểu nhất để bảo mật Web API và các dịch vụ HTTP khác. 
- Nó hoạt động bằng cách yêu cầu client (chẳng hạn như trình duyệt web, ứng dụng di động hoặc các bên tiêu thụ API khác) gửi tên người dùng (username) và mật khẩu (password) với mỗi yêu cầu (request). 
- Cặp tên người dùng và mật khẩu này được sử dụng để xác minh danh tính của client trước khi cấp quyền truy cập vào tài nguyên được yêu cầu.

-- Basic Authentication hoạt động như thế nào --
1. Client Gửi Thông tin xác thực (Credentials) với Mỗi Yêu cầu:
- Mỗi khi client muốn truy cập một endpoint API được bảo vệ, nó sẽ thêm một header Authorization vào yêu cầu HTTP. 
- Header này chứa tên người dùng và mật khẩu được kết hợp thành một chuỗi duy nhất và được mã hóa bằng Base64.

2. Mã hóa Thông tin xác thực:
- Client đầu tiên nối chuỗi tên người dùng và mật khẩu thành một chuỗi duy nhất, được phân tách bằng dấu hai chấm (:). Ví dụ: Username:Password
- Giả sử tên người dùng là john.doe@example.com và mật khẩu là password123, chuỗi kết hợp sẽ là: john.doe@example.com:password123
- Chuỗi này sau đó được chuyển đổi sang mã hóa Base64, có thể trông giống như sau: am9obi5kb2VAZXhhbXBsZS5jb206cGFzc3dvcmQxMjM=
- Mã hóa Base64 là một phương pháp chuyển đổi dữ liệu nhị phân thành định dạng văn bản; đây không phải là mã hóa bảo mật (encryption) và không bảo vệ dữ liệu khỏi bị đọc.

3. Định dạng Header Authorization:
- Chuỗi đã được mã hóa được đặt trong header HTTP Authorization, với tiền tố là từ Basic theo sau là một khoảng trắng: Authorization: Basic am9obi5kb2VAZXhhbXBsZS5jb206cGFzc3dvcmQxMjM=

4. Xác thực Phía Máy chủ (Server-Side):
- Khi máy chủ API (ví dụ: ứng dụng ASP.NET Core Web API) nhận được một yêu cầu có header Authorization, nó sẽ:
  - Trích xuất chuỗi mã hóa Base64 từ header.
  - Giải mã (decode) nó để lấy lại chuỗi "username:password" ban đầu.
  - Tách chuỗi để lấy riêng tên người dùng và mật khẩu.
  - Xác thực các thông tin này bằng cách kiểm tra chúng với một kho lưu trữ người dùng, chẳng hạn như cơ sở dữ liệu hoặc một bộ sưu tập trong bộ nhớ.
  - Nếu thông tin xác thực hợp lệ, máy chủ sẽ xử lý yêu cầu và trả về tài nguyên được bảo vệ.
  - Nếu không hợp lệ, máy chủ sẽ trả về một phản hồi HTTP 401 Unauthorized.

-- Khi nào nên sử dụng Xác thực Cơ bản trong ứng dụng thực tế? -- 
- Xác thực Cơ bản có thể được xem xét trong các kịch bản sau:
  1. Các Dịch vụ Nội bộ Đơn giản: 
  - Đối với các API nội bộ nhỏ hoặc các microservice chạy trong một môi trường đáng tin cậy (chẳng hạn như trong mạng công ty), Basic Authentication đôi khi có thể chấp nhận được. 
  - Nó dễ cài đặt và sử dụng, đặc biệt nếu bạn không cần các tính năng bảo mật nâng cao. Luôn đảm bảo sử dụng SSL/TLS (HTTPS) để bảo vệ thông tin xác thực.
  2. Môi trường Rủi ro Thấp: 
  - Nếu dữ liệu trao đổi không quá nhạy cảm, hoặc giao tiếp đã được bảo mật qua mã hóa TLS, và ứng dụng của bạn không yêu cầu các tính năng nâng cao như token hết hạn, thu hồi token, hoặc ủy quyền, Basic Authentication có thể đáp ứng nhu cầu của bạn.
  3. Tương thích Ngược: 
  - Một số hệ thống cũ hoặc client của bên thứ ba chỉ hỗ trợ Basic Authentication. 
  - Trong những trường hợp như vậy, có thể cần phải sử dụng nó để duy trì khả năng tương thích.

-- Hạn chế của Xác thực Cơ bản --
- Mặc dù đơn giản, Basic Authentication có một số hạn chế đáng kể:
1. Thông tin xác thực được gửi với mọi yêu cầu: 
  - Tên người dùng và mật khẩu được truyền đi trên mỗi lệnh gọi API, làm tăng nguy cơ bị chặn nếu không sử dụng HTTPS.
2. Không có Quản lý Session hoặc Token tích hợp: 
  - Không có cơ chế để làm hết hạn session hoặc thu hồi quyền truy cập mà không cần thay đổi thông tin xác thực, gây khó khăn trong việc quản lý session của người dùng một cách an toàn.
3. Dễ bị tấn công nếu không có HTTPS: 
  - Thông tin xác thực chỉ được mã hóa Base64, không phải mã hóa bảo mật, vì vậy nếu được gửi qua HTTP thông thường, chúng có thể dễ dàng bị kẻ tấn công bắt và giải mã.
4. Base64 KHÔNG PHẢI là Mã hóa Bảo mật: 
  - Base64 chỉ là một kỹ thuật mã hóa (encoding). 
  - Bất kỳ ai chặn được yêu cầu HTTP đều có thể dễ dàng giải mã thông tin xác thực và lấy được tên người dùng và mật khẩu.
5. Không hỗ trợ các tính năng bảo mật hiện đại: 
  - Nó không hỗ trợ xác thực đa yếu tố, làm mới token, hoặc đăng nhập một lần (single sign-on), những tính năng cần thiết cho các ứng dụng an toàn hiện đại.

