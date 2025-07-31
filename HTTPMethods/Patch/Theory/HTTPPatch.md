-- HTTP PATCH Method in ASP.NET Core Web API --
- Phương thức HTTP PATCH trong ASP.NET Core Web API được sử dụng để cập nhật một phần tài nguyên hiện có. 
- Khác với phương thức PUT, vốn thay thế toàn bộ tài nguyên, PATCH cho phép thay đổi từng phần nên hiệu quả hơn trong những trường hợp chỉ cần cập nhật một vài trường dữ liệu.

-- Characteristics of HTTP PATCH Method --
1. Cập nhật từng phần (Partial Update):
- PATCH được dùng để thay đổi một phần của tài nguyên. 
- Ví dụ, nếu bạn có một đối tượng User gồm các thuộc tính như Name, Email, Address, thì một yêu cầu PATCH chỉ cần gửi thay đổi cho trường Email mà không ảnh hưởng tới các trường còn lại.

2. Tính chất Idempotent (Idempotency):
- Về lý thuyết, PATCH là idempotent – tức là gọi cùng một PATCH request nhiều lần sẽ không gây thay đổi thêm ngoài lần đầu.
- Tuy nhiên, trong thực tế điều này phụ thuộc vào cách hiện thực trên phía server. 
- Một số thao tác PATCH có thể không idempotent nếu mỗi lần áp dụng lại gây ra kết quả khác nhau.

-- How Do We Implement HTTP PATCH Method in ASP.NET Core Web API? --
- Để triển khai phương thức PATCH, ta thực hiện theo các bước sau:
1. Định nghĩa Model (Resource Model):
  - Tạo class đại diện cho cấu trúc dữ liệu của tài nguyên bạn muốn thao tác (ví dụ: User, Product, ...).
2. Tạo DTO (Data Transfer Object):
  - Là phiên bản đơn giản hơn hoặc rút gọn của model, dùng để nhận dữ liệu từ client. 
  - Với PATCH, DTO thường chỉ chứa các trường có thể cập nhật.
3. Tạo Controller:
  - Viết controller kế thừa từ ControllerBase. 
  - Đây là nơi chứa các endpoint xử lý yêu cầu PATCH.
4. Định nghĩa Action xử lý PATCH:
  - Tạo action method trong controller với attribute [HttpPatch].
  - Có thể khai báo route nếu cần.
5. Sử dụng JSON Patch:
  - ASP.NET Core hỗ trợ JSON Patch (chuẩn [RFC 6902]) – định dạng JSON mô tả tập hợp các thao tác cần thực hiện lên một tài nguyên. 
  - JSON Patch thường được gửi với content-type application/json-patch+json.
6. Hiện thực logic cập nhật:
  - Trong action PATCH, sử dụng JsonPatchDocument<T> để áp dụng các thay đổi vào đối tượng gốc (được lấy từ database). 
  - Sau đó lưu thay đổi vào DB.
7. Trả về phản hồi (Response):
  - Tuỳ vào kết quả xử lý, bạn có thể trả về:
    - 200 OK: Cập nhật thành công
    - 404 Not Found: Không tìm thấy tài nguyên
    - 400 Bad Request: Dữ liệu yêu cầu không hợp lệ

-- When Should We Use HTTP PATCH Method in ASP.NET Core Web API? --
- Phương thức HTTP PATCH trong ASP.NET Core Web API được sử dụng để cập nhật một phần dữ liệu của tài nguyên. Dưới đây là các trường hợp nên sử dụng PATCH:
1. Cập nhật một phần tài nguyên (Partial Update):
  - Sử dụng PATCH khi bạn chỉ cần cập nhật một vài trường (field) của tài nguyên mà không cần gửi toàn bộ dữ liệu. 
  - Điều này đặc biệt hiệu quả trong các hệ thống có tài nguyên phức tạp, nhiều thuộc tính, nhưng chỉ thay đổi một phần nhỏ dữ liệu.

2. Tiết kiệm băng thông và giảm tải hệ thống: 
  - PATCH giúp giảm lưu lượng mạng và tài nguyên xử lý bằng cách chỉ gửi phần thay đổi thay vì toàn bộ đối tượng. 
  - Điều này có lợi trong môi trường có giới hạn về băng thông hoặc hiệu suất.

3. Hỗ trợ tính chất idempotent (có thể thiết kế): 
  - Mặc dù PATCH không bắt buộc phải idempotent, nhưng bạn có thể thiết kế endpoint PATCH theo hướng idempotent để đảm bảo an toàn khi retry request (gửi lại yêu cầu nhiều lần mà không gây tác dụng phụ).

4. Tuân thủ nguyên tắc REST: 
  - Trong RESTful API, việc sử dụng đúng HTTP method theo mục đích là điều quan trọng. 
  - PATCH được thiết kế để xử lý các thao tác cập nhật từng phần, vì vậy nó là lựa chọn phù hợp nhất trong trường hợp này.

-- So sánh HTTP PUT và HTTP PATCH trong ASP.NET Core Web API -- 
- Cả hai phương thức PUT và PATCH đều được dùng để cập nhật tài nguyên trong ASP.NET Core Web API, nhưng khác nhau về cách cập nhật:
1. HTTP PUT
- Tính idempotent: 
  - PUT là phương thức idempotent, tức là gọi nhiều lần với cùng dữ liệu sẽ cho kết quả như nhau. 
  - Điều này đảm bảo tính ổn định khi gửi lại yêu cầu.
- Cập nhật toàn bộ (Full Update): 
  - PUT thay thế toàn bộ tài nguyên bằng dữ liệu mới được gửi lên. 
  - Tức là payload phải bao gồm đầy đủ tất cả các thuộc tính. 
  - Nếu một thuộc tính không được gửi lên, nó có thể bị đặt về mặc định hoặc bị xóa.
- Khi nào dùng: 
  - Dùng PUT khi bạn muốn cập nhật toàn bộ đối tượng và có đầy đủ thông tin cần thiết cho tài nguyên.
2. HTTP PATCH
- Cập nhật từng phần (Partial Update): 
  - PATCH chỉ yêu cầu gửi những trường dữ liệu cần thay đổi, không cần toàn bộ đối tượng.
- Không đảm bảo idempotent: 
  - PATCH có thể không idempotent, tức là gửi cùng một request nhiều lần có thể cho kết quả khác nhau.
  - Tuy nhiên, bạn hoàn toàn có thể thiết kế để đảm bảo tính idempotent nếu cần.
- Định dạng dữ liệu: 
  - Payload của PATCH thường dùng định dạng đặc biệt như JSON Patch (RFC 6902) hoặc JSON Merge Patch, giúp mô tả rõ thao tác cần thực hiện: thêm (add), cập nhật (replace), xoá (remove), v.v.
- Khi nào dùng:
  - Dùng PATCH khi bạn chỉ cần cập nhật một phần nhỏ thông tin của tài nguyên và muốn tối ưu hiệu năng, giảm lượng dữ liệu truyền tải.

- Cách chọn giữa PUT và PATCH trong ASP.NET Core Web API
- Dùng PUT nếu:
  - Cần cập nhật toàn bộ tài nguyên.
  - Ứng dụng/Client luôn có thể cung cấp đầy đủ thông tin của đối tượng.
- Dùng PATCH nếu:
  - Chỉ cần cập nhật một số trường nhỏ trong tài nguyên.
  - Muốn giảm băng thông, tăng hiệu suất và giữ cho payload nhỏ gọn.

