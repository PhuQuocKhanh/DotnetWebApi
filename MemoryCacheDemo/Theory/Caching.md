-- What is Caching? --
- Caching là quá trình lưu trữ dữ liệu được truy cập thường xuyên vào một bộ nhớ tạm thời (gọi là cache) để các lần yêu cầu tiếp theo có thể được xử lý nhanh hơn. 
- Thay vì phải tính toán lại hoặc truy xuất dữ liệu từ cơ sở dữ liệu hay API bên ngoài mỗi lần, ứng dụng sẽ lấy dữ liệu từ cache (nếu có). 
- Điều này giúp giảm bớt các phép tính hoặc truy vấn không cần thiết, cải thiện hiệu suất và tốc độ phản hồi của ứng dụng, đồng thời giảm tải cho máy chủ cơ sở dữ liệu.

-- What is Caching in ASP.NET Core? --
- Trong ASP.NET Core, caching là việc lưu trữ dữ liệu được yêu cầu thường xuyên, chẳng hạn như kết quả truy vấn cơ sở dữ liệu, phản hồi từ API hoặc dữ liệu đã qua xử lý, vào bộ nhớ trong (in-memory) hoặc kho cache phân tán (distributed cache). 
- Các yêu cầu tiếp theo đối với cùng dữ liệu sẽ được xử lý nhanh hơn mà không cần truy vấn lại cơ sở dữ liệu hoặc thực hiện lại các tác vụ tốn kém. Nhờ đó, caching giúp tăng hiệu suất tổng thể của ứng dụng và cải thiện trải nghiệm người dùng.
- ASP.NET Core cung cấp các cơ chế caching tích hợp sẵn, giúp lập trình viên dễ dàng áp dụng vào dự án Web API. Một số chiến lược caching được hỗ trợ bao gồm:
  1. In-Memory Caching – Lưu dữ liệu trong bộ nhớ RAM của chính máy chủ đang chạy ứng dụng.
  2. Distributed Caching – Lưu cache trên hệ thống phân tán, cho phép chia sẻ dữ liệu cache giữa nhiều máy chủ, sử dụng các kho cache như Redis hoặc SQL Server.
  3. Response Caching – Lưu trữ toàn bộ phản hồi HTTP, đặc biệt hữu ích cho các API trả về dữ liệu ít thay đổi.

-- Why is Caching Important in ASP.NET Core Web API? --
- Caching đóng vai trò then chốt trong việc nâng cao hiệu năng và khả năng mở rộng (scalability) của ứng dụng ASP.NET Core Web API. 
- Nó giúp giảm thời gian xử lý mỗi request từ client, giảm tải cho các dịch vụ backend, đồng thời tối ưu việc sử dụng tài nguyên hệ thống.
- Nếu không có caching, mỗi request đều có thể kích hoạt các thao tác tốn thời gian như truy vấn cơ sở dữ liệu hoặc gọi các dịch vụ bên ngoài, khiến thời gian phản hồi chậm hơn và chi phí tăng lên.
- Nhờ caching, API có thể trả kết quả nhanh hơn, xử lý được nhiều request hơn và mang lại trải nghiệm ổn định cho người dùng.

- Lợi ích khi áp dụng Caching trong ASP.NET Core Web API:
  1. Hiệu năng & tốc độ:
    - Giảm số lần truy cập database hoặc dịch vụ bên ngoài, giúp API phản hồi nhanh hơn.
  2. Khả năng mở rộng: 
    - Tiết kiệm tài nguyên, cho phép server xử lý nhiều request đồng thời hơn mà không cần nâng cấp phần cứng.
  3. Giảm chi phí: 
    - Nếu database hoặc dịch vụ bên thứ ba tính phí theo số lượng request hoặc dung lượng sử dụng (đặc biệt trong môi trường cloud), caching giúp giảm đáng kể chi phí bằng cách giảm số lần gọi ra ngoài.
  4. Cải thiện trải nghiệm người dùng: 
    - Phản hồi nhanh hơn đồng nghĩa với trải nghiệm tốt hơn, giữ chân người dùng lâu hơn.

- Ví dụ:
- Ứng dụng có dữ liệu ít thay đổi như danh sách sản phẩm, quốc gia, tỉnh/thành phố hoặc cấu hình hệ thống.
- Nhiều người dùng có thể yêu cầu cùng một dữ liệu tại cùng thời điểm.
1. Không dùng Caching:
  - Mỗi request đi qua toàn bộ pipeline xử lý.
  - Ví dụ: 
    - mỗi lần client yêu cầu một tài nguyên, API sẽ truy vấn DB hoặc gọi external API, dẫn đến I/O lặp lại, tăng tải cho DB và làm giảm hiệu năng khi traffic cao.
  - Quy trình (Không caching):
    - Client gửi request đến Web API.
    - Web API nhận request và truy vấn DB hoặc dịch vụ bên ngoài.
    - DB xử lý và trả dữ liệu.
    - Web API trả dữ liệu cho client.
  → Mỗi request đều truy cập DB hoặc external service, ngay cả khi dữ liệu không thay đổi.
2. Có dùng Caching:
- Khi request đến, API kiểm tra dữ liệu trong cache.
- Cache hit → trả dữ liệu ngay lập tức, bỏ qua bước truy vấn DB.
- Cache miss → lấy dữ liệu từ DB/external API, lưu vào cache, sau đó trả về cho client.
- Những request sau sẽ lấy từ cache, giảm số lần truy cập backend, tăng tốc phản hồi.
- Quy trình (Có caching):
  - Client gửi request đến Web API.
  - Web API kiểm tra cache.
  - Nếu cache hit, trả dữ liệu ngay từ cache.
  - Nếu cache miss, truy vấn DB/dịch vụ bên ngoài → lưu vào cache → trả dữ liệu.
  - Những request tiếp theo sẽ lấy dữ liệu trực tiếp từ cache cho đến khi cache hết hạn hoặc bị xóa.
  → Với caching, Web API chỉ truy cập DB/dịch vụ ngoài ở lần request đầu tiên hoặc khi cache hết hạn/invalid.

-- What is a Cache Hit and a Cache Miss? --
1. Cache Hit: 
  - Khi một mục dữ liệu được yêu cầu đã tồn tại trong cache, đó được gọi là cache hit.
  - Ứng dụng có thể nhanh chóng lấy và trả về dữ liệu mà không cần xử lý bổ sung. 
  - Không cần gọi ra bên ngoài hoặc truy vấn cơ sở dữ liệu, nên thời gian phản hồi rất nhanh.
2. Cache Miss: 
  - Khi một mục dữ liệu được yêu cầu không có trong cache, đó được gọi là cache miss. 
  - Lúc này, ứng dụng phải truy xuất dữ liệu từ nguồn gốc ban đầu (ví dụ: cơ sở dữ liệu hoặc API bên ngoài) rồi lưu vào cache để dùng cho các lần truy cập sau.

-- What are the Different Types of Caching Available in ASP.NET Core? --
1. In-Memory Caching
  - Lưu trữ dữ liệu trực tiếp trong bộ nhớ RAM của web server đang chạy ứng dụng.
  - Dữ liệu nằm trong bộ nhớ của server, truy cập cực nhanh.
  - Cấu hình đơn giản thông qua interface IMemoryCache.
  - Hiệu suất đọc/ghi cao nhưng chỉ áp dụng được cho một instance của server (không chia sẻ cache giữa nhiều server).
  - Phù hợp khi dữ liệu không cần chia sẻ qua nhiều server.
2. Distributed Caching
  - Lưu trữ dữ liệu ở một cache server bên ngoài như Redis, NCache hoặc SQL Server, cho phép nhiều instance của ứng dụng cùng truy cập.
  - Dữ liệu nằm trong hệ thống cache bên ngoài, có thể truy cập bởi nhiều server.
  - Thích hợp cho môi trường load balancing hoặc ứng dụng chạy nhiều instance.
  - Triển khai qua interface IDistributedCache.
  - Đảm bảo tính nhất quán dữ liệu giữa các instance.
3. Response Caching
  - Lưu trữ toàn bộ response HTTP để tăng tốc phản hồi cho các request tương tự.
  - Cache toàn bộ nội dung phản hồi của endpoint.
  - Cho phép client hoặc proxy phục vụ lại response đã cache cho request sau.
  - Cấu hình thông qua attribute [ResponseCache] hoặc middleware.
  - Phù hợp khi response giống nhau cho nhiều request và không chứa dữ liệu nhạy cảm hay riêng tư của user.

-- Differences Between In-Memory Caching and Distributed Caching --
1. In-Memory Caching
  - Vị trí lưu trữ: Lưu trực tiếp trong bộ nhớ (RAM) của tiến trình ứng dụng.
  - Phạm vi: Chỉ khả dụng trong instance (phiên bản) ứng dụng đang chạy trên một server.
  - Hiệu năng: Rất nhanh vì dữ liệu nằm ngay trong bộ nhớ ứng dụng.
  - Chia sẻ dữ liệu: Không chia sẻ dữ liệu giữa nhiều instance.
  - Trường hợp sử dụng: Phù hợp cho ứng dụng nhỏ hoặc khi không cần chia sẻ dữ liệu giữa các server.
  - Độ phức tạp triển khai: Cài đặt đơn giản.
  - Hạn chế: Không phù hợp với môi trường multi-server hoặc load balancing vì mỗi server có cache riêng, dữ liệu không đồng bộ.
2. Distributed Caching
  - Vị trí lưu trữ: Lưu trên một hệ thống cache bên ngoài (external cache server).
  - Phạm vi: Chia sẻ dữ liệu giữa nhiều server hoặc nhiều instance ứng dụng thông qua các giải pháp cache phân tán như Redis, SQL Server Distributed Cache.
  - Hiệu năng: Chậm hơn in-memory một chút do có network overhead, nhưng vẫn nhanh hơn nhiều so với truy vấn trực tiếp vào nguồn dữ liệu gốc.
  - Chia sẻ dữ liệu: Dữ liệu được đồng bộ và chia sẻ giữa tất cả các instance ứng dụng.
  - sTrường hợp sử dụng: Thích hợp cho ứng dụng lớn, phân tán, cần đảm bảo tính nhất quán (consistency) và chia sẻ dữ liệu giữa nhiều node.
  - Độ phức tạp triển khai: Cần cấu hình và hạ tầng bổ sung.
  - Ưu điểm: Khả năng mở rộng cao, phù hợp với môi trường load-balanced hoặc cloud-native.

-- Distributed Caching Techniques Supported by ASP.NET Core Web API: --
1. Redis Cache
  - Mô tả: Redis là một in-memory data store mã nguồn mở, thường được dùng làm cache phân tán.
  - Đặc điểm: Hỗ trợ nhiều cấu trúc dữ liệu như string, hash, list, set, sorted set…, giúp linh hoạt trong nhiều tình huống.
  - Ưu điểm: Hiệu năng cao, dễ mở rộng (scalability), có các tính năng nâng cao như persistence (lưu trữ dữ liệu bền vững), replication (sao chép), clustering (cụm).
  - Trường hợp sử dụng: Lý tưởng cho ứng dụng cần read/write nhanh, lưu trữ session, hoặc xử lý real-time analytics.
2. SQL Server Cache
  - Mô tả: Sử dụng Microsoft SQL Server làm nơi lưu trữ cache.
  - Triển khai: Thông qua package Microsoft.Extensions.Caching.SqlServer, cung cấp implementation của IDistributedCache để lưu dữ liệu cache vào SQL Server.
  - Đối tượng sử dụng: Phù hợp với tổ chức đang dùng hệ sinh thái Microsoft, muốn tận dụng SQL Server thay vì triển khai công nghệ cache mới.
3. NCache
  - Mô tả: Giải pháp cache phân tán dành riêng cho .NET, hỗ trợ cả in-memory caching và kiến trúc phân tán.
  - Ưu điểm: Cho phép nhiều instance ứng dụng dùng chung một cache, hỗ trợ caching clusters, high availability, và nhiều mô hình cache (replicated, partitioned, client cache).
  - Trường hợp sử dụng: Phù hợp với doanh nghiệp dùng nhiều .NET, cần giải pháp cache phân tán đầy đủ tính năng, hỗ trợ mạnh, và khả năng clustering cao.
4. Memcached
  - Mô tả: Hệ thống cache in-memory mã nguồn mở, hiệu năng cao, giúp giảm tải cơ sở dữ liệu cho ứng dụng web.
  - Đặc điểm: Đơn giản, nhẹ, tập trung vào key-value storage, không hỗ trợ các cấu trúc dữ liệu nâng cao như Redis.
  - Trường hợp sử dụng: Phù hợp khi cần một key-value store đơn giản, nhanh, và dễ mở rộng, không yêu cầu tính năng cache phức tạp.

-- Key Differences Between Redis, SQL Server, NCache, and Memcached: --
1. Redis:
  - Cung cấp các cấu trúc dữ liệu phong phú, khả năng xử lý thông lượng cao và hỗ trợ clustering, phù hợp cho các kịch bản caching phức tạp và ứng dụng có lưu lượng lớn.
2. SQL Server Cache:
  - Là lựa chọn caching đơn giản cho các ứng dụng đã sử dụng SQL Server, nhưng hiệu năng và tính năng không mạnh bằng các hệ thống caching chuyên dụng.
3. NCache:
  - Cung cấp giải pháp distributed cache tối ưu cho .NET với các tính năng ở mức enterprise, tuy nhiên thường yêu cầu chi phí bản quyền.
4. Memcached:
  - Đơn giản và nhẹ hơn Redis hoặc NCache, phù hợp cho các nhu cầu caching cơ bản.