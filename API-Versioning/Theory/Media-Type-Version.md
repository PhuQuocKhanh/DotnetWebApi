-- Triển khai API Versioning trong ASP.NET Core Web API bằng Media Type Versioning -- 
- Media Type Versioning (còn gọi là Content Negotiation Versioning hoặc Accept Header Versioning) là một phương pháp versioning trong API, trong đó phiên bản API được chỉ định trong HTTP Accept Header, thường dưới dạng custom media type.
- Thay vì đưa version vào URL hoặc custom header, client chỉ định version như sau:
  - Accept: application/json;v=2.0
- Middleware API Versioning sẽ phân tích Accept header để xác định logic của phiên bản API cần sử dụng.
- Phương pháp này mang tính RESTful hơn vì nó tận dụng cơ chế content negotiation của HTTP, cho phép client yêu cầu một phiên bản API cụ thể thông qua media type. Tuy nhiên, nó cũng phức tạp hơn trong triển khai và dễ gây nhầm lẫn cho client nếu không cấu hình đúng header Accept.

-- Cách hoạt động của Media Type Versioning -- 
- Khi client gửi request với Accept header chứa media type kèm version, middleware API Versioning trong ASP.NET Core sẽ đọc header này và tách thông tin version:
  - Nếu header là application/json;v=1.0 → request được định tuyến đến controller/action của API version 1.
  - Nếu header là application/json;v=2.0 → request được định tuyến đến controller/action của API version 2.
  - Nếu không có version hoặc thiếu header → API có thể fallback về default version hoặc trả về lỗi, tùy cấu hình.
- Điểm quan trọng: URL không thay đổi (ví dụ: /api/products), mà cơ chế content negotiation quyết định version thông qua tham số v trong Accept header.

-- Khi nào nên dùng Media Type Versioning? -- 
- Media Type Versioning phù hợp trong các tình huống:
1. Muốn giữ URL sạch và ổn định, không thay đổi khi thêm version mới.
  - Ví dụ: /api/products luôn giữ nguyên, version được xác định qua Accept header (application/json;v=2.0).
2. Muốn tuân thủ chặt chẽ nguyên tắc RESTful và dùng HTTP Content Negotiation đúng chuẩn.
  - Không chỉ negotiate định dạng (JSON/XML) mà còn cả version.
3. Phù hợp khi API yêu cầu mức độ REST-compliance cao hoặc cần hỗ trợ nhiều loại representation.
  - Muốn kết hợp versioning và content negotiation trong một cơ chế thống nhất.
- Media Type Versioning thường được áp dụng cho các API chuyên nghiệp, tài liệu hóa đầy đủ, REST-compliant, đặc biệt khi client có khả năng hỗ trợ content negotiation.