-- Difference Between In-Memory Caching and Distributed Caching in ASP.NET Core --
Trong ASP.NET Core, In-Memory Caching và Distributed Caching là hai chiến lược caching phổ biến, giúp cải thiện hiệu năng và khả năng mở rộng của ứng dụng bằng cách lưu trữ dữ liệu được truy cập thường xuyên trong cache thay vì phải lấy lại từ các nguồn chậm hơn (như database).

-- In-Memory Caching trong ASP.NET Core Web API -- 
- In-Memory Caching lưu dữ liệu cache ngay trong bộ nhớ (RAM) của web server nơi ứng dụng đang chạy. 
- Vì cache này nằm trong cùng process với ứng dụng, việc truy xuất dữ liệu cực kỳ nhanh.
1. Cấu hình và Thiết lập
  - Provider: ASP.NET Core cung cấp sẵn IMemoryCache, rất dễ dùng, không cần hạ tầng bên ngoài.
  - Đăng ký Service: Trong Program.cs, cấu hình bằng Services.AddMemoryCache(). Có thể thiết lập thêm size limit hoặc expiration policy.
  - Cách dùng: Inject IMemoryCache vào controller/service.
  - Thêm dữ liệu: Set, CreateEntry, hoặc GetOrCreate.
  - Lấy dữ liệu: Get hoặc GetOrCreate.
2. Các tùy chọn Cache Entry
  - Absolute Expiration: dữ liệu hết hạn tại một thời điểm cố định.
  - Sliding Expiration: tự động gia hạn nếu dữ liệu được truy cập lại trong khoảng thời gian cho phép.
  - Priority: độ ưu tiên giữ lại trong cache khi bị áp lực bộ nhớ (memory pressure).
3. Eviction và Cleanup
  - Memory Pressure: Khi server thiếu RAM, ASP.NET Core sẽ evict (xóa) entry theo mức ưu tiên.
  - Manual Eviction: Lập trình viên có thể xóa cache thủ công khi dữ liệu gốc thay đổi.
  - Ưu điểm
    - Tốc độ cao: Vì dữ liệu nằm trong RAM của server.
    - Dễ triển khai: Không cần thêm hạ tầng ngoài.
  - Nhược điểm
    - Không mở rộng (scalable): Cache chỉ tồn tại cục bộ trên một server, không dùng được trong cluster/web farm.
    - Dữ liệu dễ mất: Khi server restart hoặc crash, cache bị mất.
4. Use Case
  - Ứng dụng chạy 1 server hoặc số lượng server cố định.
  - Trường hợp yêu cầu latency cực thấp.
  - Dữ liệu cache nhỏ gọn, dễ quản lý và không cần đồng bộ nhiều node.

2. Distributed Caching trong ASP.NET Core Web API
- Distributed Caching lưu trữ dữ liệu cache bên ngoài ứng dụng, có thể dùng chung giữa nhiều server/instance. Đây là lựa chọn phổ biến trong môi trường load balancing, cloud hoặc microservices.
1. Cấu hình và Thiết lập
  - Provider: ASP.NET Core hỗ trợ nhiều provider như Redis, SQL Server, NCache (Redis thường được ưa chuộng nhờ tốc độ và tính năng).
  - Đăng ký Service: Trong Program.cs, cấu hình bằng Services.AddStackExchangeRedisCache() (nếu dùng Redis).
  - Khai báo Connection String: Trong appsettings.json hoặc code.
2. Cách dùng
  - Thêm dữ liệu: SetAsync hoặc Set (key-value).
  - Lấy dữ liệu: GetAsync hoặc Get.
  - Expiration/Eviction: Có thể cấu hình TTL (time-to-live), auto eviction để tránh cache bị quá tải.
4. Các lưu ý về Data Consistency
  - Invalidation: Cần xóa hoặc cập nhật cache khi dữ liệu trong DB thay đổi.
  - Consistency: Có thể dùng chiến lược như write-through hoặc write-behind để tránh dữ liệu cache và DB bị lệch.
  - Ưu điểm
    - Scalability: Cache nằm ngoài server app → dùng chung cho nhiều instance.
    - Availability: Cache không bị mất khi restart app.
    - Hiệu năng cao: Giảm số lần query DB.
  - Nhược điểm
    - Phức tạp hơn: Cần setup, maintain hệ thống cache riêng (Redis cluster, SQL cache...).
    - Chi phí: Tốn thêm hạ tầng (server hoặc dịch vụ cloud).
5. Use Case
  - Ứng dụng cloud-based hoặc microservices, chạy trên nhiều server.
  - Hệ thống cần tính nhất quán dữ liệu giữa nhiều instance.
  - Cần cache persistence (không mất dữ liệu khi app restart).
  - Khi ứng dụng phải scale out linh hoạt.

-- Chọn chiến lược nào?
- In-Memory Cache: phù hợp với ứng dụng nhỏ, chạy 1 server, cần tốc độ cực nhanh và setup đơn giản.
- Distributed Cache: phù hợp với ứng dụng lớn, nhiều instance, yêu cầu scalability và high availability.