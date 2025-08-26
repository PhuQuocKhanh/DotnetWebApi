-- Vòng đời Xử lý Request trong ASP.NET Core --
1. Client (HTTP Request)
- Đây là điểm khởi đầu, nơi client (như trình duyệt, ứng dụng di động, hoặc một dịch vụ khác) khởi tạo một HTTP request đến máy chủ. 
- Đó có thể là một trình duyệt yêu cầu một trang web, một lời gọi REST API, hoặc bất kỳ client nào dựa trên giao thức HTTP.

2. Middleware
- Request đầu tiên sẽ đi qua một chuỗi các thành phần middleware. 
- Middleware là một khái niệm cực kỳ quan trọng trong ASP.NET Core, tạo thành một "đường ống" (pipeline) mà mỗi thành phần có thể kiểm tra, sửa đổi, hoặc "ngắt mạch" (short-circuit) request/response. Dưới đây là một số tác vụ phổ biến của Middleware:
  - Định tuyến (Routing): Xác định endpoint (controller/action) sẽ xử lý request.
  - CORS (Cross-Origin Resource Sharing): Cho phép hoặc chặn các request từ các nguồn (origin) khác.
  - Xác thực (Authentication): Xác thực danh tính của người dùng.
  - Phục vụ tệp tĩnh (Static Files): Phục vụ các tài nguyên tĩnh như hình ảnh, CSS, JS, v.v., trực tiếp mà không cần đi qua controller.
  - Xử lý ngoại lệ (Exception Handling): Bắt các ngoại lệ chưa được xử lý và tạo ra các response lỗi được chuẩn hóa trên toàn cục.
  - Middleware thực thi tuần tự theo thứ tự chúng được đăng ký. Sau khi xử lý xong bởi middleware, request sẽ được chuyển đến cơ chế định tuyến.

3. Cơ chế Định tuyến (Routing Engine) (Chọn Controller & Action)
- Sau middleware, cơ chế định tuyến sẽ kiểm tra URL và phương thức HTTP của request đến để xác định controller và action nào sẽ xử lý request. Nó ánh xạ route đến với phương thức action tương ứng.

4. Bộ lọc Ủy quyền (Authorization Filter) (Kiểm tra Xác thực & Ủy quyền người dùng)
- Sau khi cơ chế định tuyến xác định được action mục tiêu, các Bộ lọc Ủy quyền (Authorization Filter) sẽ được thực thi. 
- Các bộ lọc này xác minh xem người dùng đã được xác thực (authenticated) và có quyền (authorized) truy cập tài nguyên được yêu cầu hay không. 
- Nếu người dùng không vượt qua được bước ủy quyền, request sẽ bị "ngắt mạch" và một response 401/403 sẽ được trả về. Ví dụ điển hình là các attribute [Authorize].

5. Bộ lọc Tài nguyên (Resource Filter) (Caching, Ngắt mạch)
- Các Bộ lọc Tài nguyên (Resource Filter) chạy sau bước ủy quyền. Chúng chịu trách nhiệm cho các tác vụ như caching và "ngắt mạch" pipeline trước khi action được thực thi. 
- Chúng có thể lưu trữ response vào cache hoặc dừng thực thi và trả về một kết quả đã được cache, giúp cải thiện hiệu suất.

6. Bộ lọc Hành động (Action Filter) (Xác thực đầu vào, Ghi log, Logic tiền xử lý)
- Trước khi phương thức action thực sự chạy, các Bộ lọc Hành động (Action Filter) sẽ được thực thi. Action filter cho phép chạy logic ngay trước và ngay sau khi phương thức action thực thi. Chúng thực hiện các tác vụ tiền xử lý như:
  - Xác thực (validate) các tham số đầu vào.
  - Ghi log (logging) thời điểm bắt đầu thực thi action.
  - Thao tác hoặc chuẩn bị dữ liệu cần thiết cho action.

7. Controller Action (Logic nghiệp vụ)
- Đây là trái tim của ứng dụng, nơi chứa logic nghiệp vụ của bạn. Action của controller nhận request, xử lý dữ liệu đầu vào, tương tác với các service hoặc cơ sở dữ liệu, và trả về một kết quả (thường là một model response hoặc view).

8. Bộ lọc Hành động (Action Filter) (Logic hậu xử lý, Ghi log, Kiểm toán)
- Sau khi phương thức action kết thúc, Action Filter chạy một lần nữa để thực hiện các tác vụ hậu xử lý. Các tác vụ phổ biến bao gồm:
  - Ghi log việc action đã hoàn thành.
  - Kiểm toán (auditing) các hành động của người dùng.
  - Sửa đổi kết quả của action trước khi nó được xử lý bởi các bộ lọc kết quả.

9. Bộ lọc Kết quả (Result Filter) (Sửa đổi Response, Nén, Thêm Headers)
- Các Bộ lọc Kết quả (Result Filter) thực thi sau khi action đã trả về một kết quả, nhưng trước khi kết quả đó được thực thi (để tạo ra response cuối cùng). 
- Chúng có thể sửa đổi response, nén dữ liệu, hoặc thêm các HTTP header. 
- Ví dụ, bạn có thể thêm một header tùy chỉnh để theo dõi hoặc áp dụng nén cho dữ liệu đầu ra.

10. Bộ lọc Tài nguyên (Resource Filter) (Cache kết quả đầu ra của Response)
- Bộ lọc Tài nguyên (Resource Filter) chạy một lần nữa sau bộ lọc kết quả, thường là để cache kết quả đầu ra của response cho các request tiếp theo. 
- Điều này giúp giảm tải cho máy chủ bằng cách phục vụ các response đã được cache.

11. Bộ lọc Ngoại lệ (Exception Filter) (Xử lý ngoại lệ)
Các Bộ lọc Ngoại lệ (Exception Filter) bắt các ngoại lệ chưa được xử lý xảy ra trong quá trình thực thi của pipeline (bao gồm middleware, thực thi action, các bộ lọc). Chúng cung cấp một cơ chế xử lý ngoại lệ tập trung và có thể tạo ra các response lỗi phù hợp hoặc thực hiện ghi log. Nếu có lỗi xảy ra, các bộ lọc này sẽ thực thi để tạo ra một response lỗi thân thiện.

12. Client (HTTP Response)
Sau khi tất cả các bộ lọc và quá trình xử lý hoàn tất, HTTP response sẽ được gửi trở lại cho client. Response này chứa dữ liệu hoặc thông tin lỗi mà client đã yêu cầu.