-- Applying Model Binding Attributes to Model Properties in ASP.NET Core Web API: --
- Trong ASP.NET Core Web API, Model Binding cho phép framework trích xuất dữ liệu từ các HTTP request và ánh xạ chúng vào các tham số trong action method hoặc các thuộc tính trong model. 
- Các thuộc tính binding như [FromRoute], [FromQuery], [FromHeader] và [FromBody] được dùng để chỉ rõ dữ liệu cụ thể sẽ được lấy từ đâu trong request đến.
- Ví dụ, bạn có thể chỉ định rằng một tham số nên được đọc từ URL route, chuỗi truy vấn (query string), HTTP headers, hoặc nội dung của request body.

-- When to Use Binding Attributes --
- Việc hiểu rõ thời điểm sử dụng từng thuộc tính binding là rất quan trọng để thiết kế API hiệu quả:
1. [FromRoute]: 
  - Thuộc tính này được sử dụng khi dữ liệu (như ID hoặc các định danh khác) nằm trong đường dẫn URL.
  - Ví dụ: Với route /api/products/{id}, đoạn {id} trong URL có thể được binding trực tiếp vào một tham số hoặc thuộc tính thông qua [FromRoute].

2. [FromQuery]: 
  - Sử dụng cho các tham số được lấy từ chuỗi truy vấn (query string). 
  - Thường dùng cho các dữ liệu tùy chọn như phân trang hoặc lọc dữ liệu trong các HTTP GET request.
  - Ví dụ: /api/products?page=2&size=10

3. [FromHeader]: 
  - Dùng khi cần lấy dữ liệu từ HTTP headers. 
  - Thường được sử dụng cho các thông tin metadata hoặc dữ liệu điều khiển như phiên bản API, token xác thực, thiết lập ngôn ngữ, v.v.
  - Ví dụ: Authorization: Bearer <token>

4. [FromBody]: 
  - Thuộc tính [FromBody] dùng để binding các đối tượng phức tạp từ phần body của request.
  - Nó đặc biệt hữu ích trong các request POST, PUT, PATCH để tạo mới hoặc cập nhật tài nguyên, khi dữ liệu JSON hoặc XML được gửi lên từ client.
  - Ví dụ: Khi gửi JSON đại diện cho một sản phẩm mới, [FromBody] cho phép deserialization tự động JSON đó vào một đối tượng model.

-- Advantages of Using Binding Attributes: --
1. Ánh xạ dữ liệu một cách tường minh (Explicit Data Mapping):
- Mỗi phần dữ liệu được ánh xạ rõ ràng đến nguồn tương ứng (route, query, header, body), giúp tăng tính minh bạch và dễ hiểu về luồng dữ liệu đi vào hệ thống.

2. Linh hoạt trong nguồn dữ liệu (Flexible Data Sources):
- Trong cùng một phương thức API, ta có thể kết hợp nhiều nguồn dữ liệu như tham số trong route, chuỗi truy vấn (query string), header của request, và dữ liệu trong body — phù hợp với các yêu cầu xử lý phức tạp.

3. Tăng khả năng đọc và bảo trì mã nguồn (Enhanced Readability):
- Việc chú thích trực tiếp lên thuộc tính của model giúp mô hình trở nên tự mô tả (self-descriptive), từ đó cải thiện khả năng đọc hiểu và bảo trì mã trong dài hạn.

4. Hỗ trợ kiểm tra và bảo mật tốt hơn (Validation and Security):
- Khi biết rõ từng dữ liệu đến từ đâu, ta có thể dễ dàng xây dựng các rule validation cụ thể, đồng thời kiểm soát tốt hơn các yêu cầu về bảo mật (như kiểm tra giá trị từ header hoặc body).

- Tại sao nên sử dụng Model Binding Attributes trong ứng dụng thực tế?
  - Việc áp dụng các thuộc tính binding trong ASP.NET Core Web API là một kỹ thuật mạnh mẽ giúp kiểm soát chặt chẽ quá trình ánh xạ dữ liệu từ HTTP request vào tham số hoặc model của ứng dụng.
  - Trong các ứng dụng thực tế, chẳng hạn như nền tảng thương mại điện tử, các endpoint thường cần xử lý những request phức tạp chứa nhiều loại dữ liệu đến từ các nguồn khác nhau.
  - Sử dụng binding attributes sẽ giúp bạn:
    - Tổ chức và xử lý request một cách gọn gàng, có cấu trúc rõ ràng.
    - Đảm bảo API dễ mở rộng, dễ kiểm thử và dễ bảo trì.
    - Giảm lỗi tiềm ẩn do dữ liệu bị ánh xạ sai nguồn hoặc xử lý không chính xác.

