-- What is Model Binding in ASP.NET Core Web API? --
- Model Binding trong ASP.NET Core Web API là quá trình tự động ánh xạ (bind) dữ liệu từ HTTP request (như query string, route data, body, header, form data,...) vào các tham số của action method hoặc các đối tượng trong controller. 
- Cơ chế này giúp trích xuất và chuyển đổi dữ liệu từ yêu cầu HTTP thành các đối tượng có kiểu dữ liệu mạnh (strongly typed) trong .NET, giúp lập trình viên viết mã gọn gàng, dễ hiểu và dễ bảo trì hơn.
- Ví dụ: khi client gửi dữ liệu qua body (dạng JSON) hoặc qua query parameters, ASP.NET Core sẽ tự động ánh xạ các giá trị đó vào đối tượng tương ứng (ví dụ: đối tượng User) thông qua model binding.

- ASP.NET Core hỗ trợ model binding từ nhiều nguồn dữ liệu khác nhau:
  1. Query Strings: Tham số được truyền qua URL, ví dụ: ?name=John&age=30
  2. Route Data: Tham số được định nghĩa trong route (URL path), ví dụ: /users/5
  3. Form Data: Dữ liệu được gửi từ form HTML (thường dùng cho phương thức POST)
  4. Request Body: Dữ liệu trong body của HTTP request (thường ở dạng JSON, XML – áp dụng cho POST, PUT, PATCH)
  5. Headers: Dữ liệu được gửi thông qua HTTP headers (ví dụ: token, custom headers,...)

-- Why Is Model Binding Important in ASP.NET Core Web API? --
- Model Binding rất quan trọng vì nó giúp trừu tượng hóa (abstract) quá trình phân tích và chuyển đổi dữ liệu từ HTTP request thành các đối tượng trong .NET, từ đó giúp lập trình viên tập trung vào xử lý nghiệp vụ thay vì loay hoay với việc trích xuất dữ liệu thủ công.
- Các lý do chính khiến Model Binding trở nên quan trọng:

1. ✅ Đơn giản hóa xử lý dữ liệu
  - Không cần viết code thủ công để đọc dữ liệu từ HttpRequest. 
  - ASP.NET Core sẽ tự động ánh xạ (map) dữ liệu đến các tham số trong action method hoặc các thuộc tính của đối tượng.

2. ✅ Dễ bảo trì
  - Vì logic ánh xạ dữ liệu được xử lý tập trung và nhất quán, bạn không cần lặp lại các đoạn mã chuyển đổi dữ liệu trong nhiều controller, dẫn đến mã nguồn rõ ràng, dễ đọc và dễ bảo trì.

3. ✅ Hỗ trợ nhiều nguồn dữ liệu
  - Model Binding hỗ trợ dữ liệu đến từ: query string, route data, request body (JSON, XML, form-data), HTTP headers,... 
  - Điều này giúp API linh hoạt với nhiều kiểu định dạng request khác nhau.

4. ✅ Tích hợp xác thực dữ liệu (Validation)
  - ASP.NET Core tích hợp validation cùng với model binding.
  - Bạn có thể sử dụng các Data Annotations (như [Required], [Range],...) hoặc custom validation. 
  - Nếu dữ liệu không hợp lệ, framework sẽ tự động báo lỗi và bạn có thể xử lý hợp lý.

-- What are the Model Binding Techniques used in ASP.NET Core Web API? --
1. FromQuery
  - Dùng để bind dữ liệu từ query string trên URL.
  - Ví dụ: GET /api/users?name=John → [FromQuery] string name
2. FromRoute
  - Dùng để bind dữ liệu từ route parameters (biến trong URL).
  - Ví dụ: GET /api/users/5 → [FromRoute] int id

3. FromBody
  - Dùng để bind dữ liệu từ request body (thường là JSON hoặc XML).
  - Áp dụng khi client gửi object dạng phức tạp (complex type).
  - Ví dụ: [HttpPost] public IActionResult Create([FromBody] UserDto user)

4. FromForm
  - Dùng để bind dữ liệu từ form data (content-type: application/x-www-form-urlencoded hoặc multipart/form-data).
  - Phù hợp cho các request kiểu POST từ HTML form.

5. FromHeader
  - Dùng để bind dữ liệu từ HTTP headers.
  - Thường dùng để lấy metadata như version, token, user-agent,...
  - Ví dụ: [FromHeader(Name = "X-Correlation-Id")] string correlationId

📌 Model Binding mặc định (không dùng attribute)
- Nếu không chỉ định attribute, ASP.NET Core sẽ sử dụng quy tắc binding mặc định như sau:
  - Với kiểu đơn giản (int, string, bool,...) → bind từ query string, route, hoặc form.
  - Với kiểu phức tạp (complex type) → bind từ body (mặc định là JSON).

-- How Do We Handle Model Binding Errors in ASP.NET Core Web API? --
- Model Binding không phải lúc nào cũng hoàn hảo. Đôi khi dữ liệu được gửi từ client có thể bị thiếu, sai định dạng, hoặc không thỏa mãn các ràng buộc đã khai báo. Rất may, ASP.NET Core cung cấp nhiều cơ chế để phát hiện và xử lý các lỗi binding.

📌 Lỗi Model Binding xảy ra khi nào?
  - Lỗi xảy ra khi dữ liệu từ HTTP request không thể bind thành công vào model do:
  - Sai kiểu dữ liệu (type mismatch)
  - Thiếu trường bắt buộc (missing required fields)
  - Dữ liệu không đúng định dạng (invalid format)

✅ Cách xử lý lỗi binding hiệu quả
1. Sử dụng Validation Attributes (Data Annotations)
  - Bạn có thể áp dụng các thuộc tính validation (như [Required], [StringLength], [Range],...) lên model để định nghĩa các quy tắc kiểm tra đầu vào.
2. Kiểm tra ModelState
  - Sau khi model binding hoàn tất, bạn có thể kiểm tra ModelState.IsValid trong action method để biết liệu có lỗi binding hoặc validation nào không.

-- How Does Model Binding Work in ASP.NET Core Web API? --
1. Tiếp nhận Request
- Khi một HTTP request được gửi đến ứng dụng ASP.NET Core:
  - Hệ thống routing sẽ xác định controller và action method phù hợp để xử lý request dựa trên cấu hình route ([Route], UseEndpoints, v.v.).
2. Xác định tham số (Parameter Discovery)
- Framework sẽ duyệt qua các tham số trong action method để xác định:
  - Kiểu dữ liệu cần binding.
  - Nguồn dữ liệu tương ứng thông qua các attribute như [FromBody], [FromQuery], [FromRoute], v.v.
3. Kích hoạt Model Binder Provider
- ASP.NET Core sử dụng danh sách các Model Binder Provider đã được đăng ký để tìm Model Binder phù hợp cho từng tham số.
- Binder đầu tiên có khả năng xử lý kiểu dữ liệu sẽ được sử dụng.
4. Trích xuất dữ liệu thông qua Value Providers
- Model Binder sử dụng các Value Provider để truy xuất dữ liệu thô từ các nguồn cụ thể:
5. Chuyển đổi dữ liệu và gán giá trị
- Model Binder sẽ cố gắng chuyển đổi dữ liệu thô thành kiểu dữ liệu .NET phù hợp sử dụng TypeConverter hoặc InputFormatter.
- Nếu thành công → giá trị được gán vào tham số hoặc property tương ứng.
- Nếu thất bại → lỗi binding sẽ được ghi vào ModelState.
6. Thực hiện kiểm tra hợp lệ (Validation)
- Sau khi binding xong, ASP.NET Core sẽ tự động chạy quá trình xác thực dữ liệu:
  - Dựa trên các Data Annotations như [Required], [Range], [StringLength],...
  - Các lỗi validation sẽ được ghi lại trong ModelState.
7. Thực thi Action Method
- Nếu binding và validation đều thành công → Action method được thực thi.
- Nếu controller có [ApiController] và binding thất bại:
  - ASP.NET Core tự động trả về HTTP 400 Bad Request
  - Kèm theo thông tin chi tiết lỗi trong response body (ModelState)