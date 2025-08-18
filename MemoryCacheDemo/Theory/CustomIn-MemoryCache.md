-- Custom In-Memory Cache in ASP.NET Core Web API? --
- In-memory caching trong ASP.NET Core là một cách nhanh chóng và hiệu quả để lưu trữ dữ liệu trực tiếp trong bộ nhớ của ứng dụng. Mặc định, IMemoryCache tích hợp sẵn trong ASP.NET Core cung cấp các thao tác cơ bản như set và get giá trị. 
- Tuy nhiên, nó không hỗ trợ việc liệt kê tất cả các key hoặc xóa có chọn lọc nhiều mục trong cache.
- Việc tùy chỉnh in-memory caching là cần thiết trong ASP.NET Core Web API để cải thiện khả năng quản lý cache và kiểm soát tốt hơn dữ liệu được lưu trữ.

-- When to Customize In-Memory Caching in ASP.NET Core Web API: --
- Trong các ứng dụng lớn hoặc phức tạp, việc tùy chỉnh đặc biệt hữu ích khi bạn cần:
1. Lấy tất cả Key và Value trong cache
  - Cung cấp khả năng theo dõi trạng thái cache theo thời gian thực.
  - Giúp đảm bảo dữ liệu được phục vụ là hợp lệ và phát hiện sớm các vấn đề.
  - Ví dụ: nếu một cặp key–value mong đợi không tồn tại, có thể cache chưa được set đúng hoặc nguồn dữ liệu không populate cache như dự kiến.
2. Lấy một cache entry cụ thể theo key
  - Phục vụ mục đích debug hoặc kiểm tra dữ liệu.
  - Ví dụ: nếu người dùng báo cáo thông tin trên website bị lỗi thời, bạn có thể nhanh chóng kiểm tra cache entry để xác định nguyên nhân.
3. Xóa toàn bộ cache
  - Áp dụng khi: cập nhật dữ liệu lớn, bảo trì định kỳ, hoặc gặp sự cố bất ngờ.
  - Đảm bảo toàn bộ dữ liệu được fetch lại từ nguồn gốc, tránh phục vụ dữ liệu lỗi thời.
4. Xóa một cache entry cụ thể theo key
  - Chỉ xóa dữ liệu không hợp lệ hoặc cần cập nhật.
  - Giúp giữ hiệu năng chung trong khi vẫn đảm bảo tính chính xác của dữ liệu riêng lẻ.
5. Cân nhắc bảo mật
  - Việc expose thông tin cache qua API endpoint tiềm ẩn rủi ro bảo mật.
  - Cần áp dụng authentication và authorization chặt chẽ để đảm bảo chỉ admin hoặc hệ thống được cấp quyền mới có thể đọc hoặc thao tác cache.

-- What are the limitations of In-memory Caching in ASP.NET Core? --
- In-memory caching trong ASP.NET Core cho phép lưu trữ dữ liệu nhanh chóng và hiệu quả ngay trong bộ nhớ của ứng dụng. 
- Tuy nhiên, nó có một số hạn chế khiến cho giải pháp này không phù hợp trong những hệ thống lớn hoặc phân tán. 
- Các hạn chế chính gồm:
1. Giới hạn trong phạm vi một server
  - Dữ liệu cache được lưu trong bộ nhớ RAM của từng server. 
  - Trong môi trường load balancing hoặc distributed system, mỗi instance sẽ có cache riêng biệt → dễ dẫn đến tình trạng dữ liệu không đồng nhất và nhiều cache miss do không chia sẻ cache giữa các node.
2. Hạn chế bởi dung lượng bộ nhớ
  - Dung lượng cache bị giới hạn bởi bộ nhớ vật lý (RAM) của server. 
  - Nếu cache quá nhiều hoặc đối tượng quá lớn, ứng dụng có thể tiêu thụ bộ nhớ cao hoặc gặp lỗi OutOfMemoryException. 
  - Cần định nghĩa chiến lược eviction (expiration, priority, v.v.) để giảm áp lực bộ nhớ.
3. Mất dữ liệu khi ứng dụng restart
  - Vì cache lưu trong bộ nhớ tạm (RAM), nên toàn bộ dữ liệu cache sẽ bị mất khi ứng dụng restart, recycle, hoặc crash.
  - Điều này gây bất lợi khi cache dữ liệu quan trọng hoặc tốn nhiều chi phí để tái tạo.
4. Không có sẵn cơ chế enumeration hoặc diagnostics
  - IMemoryCache không hỗ trợ sẵn việc liệt kê hoặc theo dõi các entry trong cache → khó quản lý dữ liệu đang được lưu. 
  - Nếu cần, phải tự cài đặt cơ chế key-tracking tùy chỉnh.

-- Do I need to implement Asynchronous In-Memory Caching in ASP.NET Core? --
- Thông thường là KHÔNG.
- Nguyên nhân:
  - Các thao tác trên bộ nhớ (RAM) cực kỳ nhanh, không có I/O bên ngoài.
  - Async/await chỉ hữu ích khi phải chờ database query, API call, disk I/O, network request,…
  - Với in-memory cache, async chỉ thêm overhead mà không mang lại lợi ích thực tế.
  - IMemoryCache đã được tối ưu sẵn cho concurrency và performance.S