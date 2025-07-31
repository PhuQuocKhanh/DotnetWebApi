-- What are HTTP Methods? --
- HTTP (Hypertext Transfer Protocol) cung cấp các phương thức chuẩn hóa để client (trình duyệt, ứng dụng, v.v.) gửi yêu cầu đến server nhằm thao tác với tài nguyên trên hệ thống. 
- Đây là thành phần cốt lõi trong thiết kế RESTful API, cho phép client và server giao tiếp và xử lý dữ liệu một cách rõ ràng, nhất quán.
- Mỗi HTTP Method xác định rõ hành động cần thực hiện với tài nguyên cụ thể, chẳng hạn như: truy vấn dữ liệu, tạo mới, cập nhật hoặc xóa.

- Các loại HTTP Method phổ biến:
1. ✅ HTTP GET
  - Dùng để lấy dữ liệu từ server mà không làm thay đổi trạng thái của tài nguyên.
  - Là phương thức idempotent – gọi nhiều lần vẫn trả về kết quả như nhau và không gây thay đổi dữ liệu.
  - Ví dụ: Gửi yêu cầu GET đến /api/users để nhận danh sách người dùng.
  - Trong ASP.NET Core Web API, sử dụng [HttpGet] để định nghĩa phương thức này.
2. ✅ HTTP POST
  - Dùng để gửi dữ liệu lên server nhằm tạo mới tài nguyên.
  - Dữ liệu thường được truyền trong body của request, định dạng theo Content-Type như application/json.
  - Ví dụ: Gửi thông tin đăng ký để tạo mới một tài khoản người dùng.
  - Trong ASP.NET Core Web API, sử dụng [HttpPost].
3. ✅ HTTP PUT
  - Dùng để cập nhật toàn bộ tài nguyên tại một URI cụ thể.
  - Nếu tài nguyên chưa tồn tại, server có thể tạo mới (tùy vào thiết kế).
  - Ví dụ: Cập nhật toàn bộ thông tin hồ sơ người dùng.
  - Trong ASP.NET Core Web API, sử dụng [HttpPut].
4. ✅ HTTP PATCH
  - Dùng để cập nhật một phần tài nguyên (khác với PUT là cập nhật toàn bộ).
  - Chỉ gửi lên phần dữ liệu cần thay đổi.
  - Ví dụ: Cập nhật riêng địa chỉ email của người dùng mà không ảnh hưởng các trường khác.
  - Trong ASP.NET Core Web API, sử dụng [HttpPatch].
5. ✅ HTTP DELETE
  - Dùng để xóa tài nguyên trên server.
  - Ví dụ: Gửi yêu cầu xóa một tài khoản người dùng cụ thể.
  - Trong ASP.NET Core Web API, sử dụng [HttpDelete].
6. ✅ HTTP HEAD
  - Tương tự GET nhưng không trả về nội dung (body) – chỉ trả về header.
  - Thường được dùng để kiểm tra metadata của tài nguyên (như Content-Length, Content-Type) trước khi gửi yêu cầu GET thực sự.
  - Trong ASP.NET Core Web API, sử dụng [HttpHead].
7. ✅ HTTP OPTIONS
  - Dùng để truy vấn các tùy chọn cấu hình mà server hỗ trợ đối với tài nguyên cụ thể.
  - Thường gặp trong các request CORS preflight, dùng để kiểm tra trước xem server có cho phép các phương thức như PUT, DELETE,... không.
  - Trong ASP.NET Core Web API, sử dụng [HttpOptions] (lưu ý: chính tả đúng là "Options", không phải "Otions" như đoạn gốc).

-- What are Safe and Unsafe HTTP Methods? -- 
- Trong HTTP, các phương thức được phân loại dựa trên khả năng tác động đến trạng thái của server. 
- Chúng được chia thành hai nhóm: 
  1. Phương thức an toàn (Safe Methods)
  2. Phương thức không an toàn (Unsafe Methods).

1. Phương thức HTTP an toàn (Safe HTTP Methods):
- Là những phương thức không làm thay đổi trạng thái dữ liệu trên server. 
- Chúng thường dùng để truy vấn dữ liệu (read-only), không tạo, cập nhật hoặc xóa tài nguyên.
- Các phương thức này có thể được gọi nhiều lần (idempotent) mà không gây ra tác dụng phụ (side effects).
- Một số phương thức an toàn phổ biến:
  - GET: Truy vấn dữ liệu từ server. Không làm thay đổi dữ liệu.
  - HEAD: Giống như GET, nhưng chỉ trả về phần header (metadata) của response, không bao gồm body.
  - OPTIONS: Yêu cầu server cung cấp thông tin về các phương thức được hỗ trợ cho một resource cụ thể (dùng nhiều trong CORS).
2. Phương thức HTTP không an toàn (Unsafe HTTP Methods):
- Là các phương thức có khả năng làm thay đổi trạng thái trên server, chẳng hạn như tạo mới, cập nhật hoặc xóa tài nguyên. 
- Chúng có thể gây tác động đến dữ liệu hoặc hệ thống backend, do đó cần được sử dụng cẩn thận, đặc biệt trong các môi trường sản xuất.
- Một số phương thức không an toàn:
  - POST: Gửi dữ liệu lên server để tạo mới một resource. Có thể gây thay đổi dữ liệu.
  - PUT: Ghi đè (replace) toàn bộ resource tại URL được chỉ định. Thay đổi trạng thái server.
  - PATCH: Cập nhật một phần của resource. Cũng làm thay đổi trạng thái server.
  - DELETE: Xóa một resource khỏi server. Gây thay đổi dữ liệu.

-- What are Idempotent Methods in ASP.NET Core Web API --
- Trong ngữ cảnh ASP.NET Core Web API, một phương thức Idempotent (bất biến) là phương thức HTTP mà khi được gọi nhiều lần với cùng một yêu cầu thì kết quả trả về vẫn giống nhau, không gây ra thay đổi trạng thái nào thêm trên server sau lần gọi đầu tiên. Nói cách khác, gọi nhiều lần cũng như chỉ gọi một lần.
- Các phương thức Idempotent trong ASP.NET Core Web API:
1. GET
  - Dùng để lấy dữ liệu, không gây thay đổi trạng thái server.
  - Gọi nhiều lần sẽ trả về cùng một kết quả (nếu dữ liệu không bị thay đổi bởi yêu cầu khác).
  - Ví dụ: gọi API GET /api/users/5 nhiều lần sẽ luôn trả về thông tin của user ID 5, miễn là user này không bị cập nhật hoặc xóa.
2. PUT
  - Dùng để cập nhật toàn bộ tài nguyên (thay thế hoàn toàn).
  - Gửi cùng một payload nhiều lần sẽ dẫn đến trạng thái tài nguyên giống nhau.
  - Ví dụ: gọi PUT /api/products/10 với body { "price": 100 } nhiều lần thì giá sản phẩm vẫn là 100, không thay đổi thêm gì nữa.
3. DELETE
  - Dùng để xóa tài nguyên.
  - Sau khi tài nguyên bị xóa, các lần gọi tiếp theo không làm thay đổi gì nữa (tài nguyên không còn).
  - Ví dụ: gọi DELETE /api/files/abc.pdf nhiều lần — lần đầu xóa file, các lần sau không làm gì cả vì file đã không tồn tại.
4. HEAD
  - Tương tự như GET nhưng chỉ trả về header, không có body.
  - Gọi nhiều lần không ảnh hưởng đến tài nguyên.
  - Ví dụ: HEAD /api/documents/123 chỉ kiểm tra metadata (như Content-Length, ETag, v.v.).
5. OPTIONS
  - Dùng để kiểm tra phương thức HTTP được hỗ trợ cho một endpoint.
  - Không làm thay đổi dữ liệu hoặc cấu hình server.
  - Ví dụ: OPTIONS /api/users trả về các phương thức được hỗ trợ như GET, POST, DELETE.
6. PATCH (có thể là idempotent hoặc không)
  - Dùng để cập nhật một phần tài nguyên.
  - Nếu PATCH thực hiện cùng một thay đổi mỗi lần (ví dụ: luôn đặt trạng thái user là "active"), thì nó là idempotent.
  - Nhưng nếu PATCH có tác dụng tích lũy (như tăng loginCount), thì nó không idempotent.
  - Ví dụ:
    - PATCH /api/users/1 với { "status": "active" } → idempotent
    - PATCH /api/users/1 với { "loginCount": "+1" } → không idempotent

-- Non-Idempotent Method in ASP.NET Core Web API: --
- Trong bối cảnh ASP.NET Core Web API, phương thức HTTP không idempotent là phương thức mà khi được gọi nhiều lần với cùng một yêu cầu (request), sẽ tạo ra kết quả khác nhau mỗi lần gọi. 
- Việc lặp lại cùng một request có thể gây ra các tác dụng phụ bổ sung hoặc làm thay đổi trạng thái máy chủ theo cách khác với lần gọi đầu tiên.
- POST: Đây là một phương thức không idempotent vì gửi cùng một request POST nhiều lần thường sẽ tạo ra nhiều tài nguyên mới hoặc làm thay đổi trạng thái hệ thống theo những cách khác nhau. 
- Ví dụ: việc gửi một form nhiều lần có thể dẫn đến việc tạo nhiều bản ghi trong cơ sở dữ liệu.

-- Best Practices for Using HTTP Methods in ASP.NET Core Web API: --
1. GET: Sử dụng cho các thao tác đọc dữ liệu (read-only), an toàn, không làm thay đổi trạng thái của server.
2. POST: Sử dụng để tạo mới tài nguyên hoặc gửi dữ liệu lên server để xử lý (ví dụ: tạo bản ghi mới, submit form).
3. PUT: Sử dụng để cập nhật hoặc thay thế hoàn toàn một tài nguyên đã tồn tại.
4. PATCH: Sử dụng để cập nhật một phần của tài nguyên khi chỉ cần thay đổi một vài trường dữ liệu.
5. DELETE: Sử dụng để xóa một tài nguyên khỏi server.
6. OPTIONS: Sử dụng để kiểm tra các phương thức HTTP được hỗ trợ bởi server cho một endpoint cụ thể.
7. HEAD: Sử dụng khi chỉ cần lấy metadata của tài nguyên (headers), không cần nội dung (body).