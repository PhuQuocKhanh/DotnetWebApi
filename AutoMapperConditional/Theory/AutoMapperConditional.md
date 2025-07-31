-- AutoMapper Conditional Mapping in ASP.NET Core Web API --
- Trong các dự án ASP.NET Core Web API hiện đại, AutoMapper là thư viện phổ biến được sử dụng để ánh xạ giữa các mô hình miền (domain models – thường dùng với EF Core) và các đối tượng truyền dữ liệu (DTOs).
- Thông thường, chúng ta cần kiểm soát khi nào và như thế nào từng thuộc tính được ánh xạ. 
- AutoMapper hỗ trợ hai cơ chế tích hợp sẵn để làm điều này: Pre-Condition và Condition. Mặc dù không có phương thức Post-Condition chuyên biệt, ta có thể giả lập nó thông qua phương thức AfterMap.

-- What are AutoMapper Pre-Condition, Condition, and (simulated) Post-Condition? --
1. AutoMapper Pre-Condition:
- Là điều kiện được kiểm tra trước khi AutoMapper đọc giá trị từ thuộc tính nguồn.
- Mục đích: Xác định xem thuộc tính đó có nên được ánh xạ hay không.
- Nếu điều kiện sai (false), AutoMapper không đọc giá trị nguồn và bỏ qua ánh xạ cho thuộc tính đó.
2. AutoMapper Condition:
- Là điều kiện được kiểm tra sau khi AutoMapper đã đọc giá trị từ thuộc tính nguồn, nhưng trước khi gán vào thuộc tính đích.

Mục đích: Kiểm tra xem giá trị đã đọc có thỏa điều kiện để gán vào thuộc tính đích hay không.
-- AfterMap Execution (Simulated Post-Condition): --
