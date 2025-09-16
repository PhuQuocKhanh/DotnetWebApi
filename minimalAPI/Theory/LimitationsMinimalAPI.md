-- Hạn chế của Minimal API trong ASP.NET Core -- 
- Minimal API trong ASP.NET Core mang lại cách tiếp cận gọn nhẹ để phát triển nhanh các HTTP API quy mô nhỏ. 
- Tuy nhiên, bên cạnh sự đơn giản và hiệu năng cao, chúng vẫn tồn tại một số hạn chế ảnh hưởng đến khả năng phù hợp cho những loại ứng dụng nhất định:

1. Hạn chế khi xử lý ứng dụng phức tạp
  - Minimal API phù hợp nhất cho ứng dụng nhẹ, microservices hoặc API không cần middleware phức tạp, routing nâng cao hay business logic rườm rà. 
  - Với các yêu cầu phức tạp như validation chi tiết, dependency injection nâng cao hoặc policy authorization phức tạp, việc sử dụng Minimal API sẽ khó duy trì clean code và tổ chức hợp lý khi ứng dụng mở rộng.

2. Thiếu các tính năng nâng cao của MVC
  - Minimal API không hỗ trợ sẵn nhiều tính năng nâng cao có trong MVC controller, ví dụ: attribute routing, action filter (authorization, resource, exception), hay cơ chế model validation tích hợp sẵn. 
  - Nếu dự án yêu cầu các tính năng này (như kiểm soát truy cập qua attribute-based authorization hoặc validation phức tạp), lập trình viên phải tự triển khai thủ công, dẫn đến code cồng kềnh và khó bảo trì.

3. Ít convention, nhiều cấu hình thủ công hơn
  - Trong ASP.NET Core MVC, nhiều quy ước được áp dụng sẵn như model binding, auto routing, hay response formatting. 
  - Minimal API không có nhiều convention như vậy, buộc developer phải tự cấu hình routing, response handling và validation. 
  - Điều này đòi hỏi hiểu biết sâu hơn về ASP.NET Core pipeline, làm tăng effort ban đầu và overhead trong quá trình phát triển.

4. Thách thức trong tổ chức và bảo trì code
  - Minimal API thường định nghĩa endpoint trực tiếp trong file Program.cs. 
  - Khi số lượng endpoint tăng, code dễ bị lộn xộn và khó mở rộng. 
  - Nếu không tách logic ra các class/handler riêng, dự án có thể trở nên khó maintain, debug và scale.

-- So sánh Minimal API và Controller-Based API -- 
1. Minimal API
  - Nhấn mạnh sự đơn giản, cú pháp ngắn gọn. Route và handler thường được khai báo trực tiếp trong Program.cs.
  - Cấu hình ít, setup nhanh, phù hợp cho API nhỏ hoặc microservice.
  - Hiệu năng tốt nhờ pipeline tinh gọn, overhead thấp.
  - Hạn chế ở chỗ không hỗ trợ đầy đủ attribute routing, filter (authorization, action, resource, exception), và model validation tích hợp.
2. Controller-Based API
  - Dựa trên mô hình MVC, các action được tổ chức thành controller giúp dự án rõ ràng, maintainable, dễ scale.
  - Hỗ trợ attribute-based routing cho phép định nghĩa route tường minh.
  - Có sẵn hệ thống filter, authorization, model binding, validation → đơn giản hóa kịch bản phức tạp.
  - Thích hợp với ứng dụng cần middleware phức tạp, business logic nhiều tầng, validation chuyên sâu và quản lý endpoint chi tiết.

- Khuyến nghị
  - Minimal API: tối ưu cho API nhẹ, hiệu năng cao, cần phát triển nhanh và đơn giản.
  - Controller-Based API: phù hợp cho ứng dụng phức tạp, cần cấu trúc rõ ràng, dễ bảo trì, nhiều tính năng nâng cao.