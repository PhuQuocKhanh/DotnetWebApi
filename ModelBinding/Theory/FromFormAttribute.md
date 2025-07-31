-- Model Binding Using FromForm Attribute in ASP.NET Core Web API --
- Trong ASP.NET Core Web API, thuộc tính [FromForm] được sử dụng để ràng buộc (bind) dữ liệu từ các HTTP request (thường là POST) vào các tham số của phương thức action, khi dữ liệu được gửi dưới dạng form data với Content-Type là application/x-www-form-urlencoded hoặc multipart/form-data.
- Nói cách khác, [FromForm] chỉ định rằng tham số tương ứng sẽ được lấy từ dữ liệu form trong phần thân (body) của HTTP request. 
- Thuộc tính này thường được dùng trong các request POST, nơi dữ liệu được truyền lên thông qua form HTML hoặc khi upload file.

-- How Does FromForm Attribute Work in ASP.NET Core Web API? --
- Khi một phương thức action trong controller có tham số được đánh dấu bằng [FromForm], hệ thống Model Binding của ASP.NET Core sẽ cố gắng ánh xạ (map) các trường dữ liệu từ form gửi lên với các thuộc tính tương ứng trong đối tượng tham số, dựa trên tên của các trường form đó.
- Cách binding này rất phổ biến trong các tình huống như:
  - Nhận dữ liệu từ form HTML gửi lên backend API
  - Upload file (sử dụng IFormFile hoặc List<IFormFile>)
- Lưu ý: [FromForm] không bắt buộc chỉ dùng cho phương thức POST; nó có thể áp dụng cho bất kỳ HTTP method nào có body (ví dụ: PUT). Tuy nhiên, nó vẫn thường được dùng nhiều nhất với POST và PUT.

-- What are application/x-www-form-urlencoded and multipart/form-data Content Types? --
- Trong lập trình web, đặc biệt khi xử lý các biểu mẫu (form) HTML và HTTP request, hai loại Content-Type phổ biến là application/x-www-form-urlencoded và multipart/form-data.
1. 🔹 application/x-www-form-urlencoded
- Đây là định dạng mặc định khi submit form HTML.
- Dữ liệu được mã hóa theo dạng cặp key-value (tên trường và giá trị), tương tự như query string trong URL. Mỗi cặp được nối bằng dấu = và các cặp được phân tách bằng dấu &.
- Đặc điểm kỹ thuật:
  - Các ký tự không phải chữ cái hoặc số (ví dụ: khoảng trắng) sẽ được mã hóa theo chuẩn URL encoding (ví dụ: khoảng trắng thành %20).
  - Không phù hợp để gửi dữ liệu nhị phân (binary) hoặc file (ví dụ: PDF, hình ảnh), vì toàn bộ nội dung đều bị chuyển thành chuỗi ký tự, dẫn đến tăng kích thước dữ liệu.
2. 🔹 multipart/form-data
- Đây là định dạng được sử dụng khi form có chứa file upload hoặc khi cần gửi một lượng lớn dữ liệu.
- Dữ liệu được tách thành nhiều “phần” riêng biệt (part), mỗi part chứa thông tin về một trường form (hoặc một file), với phần header và phần nội dung tách biệt. 
- Các part này được phân cách bằng chuỗi boundary duy nhất.

-- Which Model Binder is used with FromForm Attribute in ASP.NET Core Web API? --
- Trong ASP.NET Core Web API, thuộc tính [FromForm] sử dụng hai loại Model Binder chính:
  1. FormFileModelBinder – dùng để bind dữ liệu là file.
  2. ComplexTypeModelBinder – dùng để bind các kiểu dữ liệu phức tạp (complex types) từ form data.
1. FormFileModelBinder
- FormFileModelBinder là trình liên kết được thiết kế riêng để xử lý việc upload file trong ASP.NET Core. 
- Khi action method nhận tham số kiểu IFormFile hoặc IFormFileCollection và có gắn [FromForm], ASP.NET Core sẽ sử dụng FormFileModelBinder để bind dữ liệu.
- Binder này sẽ trích xuất thông tin file từ request có Content-Type là multipart/form-data hoặc application/x-www-form-urlencoded, sau đó khởi tạo đối tượng IFormFile (hoặc IFormFileCollection) tương ứng, giúp lập trình viên có thể thao tác với file ngay trong phương thức xử lý.
2. ComplexTypeModelBinder
- Đối với các dữ liệu khác không phải là file – chẳng hạn như các trường văn bản (text) trong form – nếu được ánh xạ đến các thuộc tính trong một object phức tạp, ASP.NET Core sẽ sử dụng ComplexTypeModelBinder.
- Trình binder này sẽ duyệt qua từng thuộc tính trong model object, sau đó áp dụng binder phù hợp (theo attribute hoặc mặc định) cho từng thuộc tính. 
- Nó sử dụng Value Providers để đọc dữ liệu từ form và tự động chuyển đổi về đúng kiểu dữ liệu tương ứng với từng thuộc tính trong model.

-- When to Use FromForm Model Binding in ASP.NET Core Web API --
- Trong ASP.NET Core Web API, thuộc tính [FromForm] được sử dụng khi client gửi dữ liệu với Content-Type là application/x-www-form-urlencoded hoặc multipart/form-data — hai định dạng phổ biến khi gửi form.
- Dưới đây là các tình huống điển hình nên dùng [FromForm]:
1. Xử lý dữ liệu từ HTML Form
- Khi endpoint của bạn cần nhận dữ liệu được submit từ form HTML — đặc biệt trong các trường hợp:
  - Client là trình duyệt web.
  - Form được submit trực tiếp (không thông qua JavaScript/AJAX).
  - Dữ liệu được encode theo kiểu application/x-www-form-urlencoded.
2. Upload file kèm dữ liệu form
- Khi API cần xử lý upload file (ví dụ: ảnh, tài liệu...) đồng thời với dữ liệu text (như tên, mô tả...), bạn nên dùng [FromForm].
- Trường hợp này form thường được encode theo kiểu multipart/form-data.
- [FromForm] cho phép bind cả dữ liệu văn bản và file (qua IFormFile) từ cùng một request.

-- When Should We Use the Name Property of the FromQuery Attribute in ASP.NET Core Web API? --
- Thuộc tính Name của [FromQuery] được sử dụng trong trường hợp tên tham số trên query string không trùng khớp với tên tham số trong phương thức của controller.
- Ví dụ: nếu query string chứa tham số có tên là Dept, nhưng bạn muốn ánh xạ nó vào một tham số trong action có tên là DepartmentId, bạn có thể sử dụng cú pháp sau:
      [FromQuery(Name = "Dept")] int DepartmentId
- Cách dùng này sẽ ánh xạ giá trị của Dept từ query string sang tham số DepartmentId trong phương thức.
- Việc sử dụng thuộc tính Name rất hữu ích trong các trường hợp sau:
  - Khi tên tham số trên query string khác với tên tham số trong phương thức action.
  - Khi bạn muốn đặt tên tham số trong phương thức có ý nghĩa rõ ràng, có tính mô tả hơn so với tên thực tế trong URL (ví dụ: viết tắt hoặc tên kỹ thuật).

-- FromQuery with Complex Type in ASP.NET Core Web API: --