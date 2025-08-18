-- What is In-Memory Caching in ASP.NET Core Web API? --
- In-Memory Caching trong ASP.NET Core là cơ chế lưu trữ dữ liệu được truy cập thường xuyên ngay trong bộ nhớ RAM của máy chủ để cải thiện hiệu năng ứng dụng và giảm tải cho cơ sở dữ liệu.
- Khi client gửi một request yêu cầu lấy dữ liệu — đặc biệt là các dữ liệu ít thay đổi (ví dụ: dữ liệu danh mục như danh sách quốc gia, bang, thành phố) — việc cache dữ liệu đó trong bộ nhớ sẽ giúp ứng dụng phản hồi nhanh hơn ở các lần truy vấn sau, thay vì phải truy vấn cơ sở dữ liệu nhiều lần.

- Các điểm chính cần lưu ý khi dùng In-Memory Caching trong ASP.NET Core Web API:
1. Lưu trữ phía server: 
  - Dữ liệu được lưu ngay trong bộ nhớ RAM của web server nơi ứng dụng chạy.
2. Truy xuất nhanh: 
  - Lấy dữ liệu từ RAM nhanh hơn nhiều so với việc gọi API hoặc truy vấn database qua mạng.
3. Giới hạn bởi bộ nhớ server: 
  - Dữ liệu sẽ mất khi server khởi động lại hoặc application pool bị recycle. 
  - Ngoài ra, cache quá lớn có thể tiêu tốn đáng kể RAM của server.
4. Phù hợp cho dữ liệu nhỏ hoặc vừa: 
  - Lý tưởng khi dữ liệu cần cache không quá lớn và không cần chia sẻ giữa nhiều server.
5. Khi nên dùng: 
  - Trường hợp ứng dụng chạy trên một server duy nhất, truy cập dữ liệu theo hướng read-heavy và dữ liệu thay đổi ít.
6. Khi không nên dùng: 
  - Nếu hệ thống chạy load balancing hoặc cần chia sẻ cache giữa nhiều server, nên dùng Distributed Cache như Redis hoặc SQL Server Distributed Caching.

-- How To Implement In-Memory Caching Work in ASP.NET Core? --
- Trong ASP.NET Core, interface IMemoryCache được sử dụng để quản lý bộ nhớ đệm (cache) trong RAM của server.
- Để triển khai In-Memory Caching, chúng ta cần thực hiện các bước sau:
1. Inject IMemoryCache
  - Đăng ký (Add In-Memory Services) vào Dependency Injection Container trong Program.
  - Inject IMemoryCache vào service, controller, hoặc repository nơi cần sử dụng cache.
2. Sử dụng các phương thức của Cache
  - Interface IMemoryCache cung cấp các phương thức để lưu trữ, truy xuất và quản lý dữ liệu cache:
    - Set(key, value, options?): Lưu mới hoặc cập nhật dữ liệu vào cache.
    - Get(key): Lấy dữ liệu từ cache theo key.
    - TryGetValue(key, out value): Kiểm tra và lấy dữ liệu nếu tồn tại mà không gây exception khi không tìm thấy.
    - Remove(key): Xóa thủ công dữ liệu khỏi cache theo key.
3. Xác định chính sách hết hạn (Expiration Policies)
- ASP.NET Core hỗ trợ nhiều chiến lược cache khác nhau:
  1. Absolute Expiration: Dữ liệu tồn tại trong cache cho đến khi hết thời gian cố định.
  2. Sliding Expiration: Thời gian hết hạn sẽ được đặt lại mỗi khi dữ liệu được truy cập.
  3. Manual Eviction: Lập trình viên chủ động xóa cache khi dữ liệu thay đổi.
4. Đặt mức ưu tiên cho Cache (Cache Priority)
- Cache Priority xác định mức độ ưu tiên giữ lại item trong cache khi bộ nhớ server bị giới hạn.
- Khi ứng dụng thiếu RAM, runtime sẽ xóa các item có mức ưu tiên thấp trước.
- Các giá trị của CacheItemPriority gồm:
  1. Low
  2. Normal
  3. High
  4. NeverRemove

-- When Should We Use In-Memory Caching in ASP.NET Core Web API Real-Time Applications? --
- Trường hợp nên dùng:
1. Single-Server: Dễ triển khai và hiệu quả nếu ứng dụng chỉ chạy trên một máy chủ.
2. Read-Heavy Data: Phù hợp khi dữ liệu được truy cập thường xuyên nhưng ít thay đổi (ví dụ: dữ liệu danh mục, master data). Giúp tăng tốc độ phản hồi đáng kể.
3. Limited Data Size: Nếu dung lượng dữ liệu cần cache nhỏ hoặc vừa, lưu trực tiếp trong bộ nhớ RAM sẽ tiết kiệm chi phí và thời gian hơn so với việc truy vấn CSDL liên tục.
- Lưu ý quan trọng:
1. Nếu hệ thống scale-out (nhiều máy chủ), nên dùng distributed cache (ví dụ: Redis) để đồng bộ dữ liệu cache giữa các instance.
2. Không nên cache dữ liệu quá lớn trong RAM vì sẽ gây áp lực bộ nhớ hoặc lỗi OutOfMemory.
3. In-Memory Cache phù hợp cho: dữ liệu ít thay đổi, ứng dụng đơn máy chủ, hoặc các trường hợp chấp nhận được độ trễ trong đồng bộ dữ liệu.
4. Với ứng dụng thời gian thực, cần tính nhất quán dữ liệu cao hoặc dữ liệu thay đổi nhanh, nên dùng Distributed Caching.
