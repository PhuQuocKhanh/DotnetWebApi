Phương thức HTTP POST
- Phương thức HTTP POST được sử dụng để gửi dữ liệu lên máy chủ nhằm tạo một tài nguyên mới.
- Dữ liệu được gửi thông qua POST sẽ nằm trong body của yêu cầu HTTP. 
- Đây là một trong những phương thức HTTP phổ biến nhất, thường được dùng để gửi dữ liệu từ form hoặc tải tệp lên server.
- Đặc điểm của phương thức POST:
1. Đóng gói dữ liệu (Data Encapsulation):
  - Dữ liệu trong POST được đóng gói bên trong phần body của request, giúp bảo mật hơn so với gửi qua URL. 
  - Điều này đặc biệt quan trọng khi truyền các thông tin nhạy cảm như mật khẩu, mã PIN, số thẻ tín dụng,...
2. Không giới hạn kích thước dữ liệu:
  - Không giống như phương thức GET, POST không bị giới hạn độ dài của request body. 
  - Điều này khiến POST phù hợp với các tình huống cần truyền tải lượng dữ liệu lớn, chẳng hạn như upload tệp tin.
3. Không mang tính chất idempotent:
  - POST là phương thức không định danh (non-idempotent) — nghĩa là việc gửi cùng một yêu cầu POST nhiều lần có thể dẫn đến những kết quả khác nhau (ví dụ: tạo nhiều bản ghi giống nhau trong cơ sở dữ liệu).
4. Trường hợp sử dụng:
- POST thường được dùng trong các tình huống như:
  - Gửi dữ liệu từ form (form submission),
  - Tải tệp lên máy chủ (file upload),
  - Thực hiện các thao tác làm thay đổi trạng thái hoặc dữ liệu phía server (tạo bản ghi, thêm dữ liệu, xử lý giao dịch...).
- Mã trạng thái phản hồi:
- Các phản hồi thành công của POST thường trả về mã HTTP:
  - 200 OK: Khi cập nhật thành công và có phản hồi trả về,
  - 201 Created: Khi tài nguyên mới được tạo thành công,
  - 204 No Content: Khi thao tác thành công nhưng không có dữ liệu trả về.
- Header thường dùng:
  - Content-Type: Xác định định dạng dữ liệu gửi lên (ví dụ: application/json, application/xml, multipart/form-data khi upload file,...),
  - Content-Length: Xác định kích thước của phần body được gửi.

-- When Should We Use HTTP Post Method in ASP.NET Core Web API? --
- Phương thức HTTP POST thường được sử dụng trong ASP.NET Core Web API để tạo mới tài nguyên hoặc gửi dữ liệu từ client lên server để xử lý. 
- Một số trường hợp cụ thể nên sử dụng POST bao gồm:

1. Tạo mới tài nguyên (Creating New Resources)
  - Dùng khi cần tạo một thực thể mới trên server, ví dụ như thêm người dùng mới vào cơ sở dữ liệu.
  ➤ Dữ liệu gửi lên sẽ nằm trong body của request.
2. Gửi dữ liệu từ biểu mẫu (Submitting Form Data)
  - Khi client gửi form (ví dụ: form đăng ký người dùng, đăng bài viết, gửi bình luận), phương thức POST sẽ được sử dụng để gửi dữ liệu đó lên server.
3. Tải lên tệp tin (Uploading Files)
  - Trong các API hỗ trợ upload file, POST thường được dùng để truyền dữ liệu file (dạng multipart/form-data) qua request body.
4. Thao tác phức tạp tạo ra tài nguyên (Complex Operations That Result in Creation)
  - Nếu quá trình tạo tài nguyên đòi hỏi tính toán hoặc xử lý phức tạp dựa trên dữ liệu đầu vào, POST là lựa chọn phù hợp.
5. Xử lý hàng loạt (Bulk Operations)
  - POST được dùng khi cần gửi một tập dữ liệu lớn (nhiều thực thể) để tạo cùng lúc nhiều bản ghi trong hệ thống.

- 🔁 So sánh HTTP GET và POST trong ASP.NET Core Web API
1. Mục Tiêu: 
- HTTP GET Lấy dữ liệu từ server mà không làm thay đổi trạng thái của nó (read-only).
- HTTP POST Gửi dữ liệu lên server để tạo hoặc cập nhật tài nguyên.
2. Truyền dữ liệu: 
- HTTP GET Dữ liệu được truyền qua query string trong URL (dễ thấy, không bảo mật).
- HTTP POST Dữ liệu được đặt trong request body (an toàn hơn cho dữ liệu nhạy cảm).
3. Idempotency (Tính bất biến)
- HTTP GET Có: gọi nhiều lần không làm thay đổi trạng thái hệ thống.
- HTTP POST Không có: gọi nhiều lần có thể tạo ra nhiều bản ghi giống nhau.
4. Khả năng cache: 
- HTTP GET Có thể được cache bởi trình duyệt hoặc proxy.
- HTTP POST Thường không được cache.
5. Bảo mật:
- HTTP GET Không phù hợp với dữ liệu nhạy cảm do dữ liệu lộ trên URL.
- HTTP POST Bảo mật hơn vì dữ liệu không hiển thị trong URL.
6. Giới hạn dữ liệu:
- HTTP GET Có giới hạn độ dài URL (tuỳ vào trình duyệt & server).
- HTTP POST Không giới hạn thực tế (phụ thuộc vào cấu hình server).
7. Trường hợp sử dụng
- HTTP GET Dùng khi chỉ cần truy xuất dữ liệu, ví dụ: tìm kiếm, phân trang, lọc danh sách.
- HTTP POST Dùng khi cần gửi dữ liệu để tạo/cập nhật, ví dụ: form, upload file, tạo giao dịch.

-- Khi nào nên chọn GET hay POST? --
- Chọn GET nếu chỉ cần truy vấn hoặc lấy dữ liệu mà không thay đổi trạng thái server.
  👉 Ví dụ: lấy danh sách người dùng, tìm kiếm theo từ khoá.
- Chọn POST nếu:
  - Gửi dữ liệu để tạo mới hoặc cập nhật.
  - Gửi dữ liệu nhạy cảm.
  - Upload file.
  - Dữ liệu gửi lên có kích thước lớn (vượt quá giới hạn URL của GET).