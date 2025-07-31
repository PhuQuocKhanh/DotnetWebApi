-- HTTP PUT Method --
- Phương thức HTTP PUT là một phần của giao thức truyền siêu văn bản (HTTP), được sử dụng để cập nhật một tài nguyên trên máy chủ. 
- Khi client muốn thay thế toàn bộ nội dung của một tài nguyên hiện có, phương thức PUT sẽ được dùng để gửi yêu cầu.

- Đặc điểm chính của phương thức HTTP PUT
  1. Idempotent (Tính đồng nhất): 
    - Phương thức PUT có tính chất idempotent, tức là việc gửi cùng một yêu cầu PUT nhiều lần sẽ cho kết quả như nhau, không làm thay đổi trạng thái thêm lần nữa sau lần đầu tiên.
  2. Thay thế tài nguyên:
    - PUT được sử dụng để cập nhật hoặc thay thế toàn bộ nội dung của tài nguyên tại URL được chỉ định. 
    - Nó giả định rằng client sẽ gửi toàn bộ phiên bản mới của tài nguyên trong phần body của yêu cầu.
  3. URL đại diện cho tài nguyên:
    - Trong yêu cầu PUT, URL là định danh của tài nguyên cần cập nhật. 
    - Nếu tài nguyên tồn tại, máy chủ sẽ ghi đè nội dung bằng payload từ client. 
    - Nếu không tồn tại, tùy theo cấu hình máy chủ, có thể tạo mới tài nguyên tại URL đó hoặc trả lỗi.
  4. Không an toàn (Unsafe): 
    - PUT không được coi là an toàn vì nó thay đổi trạng thái của máy chủ.
  5. Payload:
    - Client cần gửi toàn bộ nội dung tài nguyên cần cập nhật trong phần body của yêu cầu PUT.
  6. Mã phản hồi (Response Codes):
    - 200 OK: Cập nhật thành công.
    - 201 Created: Nếu tài nguyên được tạo mới.
    - 204 No Content: Cập nhật thành công, nhưng không có nội dung trả về.
    - 404 Not Found: Không tìm thấy tài nguyên để cập nhật và cũng không thể tạo mới.

- Cách triển khai phương thức PUT trong ASP.NET Core Web API
1. Định nghĩa Model: 
  - Tạo một class C# đại diện cho tài nguyên mà bạn muốn thao tác.
  - Class này dùng để ánh xạ dữ liệu từ client gửi lên.
2. Tạo Controller: 
  - Tạo một controller kế thừa từ ControllerBase. 
  - Controller này sẽ chứa phương thức xử lý PUT.
3. Định nghĩa phương thức PUT:
  - Tạo một action method trong controller.
  - Sử dụng attribute [HttpPut] để đánh dấu phương thức này xử lý yêu cầu PUT.
  - Có thể định nghĩa route cụ thể nếu cần.
4. Cài đặt logic cập nhật:
  - Kiểm tra xem tài nguyên có tồn tại không.
  - Nếu có, cập nhật thông tin từ dữ liệu client gửi lên.
5. Ghi lại thay đổi vào database (hoặc bất kỳ nơi lưu trữ nào đang dùng).
  - Trả về phản hồi phù hợp:
    - Trả 200 OK nếu cập nhật thành công.
    - Trả 404 Not Found nếu không tìm thấy tài nguyên.
    - Trả 400 Bad Request nếu dữ liệu không hợp lệ.

-- When Should We Use HTTP PUT Method in ASP.NET Core Web API? --
- Trong ASP.NET Core Web API, phương thức HTTP PUT chủ yếu được sử dụng để cập nhật một tài nguyên (resource) hiện có trên server.
-  Dưới đây là các trường hợp sử dụng cụ thể và các lưu ý khi dùng PUT một cách hiệu quả:
1. ✅ Khi nào nên sử dụng PUT:
  - Cập nhật toàn bộ tài nguyên:
    - PUT thích hợp khi bạn cần cập nhật tất cả các thuộc tính của một tài nguyên. 
    - PUT sẽ thay thế hoàn toàn tài nguyên hiện tại bằng payload được gửi trong request. 
    - Nếu chỉ cập nhật một phần, hãy dùng PATCH.
  - Tính chất idempotent:
    - PUT là idempotent, nghĩa là gửi nhiều request PUT giống nhau sẽ cho kết quả như nhau. 
    - Điều này lý tưởng khi bạn muốn đảm bảo rằng tài nguyên luôn được cập nhật đến một trạng thái xác định, không bị ảnh hưởng bởi số lần gửi request.
  - Client xác định định danh tài nguyên:
    - Nếu client chịu trách nhiệm xác định URI của tài nguyên, thì nên dùng PUT. 
    - Ví dụ: cập nhật thông tin người dùng thông qua PUT /api/users/5.
  - Thay thế toàn bộ:
    - PUT được dùng khi bạn muốn thay thế toàn bộ nội dung hiện tại của tài nguyên bằng nội dung mới trong request body. 
    - Trong một số thiết kế API, nếu tài nguyên chưa tồn tại tại URI đó thì PUT có thể tạo mới, mặc dù thông thường POST được dùng cho mục đích tạo mới.

2. Khi không nên dùng PUT:
  - Cập nhật một phần dữ liệu:
    - Nếu bạn chỉ muốn chỉnh sửa một vài thuộc tính của tài nguyên, thì PATCH là phương án phù hợp hơn vì nó không ghi đè toàn bộ tài nguyên.
  - Không mong muốn tính idempotent:
    - Trong những trường hợp mà bạn không muốn kết quả giống nhau khi gửi lại nhiều lần, chẳng hạn khi mỗi request tạo ra một tác vụ mới hoặc thay đổi khác nhau, nên dùng POST.
  - Không có URI rõ ràng:
    - PUT yêu cầu phải gửi đến một URI xác định, nên nếu URI được server tự sinh (client không biết trước), hoặc resource không có định danh rõ ràng, hãy dùng POST.
  - Ghi đè ngoài ý muốn:
    - Do PUT thay thế toàn bộ resource, nếu client gửi một object không đầy đủ hoặc đã lỗi thời, có thể dẫn đến mất dữ liệu ngoài ý muốn.
  - Xử lý logic phức tạp:
    - Nếu việc cập nhật bao gồm logic kiểm tra dữ liệu phức tạp hoặc trigger thêm các tác vụ phụ, POST có thể phù hợp hơn. 
    - PUT chỉ nên dùng cho các thao tác cập nhật đơn giản.

-- So sánh phương thức HTTP POST và PUT trong ASP.NET Core Web API --
1. HTTP POST
- Mục đích:
  - POST dùng để tạo mới một resource trên server. 
  - Server sẽ xử lý và lưu trữ dữ liệu trong request body, thường dùng trong các form submit hoặc upload file.
- Idempotency:
  - POST không idempotent, tức là gửi nhiều POST giống nhau sẽ tạo ra nhiều kết quả khác nhau (ví dụ: tạo nhiều bản ghi).
- Quản lý ID:
  - Server sẽ tự sinh ID hoặc URI cho resource mới. Client không biết trước được ID.
- Mã trạng thái phản hồi:
  - Có thể trả về 201 Created (tạo thành công) hoặc 202 Accepted (đã chấp nhận xử lý nhưng chưa hoàn thành).
- Payload:
  - Dữ liệu POST thường là dữ liệu đầu vào để server xử lý và sinh ra một resource mới.
- Tình huống sử dụng:
  - Tạo mới user: POST /api/users
  - Gửi dữ liệu form: POST /api/contact
2. HTTP PUT
- Mục đích:
  - PUT dùng để cập nhật một resource hiện có hoặc tạo mới một resource tại URI cụ thể, thay thế toàn bộ nội dung resource bằng nội dung trong request.
- Idempotency:
  - PUT idempotent, gửi nhiều request giống nhau sẽ cho kết quả giống nhau.
- Quản lý ID:
  - Client xác định URI cho resource. Ví dụ: PUT /api/users/5 sẽ cập nhật hoặc tạo user có ID là 5.
- Mã trạng thái phản hồi:
  - 200 OK hoặc 204 No Content nếu cập nhật thành công
  - 201 Created nếu resource được tạo mới
- Payload:
  - Gửi toàn bộ dữ liệu của resource (không chỉ phần cần cập nhật).
- Tình huống sử dụng:
  - Cập nhật thông tin user: PUT /api/users/10
  - Tạo hoặc cập nhật file theo tên: PUT /api/files/report.pdf
