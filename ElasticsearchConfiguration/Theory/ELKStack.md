-- Log Analyzing Tools in ASP.NET Core --
- ASP.NET Core tích hợp sẵn API Microsoft.Extensions.Logging để hỗ trợ ghi log cơ bản. 
- Mặc dù đủ cho các tình huống đơn giản, nhưng trong môi trường production — đặc biệt là hệ thống phân tán, lưu lượng lớn — thường cần các giải pháp phân tích log chuyên sâu hơn.
- Các công cụ này không chỉ thu thập log mà còn hỗ trợ tìm kiếm, lọc, và trực quan hóa dữ liệu log. 
- Các nền tảng phân tích và trực quan hóa log nâng cao thường được tích hợp để:
  - Chẩn đoán sự cố và giám sát hiệu năng.
  - Tìm kiếm và phân tích log nhanh chóng.
  - Trực quan hóa dữ liệu log để hiểu rõ hành vi ứng dụng.

1. Serilog:
- Serilog là thư viện logging dạng structured logging, có thể xuất log ở nhiều định dạng (JSON, text, v.v.).
- Nó hỗ trợ ghi log đến nhiều "sink" (đích) khác nhau như file, database, hoặc công cụ tìm kiếm như Elasticsearch, Seq, v.v. Với khả năng ghi log dạng có cấu trúc, Serilog giúp việc lọc và truy vấn log theo trường dữ liệu trở nên dễ dàng hơn, hỗ trợ hiệu quả cho việc xử lý sự cố. 
- Serilog được ASP.NET Core hỗ trợ tốt và dễ dàng tích hợp vào pipeline logging hiện tại.
2. Seq:
- Seq là một log server chuyên lưu trữ log sự kiện có cấu trúc, thường nhận dữ liệu từ Serilog. 
- Nó cung cấp giao diện web thân thiện, hỗ trợ query log, tạo cảnh báo, và phân tích mối liên hệ giữa các sự kiện. 
- Đối với môi trường local hoặc team nhỏ, Seq là một lựa chọn mạnh mẽ, dễ cấu hình.

3. ELK Stack (Elasticsearch, Logstash, Kibana):
- Bộ ELK kết hợp ba dự án mã nguồn mở để quản lý, xử lý và trực quan hóa khối lượng log lớn:
  - Elasticsearch: 
    - Công cụ tìm kiếm và phân tích phân tán, lưu trữ log dưới dạng tài liệu JSON, hỗ trợ truy xuất nhanh.
  - Logstash: 
    - Pipeline xử lý log, thu thập log từ nhiều nguồn, parse và enrich dữ liệu, sau đó gửi tới Elasticsearch.
  - Kibana: 
    - Công cụ trực quan hóa chạy trên Elasticsearch, cung cấp giao diện web để hiển thị và tương tác với dữ liệu log đã được index.

-- What is Elasticsearch? --
1. Elasticsearch là gì?
- Elasticsearch là một công cụ tìm kiếm và phân tích phân tán (distributed, RESTful search & analytics engine) được thiết kế để:
  - Lưu trữ và lập chỉ mục khối lượng dữ liệu lớn: Lưu trữ dữ liệu dưới dạng tài liệu JSON theo mô hình NoSQL, cho phép sử dụng schema linh hoạt.
  - Tìm kiếm gần như thời gian thực (near real-time): Đảm bảo phản hồi tìm kiếm nhanh chỉ trong vài giây, ngay cả khi dữ liệu có khối lượng lớn.
  - Tìm kiếm full-text và xử lý aggregations: Hỗ trợ truy vấn phức tạp và tổng hợp dữ liệu, phù hợp cho phân tích log và phân tích dữ liệu tổng quát.
  - Elasticsearch thường được dùng cho phân tích log, tìm kiếm full-text và phân tích dữ liệu thời gian thực. Đây là thành phần lưu trữ và tìm kiếm cốt lõi của ELK Stack.
2. Logstash là gì?
- Logstash là một pipeline xử lý dữ liệu mạnh mẽ, có khả năng:
  - Thu thập dữ liệu từ nhiều nguồn: Có thể thu thập log từ file, cơ sở dữ liệu hoặc nguồn mạng.
  - Biến đổi dữ liệu: Parse, filter và định dạng lại log dữ liệu.
  - Gửi dữ liệu đến Elasticsearch: Sau khi xử lý, dữ liệu đã được cấu trúc sẽ được gửi đến Elasticsearch để lưu trữ và lập chỉ mục.
  - Lớp biến đổi (transformation layer) này giúp chuẩn hóa định dạng log, đảm bảo Elasticsearch nhận được dữ liệu có cấu trúc tốt.
3. Kibana là gì?
- Kibana là công cụ trực quan hóa và khám phá dữ liệu được thiết kế để làm việc với Elasticsearch.
- Nó cung cấp:
  - Dashboard tương tác: Cho phép xây dựng dashboard tùy chỉnh để giám sát các chỉ số và xu hướng.
  - Khả năng tìm kiếm và lọc mạnh mẽ: Sử dụng Kibana Query Language (KQL) để tạo các truy vấn phức tạp, đào sâu vào log.
  - Phân tích thời gian thực: Tự động cập nhật biểu đồ khi dữ liệu mới được lập chỉ mục, phù hợp để giám sát hệ thống đang chạy.
  - Kibana chính là “giao diện” của ELK Stack, giúp bạn nhìn thấy, khám phá và hiểu dữ liệu trong Elasticsearch.