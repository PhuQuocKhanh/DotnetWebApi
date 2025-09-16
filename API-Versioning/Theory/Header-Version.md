-- Triển khai Header Versioning trong ASP.NET Core Web API -- 
  - Header Versioning là cách client chỉ định version thông qua HTTP Header (thường là api-version), thay vì để trong URL path hay query string.
  - Ví dụ client gửi request:
    - GET /api/products 
    - Header: api-version: 1.0
  - Server sẽ đọc header này và route đến controller/action tương ứng.
  - Ưu điểm:
    - URL sạch, không dính dữ liệu version.
    - Có thể thay đổi version mà không cần chỉnh URL.
  - Nhược điểm:
  - Khó test thủ công (vì phải thêm header thủ công, ví dụ khi test bằng browser).
  - Một số client có thể thấy phức tạp hơn khi phải set header.

-- Cơ chế hoạt động của Header Versioning -- 
- Client gửi request có kèm header api-version.
- Middleware API Versioning đọc giá trị này và route đến controller phù hợp:
  - api-version: 1.0 → gọi controller v1
  - api-version: 2.0 → gọi controller v2
- Nếu header thiếu, API sẽ:
- Dùng default version (nếu cấu hình)
- Hoặc reject request.

-- Khi nào nên sử dụng Header Versioning trong ASP.NET Core Web API? --
- Chúng ta nên sử dụng Header Versioning trong các tình huống sau:
1. Khi muốn giữ URL gọn gàng và ổn định, không lộ thông tin version trên route:
  - Với Header Versioning, tất cả các version của một endpoint đều dùng chung một URL, ví dụ:
    - /api/products
  - Thông tin version được truyền qua HTTP Header, chẳng hạn:
    - api-version: 2.0
- Điều này giúp URL rõ ràng, dễ ghi nhớ và hạn chế việc thay đổi ở phía client khi cập nhật version API.
2. Khi client có thể dễ dàng gửi custom HTTP headers:
  - Cách này phù hợp trong bối cảnh client là mobile app, backend service, hoặc các HTTP client nâng cao, vốn dễ dàng thêm header vào request.
  - Nó đặc biệt thích hợp cho các tình huống machine-to-machine, mobile, hoặc internal service integration.
3. Khi muốn tách biệt versioning khỏi resource identification:
  - URL chỉ dùng để định danh tài nguyên (resource), còn thông tin version được truyền qua header.
  - Điều này giúp API dễ dàng thay đổi hoặc mở rộng cơ chế versioning trong tương lai mà không phá vỡ các URL hiện có.

👉 Tóm lại: Header Versioning trong ASP.NET Core Web API mang lại URL sạch, tách biệt version với resource, và linh hoạt trong việc thương lượng version. Chúng ta nên dùng khi client dễ dàng set header và khi muốn tránh việc để lộ thông tin version trên URL hoặc query string.