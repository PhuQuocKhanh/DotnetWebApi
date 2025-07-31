-- Phương thức HTTP GET -- 
- Phương thức HTTP GET là một trong các phương thức của Giao thức Truyền tải Siêu văn bản (HTTP), được sử dụng để yêu cầu dữ liệu từ một tài nguyên được chỉ định. Trong bối cảnh Web, đây là một trong những phương thức phổ biến nhất để truy xuất thông tin.

1. Định nghĩa và Mục đích sử dụng
- Mục đích: Phương thức GET được thiết kế để truy xuất thông tin từ máy chủ. 
- Các yêu cầu GET chỉ nên dùng để đọc dữ liệu và không được gây ra bất kỳ thay đổi nào trên máy chủ.
- Tính chất Idempotent: GET là phương thức idempotent, nghĩa là việc gửi cùng một yêu cầu nhiều lần sẽ cho kết quả như nhau và không gây ra tác động phụ.
- Tính an toàn (Safe): GET là phương thức an toàn, vì nó không làm thay đổi trạng thái của máy chủ.
2. Đặc điểm kỹ thuật 
- Mã hóa URL: Các tham số trong yêu cầu GET được gắn trực tiếp vào URL dưới dạng chuỗi truy vấn (query string). 
- Ví dụ: http://example.com/api?param1=value1&param2=value2
Các tham số này thường bao gồm thông tin tìm kiếm hoặc định danh để lấy dữ liệu mong muốn.
- Giới hạn độ dài: 
- Do tham số được gửi qua URL, nên GET bị giới hạn về độ dài URL, tùy thuộc vào trình duyệt và máy chủ xử lý. 
- Điều này giới hạn lượng dữ liệu có thể truyền qua phương thức này.
- Khả năng cache: Phản hồi từ các yêu cầu GET thường có thể được cache bởi trình duyệt hoặc các proxy trung gian, giúp tăng hiệu năng khi truy cập lại cùng một tài nguyên.
3. Phản hồi HTTP
- Mã trạng thái (Status Code): Phản hồi từ máy chủ sẽ bao gồm mã trạng thái để cho biết kết quả của yêu cầu, ví dụ:
- 200 OK: Thành công, dữ liệu đã được trả về.
- 404 Not Found: Tài nguyên không tồn tại.
- 403 Forbidden: Bị từ chối truy cập tài nguyên.

-- GET Request URL Length Limitations: --
- HTTP specification không quy định giới hạn cụ thể cho độ dài của URL trong yêu cầu GET. 
- Thay vào đó, giới hạn này phụ thuộc vào trình duyệt và máy chủ web đang sử dụng. 
- Các giới hạn này có thể khác nhau và đã thay đổi theo thời gian tùy vào phiên bản của trình duyệt và máy chủ. 
- Dưới đây là một số giới hạn phổ biến:
  - Edge: Giới hạn độ dài URL khoảng 2.083 ký tự. Các phiên bản Edge mới (dựa trên Chromium) có thể hỗ trợ URL dài hơn, tương đương với Chrome.
  - Firefox: Không công bố giới hạn chính thức, nhưng URL quá dài (trên 64.000 ký tự) có thể gây lỗi. Thực tế cho thấy nên giữ URL dưới 2.000 ký tự để đảm bảo tương thích.
  - Chrome: Giới hạn khoảng 8.192 ký tự.
  - Safari: Hỗ trợ URL dài đến 80.000 ký tự, nhưng tốt nhất vẫn nên giữ dưới 2.000 ký tự để tránh sự cố.
  - Opera: Dựa trên Chromium nên cũng có giới hạn khoảng 8.192 ký tự.
  - Máy chủ Web: Giới hạn phụ thuộc vào cấu hình. Ví dụ:
    - Apache: Mặc định giới hạn 8.192 ký tự.
    - IIS: Mặc định hỗ trợ tối đa 16.384 ký tự.

-- When Should We Use HTTP GET Method in ASP.NET Core Web API? --
- Hiểu rõ thời điểm sử dụng phương thức GET là điều quan trọng để xây dựng API tuân theo chuẩn REST. Một số tình huống điển hình:
  - Truy xuất danh sách dữ liệu: Dùng GET để lấy danh sách các resource. Ví dụ: GET /api/users để lấy toàn bộ người dùng.
  - Lấy chi tiết theo ID: Truy xuất một resource cụ thể theo định danh. Ví dụ: GET /api/users/123 để lấy thông tin user có ID là 123.
  - Tìm kiếm dữ liệu: GET có thể dùng để thực hiện tìm kiếm với các tiêu chí truyền qua query string. Ví dụ: GET /api/users?name=JohnDoe.
  - Lọc, sắp xếp, phân trang: Sử dụng GET để truyền các tham số lọc, phân trang. Ví dụ: GET /api/users?page=2&limit=20.
  - Lấy dữ liệu liên quan: Dùng GET để lấy các resource liên quan, ví dụ: GET /api/users/123/orders để lấy danh sách đơn hàng của user 123.

-- Best Practices for Using GET Requests in ASP.NET Core Web API --
- Tính idempotent: GET phải idempotent – gọi nhiều lần vẫn cho kết quả như nhau và không gây tác dụng phụ (không làm thay đổi dữ liệu).
- Dùng query parameters cho dữ liệu tùy chọn: Đối với filter, search, pagination… nên dùng query string (?key=value) thay vì nhét dữ liệu vào đường dẫn.
- Hỗ trợ cache: Do GET không thay đổi dữ liệu, nên có thể cache kết quả để cải thiện hiệu năng.
- Sử dụng HTTP status code phù hợp:
  - 200 OK: Thành công.
  - 404 Not Found: Không tìm thấy resource.
  - 400 Bad Request: Truy vấn sai cú pháp hoặc dữ liệu không hợp lệ.
- Giới hạn dữ liệu trả về: Không nên trả về quá nhiều dữ liệu trong một lần GET. Sử dụng cơ chế paging, filtering, limiting để giới hạn khối lượng dữ liệu.
- Hành động bất đồng bộ (Async): Ưu tiên viết action method dạng async để tăng khả năng mở rộng, đặc biệt khi có thao tác I/O như truy vấn cơ sở dữ liệu.

