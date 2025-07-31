-- Exclude Properties from Model Binding in ASP.NET Core Web API: --
- Trong ASP.NET Core Web API, model binding (ràng buộc mô hình) là quá trình tự động ánh xạ dữ liệu đến từ các yêu cầu (request) như: dữ liệu từ route, query string, hoặc body của request ở định dạng JSON hoặc XML, vào các tham số của action method hoặc các thuộc tính trong model. 
- Cơ chế này giúp đơn giản hóa việc truyền dữ liệu giữa client và server.
- Tuy nhiên, trong một số trường hợp, chúng ta cần bao gồm hoặc loại trừ một số thuộc tính nhất định khỏi quá trình binding dựa trên các điều kiện cụ thể. 
- Việc kiểm soát những thuộc tính nào được bind là rất quan trọng vì các lý do sau:
  - Bảo mật: Ngăn chặn các cuộc tấn công over-posting, nơi mà client có thể gửi dữ liệu để cập nhật những thuộc tính mà họ không được phép (ví dụ: tự ý gán giá trị IsAdmin = true).
  - Tối ưu hóa độ rõ ràng và khả năng bảo trì: Đơn giản hóa model bằng cách chỉ tập trung vào các thuộc tính liên quan đến một thao tác cụ thể.
  - Hiệu năng: Tối ưu hóa dữ liệu được truyền và xử lý giữa client và server.
  - Thiết kế API hợp lý: Kiểm soát dữ liệu nào được trả về trong response của API, giữ lại một số trường nội bộ không public để phục vụ logic bên trong ứng dụng. Điều này giúp duy trì hợp đồng (contract) rõ ràng giữa client và server.

-- Why Exclude Properties from Model Binding in ASP.NET Core: --
- Để hiểu rõ lý do tại sao cần loại trừ thuộc tính khỏi quá trình model binding trong một ứng dụng ASP.NET Core Web API, hãy cùng xem một ví dụ cụ thể. 
- Bạn có thể bắt đầu bằng cách tạo một dự án ASP.NET Core Web API mới có tên là ModelBinding. Sau đó, tại thư mục gốc của dự án, tạo một thư mục con có tên là Models.

-- When to use Data Transfer Object: --
- Khi bạn muốn toàn quyền kiểm soát cách dữ liệu được định dạng hoặc chuyển đổi giữa client và server.
- Khi mô hình domain chứa các thuộc tính nhạy cảm hoặc không cần thiết, không nên expose qua API.
- Khi bạn dự đoán rằng các client khác nhau hoặc các endpoint khác nhau sẽ có yêu cầu dữ liệu khác nhau.
- Khi bạn muốn tránh việc gắn chặt giữa các entity của EF Core (hoặc domain model) với hợp đồng API (API contract).

-- DTOs vs. [JsonIgnore] in ASP.NET Core Web API: -- 
1. Về hiệu năng (Performance):
- [JsonIgnore] có hiệu suất tốt vì chỉ dựa trên hành vi tuần tự hóa mặc định.
- DTO có thể tạo ra một chút overhead do phải mapping thủ công hoặc thông qua các thư viện như AutoMapper, tuy nhiên độ chênh lệch là không đáng kể, trừ khi ứng dụng xử lý khối lượng request rất lớn.

2. Về khả năng bảo trì (Maintainability):
-. DTO: Có tính bảo trì cao hơn trong dài hạn vì nó định nghĩa rõ ràng cấu trúc dữ liệu cho từng thao tác. Giúp tách biệt rõ các tầng, hạn chế việc expose nhầm dữ liệu nội bộ.
- [JsonIgnore]: Thích hợp cho các thay đổi nhanh nhưng có thể trở nên khó quản lý nếu số lượng thuộc tính nhiều hoặc cùng một model được sử dụng theo nhiều cách khác nhau.