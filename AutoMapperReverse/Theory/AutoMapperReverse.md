-- AutoMapper Reverse Mapping in ASP.NET Core Web API --
- AutoMapper là một thư viện ánh xạ đối tượng sang đối tượng (object-to-object mapping) phổ biến trong .NET, giúp đơn giản hóa quá trình ánh xạ các thuộc tính giữa các đối tượng khác nhau, chẳng hạn như giữa các mô hình thực thể (entity models) và các đối tượng truyền dữ liệu (DTO – Data Transfer Object). 
- AutoMapper giúp giảm thiểu đáng kể lượng mã lặp lại do việc gán thủ công các thuộc tính.
- Reverse mapping (ánh xạ ngược) là một tính năng trong AutoMapper cho phép ánh xạ hai chiều giữa các đối tượng mà không cần phải cấu hình riêng biệt cho từng chiều. 
- Tính năng này đặc biệt hữu ích trong các ứng dụng ASP.NET Core Web API, nơi chúng ta thường cần:
  - Ánh xạ các thực thể từ Entity Framework Core sang DTO khi trả dữ liệu về phía client.
  - Khi tạo mới hoặc cập nhật dữ liệu, ánh xạ ngược từ DTO nhận từ client về lại thực thể EF Core.

-- When Should We Use AutoMapper Reverse Mapping in ASP.NET Core Web API? --
- Luồng dữ liệu 2 chiều: Khi bạn cần ánh xạ dữ liệu cả chiều từ Entity → DTO và ngược lại.
- Giảm cấu hình trùng lặp: ReverseMap giúp bạn không cần viết hai CreateMap riêng biệt cho từng chiều.
- Dễ bảo trì: Khi thay đổi cấu trúc model, bạn chỉ cần sửa tại một chỗ, tránh thiếu sót khi mapping hai chiều.

➡️ Tóm lại: ReverseMap giúp bạn định nghĩa ánh xạ 2 chiều giữa Entity và DTO một cách gọn gàng, dễ bảo trì và nhất quán.