-- FromHeader Attribute in ASP.NET Core Web API --
- Thuộc tính FromHeader trong ASP.NET Core Web API được sử dụng để ánh xạ (bind) một tham số trong phương thức action với giá trị của một HTTP header cụ thể trong request. 
- Thuộc tính này báo cho framework biết rằng giá trị của tham số cần được lấy từ header tương ứng trong request đến. 
- Việc này rất hữu ích khi cần đọc các thông tin metadata, version, token xác thực, hoặc các dữ liệu bổ sung được truyền qua HTTP headers.

-- How Does the FromHeader Attribute Work in ASP.NET Core Web API? --
- Khi một request được gửi đến endpoint của API, hệ thống model binding của ASP.NET Core sẽ phân tích request và ánh xạ dữ liệu vào các tham số của phương thức action. 
- Nếu một tham số được đánh dấu bằng thuộc tính FromHeader, model binder sẽ tìm kiếm một header trong HTTP request có tên trùng với tên của tham số (hoặc tên được chỉ định thông qua thuộc tính Name của FromHeader).
- Nếu header đó tồn tại, giá trị của nó sẽ được binding vào tham số tương ứng.

-- When Should We Use the FromHeader Attribute in ASP.NET Core Web API? --
- Chúng ta nên sử dụng thuộc tính FromHeader khi cần trích xuất dữ liệu từ header của HTTP request và sử dụng dữ liệu đó trong các phương thức action. Thuộc tính này thường được áp dụng trong các tình huống sau:
1. Mã thông báo xác thực (Authentication Tokens):
  - Trong các hệ thống xác thực dựa trên token (ví dụ: JWT – JSON Web Token), client thường gửi token thông qua phần header của request. 
  - Khi đó, FromHeader cho phép bạn trích xuất trực tiếp giá trị token vào tham số của action method.
2. Thông tin phiên bản API (Versioning Information):
  - Khi triển khai versioning cho API thông qua header (thay vì qua URL hoặc query string), bạn có thể sử dụng FromHeader để lấy thông tin version mà client yêu cầu, từ đó định tuyến tới logic xử lý phù hợp với version đó.
3. Metadata tùy chỉnh (Custom Metadata):
  - Trong một số trường hợp, ứng dụng cần truyền thêm metadata để phục vụ xử lý request, mà không phù hợp để đặt trong body hoặc URL. 
  - Ví dụ: mã định danh request (request ID) dùng để theo dõi, cờ (flag) đặc thù của client, hoặc bật/tắt tính năng. 
  - FromHeader giúp bạn truy xuất nhanh chóng các giá trị này từ phần header.

-- What is FromBody Attribute in ASP.NET Core Web API? --
- Thuộc tính FromBody trong ASP.NET Core Web API được sử dụng để chỉ định rằng một tham số trong phương thức action sẽ được binding (ràng buộc dữ liệu) từ body của HTTP request.
- Khi một tham số được đánh dấu với [FromBody], ASP.NET Core sẽ cố gắng deserialize (giải tuần tự hóa) nội dung của request body (thường ở định dạng JSON) thành kiểu dữ liệu của tham số đó.
- Thuộc tính này thường được sử dụng cho các kiểu dữ liệu phức tạp hoặc đối tượng dữ liệu được gửi từ client lên dưới dạng JSON hoặc XML trong phần body của HTTP request.

-- How Does the FromBody Attribute Work in ASP.NET Core Web API? --
- Khi thuộc tính [FromBody] được áp dụng cho một tham số, ASP.NET Core sẽ sử dụng các input formatter đã được cấu hình sẵn (dựa trên Content-Type của request 
- ví dụ: application/json, application/xml) để deserialize (giải tuần tự) phần body của request thành một đối tượng C#.
- Quá trình này được thực hiện bởi hệ thống model binding của ASP.NET Core. Hệ thống này sẽ tự động chọn formatter phù hợp để phân tích (parse) nội dung của request body và ánh xạ (map) dữ liệu đó vào tham số được định nghĩa trong action method, dựa trên định dạng của nội dung gửi lên.

-- When Should We Use FromBody Attribute in ASP.NET Core Web API? --
- Bạn nên dùng [FromBody] khi cần ánh xạ một đối tượng phức tạp (class có nhiều thuộc tính) từ phần thân (body) của HTTP request.
- Đây là cách thường dùng trong các yêu cầu POST, PUT, hoặc PATCH, nơi client gửi dữ liệu dưới dạng JSON và bạn cần deserialize nó thành một model.
- Trường hợp sử dụng [FromBody]:
  1. Khi phương thức nhận vào một đối tượng phức tạp từ request body.
  2. Khi xử lý các phương thức POST hoặc PUT mà client gửi dữ liệu dạng JSON trong phần body.
  3. Khi dữ liệu không thể truyền qua URL (ví dụ: object phức tạp hoặc dữ liệu lớn).
