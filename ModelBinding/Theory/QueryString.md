-- What is a Query String in ASP.NET Core Web API? --
- Query string (chuỗi truy vấn) là phần nằm sau dấu hỏi chấm (?) trong URL, dùng để truyền dữ liệu từ client đến server thông qua các yêu cầu HTTP. 
- Nó bao gồm các cặp khóa-giá trị (key-value), trong đó mỗi khóa và giá trị được nối với nhau bằng dấu bằng (=), và các cặp được phân cách bởi dấu &. 
- Ví dụ: trong URL
https://example.com/api/values?Department=IT&Gender=Male
thì query string là Department=IT&Gender=Male, trong đó Department và Gender là các khóa, còn IT và Male là các giá trị tương ứng.
- Query string thường được sử dụng trong các yêu cầu HTTP GET để truyền tham số mà không làm thay đổi trạng thái trên server.
- Việc dữ liệu được nhúng trong URL giúp dễ dàng chia sẻ và đánh dấu (bookmark) các yêu cầu.

-- Model Binding using FromQuery Attribute in ASP.NET Core Web API --
- Thuộc tính [FromQuery] được sử dụng để chỉ định rằng giá trị của một tham số trong phương thức controller sẽ được lấy từ query string của HTTP request. 
- Điều này đặc biệt hữu ích khi bạn muốn ràng buộc rõ ràng một tham số với dữ liệu từ URL.

-- How Does the FromQuery Attribute Work in ASP.NET Core Web API? -- 
- Khi một yêu cầu HTTP được gửi đến Web API, [FromQuery] báo cho hệ thống model binding rằng giá trị của tham số đó phải được lấy từ query string. Ví dụ, với URL sau:
https://example.com/api/values?name=John&age=30
- Tóm lại, [FromQuery] cho phép bạn lấy dữ liệu trực tiếp từ chuỗi truy vấn trong URL một cách rõ ràng và có kiểm soát. Đây là cách phổ biến khi xử lý các yêu cầu GET có tham số đầu vào.

-- When Should We Use the FromQuery Attribute in ASP.NET Core Web API? --
- Thuộc tính [FromQuery] được sử dụng khi bạn cần lấy dữ liệu đơn giản được truyền trong query string của một HTTP GET request. Đây là lựa chọn lý tưởng trong các tình huống như:
  1. Lọc dữ liệu (filtering),
  2. Phân trang (paging),
  3. Hoặc khi client cần truyền thêm các tham số phụ hoặc tùy chọn đến server mà không cần thay đổi đường dẫn URL (route).
Bạn nên sử dụng [FromQuery] trong các trường hợp sau:
- Khi bạn muốn ghi đè nguồn binding mặc định (ví dụ: khi tên tham số có thể bị nhầm lẫn với tham số trong route hoặc dữ liệu form).
- Khi một tham số có thể được truyền từ nhiều nguồn khác nhau (route, body, form, query…), và bạn muốn chỉ định rõ là lấy từ query string.
- Khi bạn muốn hỗ trợ tham số truy vấn tùy chọn (optional query parameters) – tức là không bắt buộc phải có trong request.
