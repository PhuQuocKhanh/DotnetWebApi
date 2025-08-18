-- What is Distributed Caching? --
- Distributed Caching (bộ nhớ đệm phân tán) là cơ chế lưu trữ dữ liệu cache trên một hệ thống bên ngoài mà nhiều server ứng dụng có thể truy cập đồng thời.
Ví dụ: Redis, NCache,...
- Khác với In-Memory Cache (chỉ lưu trong bộ nhớ của một instance ứng dụng), Distributed Cache thường được cài đặt trên một cache server riêng. Tất cả các ứng dụng kết nối đến server cache này để thực hiện Get/Set dữ liệu cache.
- Thiết kế này giúp ứng dụng scalable và reliable hơn, vì dữ liệu cache vẫn tồn tại ngay cả khi một ứng dụng bị restart.
-- When Should We Use a Distributed Cache Instead of In-Memory Cache? --
- In-Memory Cache rất nhanh, nhưng chỉ tồn tại trong phạm vi một instance ứng dụng. Nếu ứng dụng chạy trên nhiều server/container thì mỗi instance sẽ có cache riêng, dẫn đến:
  - Trùng lặp dữ liệu cache
  - Tốn nhiều bộ nhớ
  - Dữ liệu cache không đồng bộ
- Trong các trường hợp này, Distributed Cache là lựa chọn tốt hơn vì cung cấp một nguồn dữ liệu cache tập trung, tất cả server đều có thể dùng chung. 
- Ngoài ra, Distributed Cache còn bền vững hơn khi một server bị down.

-- When Should We Use a Distributed Cache Instead of In-Memory Cache? -- 
1.  Multiple Server Instances
  - Ứng dụng chạy trên nhiều server (behind load balancer).
    → In-Memory Cache sẽ không đồng bộ giữa các server, còn Distributed Cache thì tất cả server đều dùng chung.
2. Memory Constraints
  - Dữ liệu cache lớn → không nên giữ riêng trong từng server.
    → Chuyển cache ra ngoài giúp tiết kiệm RAM trên app server.
3. Persistence Across Restarts
  - In-Memory Cache sẽ mất khi app restart.
    → Distributed Cache có thể giữ cache (persistence) và hỗ trợ replication để đảm bảo dữ liệu không bị mất.
4. High Scalability
  - Ứng dụng scale-out nhiều instance 
    → cần một cache duy nhất để tránh dữ liệu trùng lặp/không đồng bộ.
5. High Availability
  - Kết hợp với load balancing 
    → đảm bảo request nào cũng có thể cache hit, không phụ thuộc server nào xử lý.

-- Kiến trúc Distributed Caching (Bộ nhớ đệm phân tán) --
- Trước hết, chúng ta cần hiểu về kiến trúc của Distributed Caching, sau đó sẽ tìm hiểu cách triển khai Distributed Caching với Redis trong ứng dụng ASP.NET Core Web API.
- Trong kiến trúc Distributed Caching, nhiều thành phần phối hợp với nhau để quản lý, truy xuất và lưu trữ dữ liệu cache một cách hiệu quả. 
- Các thành phần này bao gồm Users, Load Balancer, Application Servers, Distributed Cache, và Database.

-- Components of the Distributed Caching: --
- Users (Người dùng): 
  - Là client hoặc consumer gửi request đến ứng dụng. 
  - Người dùng có thể là end-user truy cập website hoặc API client gọi endpoint của ứng dụng.
- Load Balancer (Bộ cân bằng tải): 
  - Phân phối request đến nhiều instance của Application Server. 
  - Điều này giúp tránh tình trạng quá tải một server, đảm bảo tính sẵn sàng cao và cân bằng khối lượng công việc.
- Application Servers (Máy chủ ứng dụng): 
  - Xử lý request, đồng thời đọc/ghi dữ liệu trong Distributed Cache để giảm số lần truy vấn trực tiếp vào Database. 
  - Hệ thống có nhiều App Server cho phép scale theo chiều ngang (horizontal scaling) để xử lý nhiều request đồng thời khi lưu lượng tăng.
- Distributed Cache (Bộ nhớ đệm phân tán): 
  - Là nơi lưu trữ dữ liệu cache tập trung, cho phép tất cả App Server cùng truy cập. 
  - Nó tăng hiệu năng bằng cách cung cấp dữ liệu nhanh chóng cho những request thường xuyên lặp lại. 
  - Khác với In-Memory Cache cục bộ, Distributed Cache đảm bảo tính nhất quán (consistency) và tránh dư thừa dữ liệu.
- Database (CSDL): 
  - Nguồn dữ liệu gốc và lưu trữ lâu dài. Khi có Cache Miss (không tìm thấy dữ liệu trong cache), App Server sẽ truy vấn Database, lấy dữ liệu và (thường) lưu nó vào cache để phục vụ các request sau.

-- Cách Distributed Caching hoạt động: -- 
- User Request: 
  - Người dùng gửi request đến ứng dụng (qua Load Balancer).
- Load Balancer: 
  - Điều hướng request đến một trong các Application Server.
- Cache Check: 
  - App Server kiểm tra dữ liệu trong Distributed Cache.
- Cache Hit: 
  - Nếu dữ liệu có trong cache → trả kết quả nhanh chóng từ cache.
- Cache Miss: 
  - Nếu dữ liệu không có trong cache → App Server truy vấn Database để lấy dữ liệu.
- Store Data in Cache: 
  - Sau khi lấy dữ liệu từ Database, App Server sẽ lưu lại vào Distributed Cache để tái sử dụng cho những request tiếp theo.
- Return Data: 
  - Dữ liệu (từ cache hoặc từ DB) được trả về cho người dùng. 

-- Redis là gì? -- 
- Redis (viết tắt của Remote Dictionary Server) là một hệ thống lưu trữ dữ liệu in-memory mã nguồn mở, thường được sử dụng như giải pháp caching, cơ sở dữ liệu, và message broker. 
- Redis lưu trữ dữ liệu theo mô hình key–value và hỗ trợ nhiều cấu trúc dữ liệu nâng cao (như list, set, sorted set, hash).

- Đặc điểm nổi bật của Redis:
  1. Hiệu năng cao: 
    - Dữ liệu được lưu trực tiếp trên bộ nhớ RAM, giúp thao tác đọc/ghi nhanh hơn nhiều so với lưu trữ trên ổ đĩa.
  2. Hỗ trợ đa dạng cấu trúc dữ liệu: 
    - Redis không chỉ làm việc với string mà còn hỗ trợ các cấu trúc như hash, list, set, sorted set… giúp mở rộng phạm vi ứng dụng trong nhiều trường hợp thực tế.
- Trong các ứng dụng ASP.NET Core Web API, Redis thường được dùng như một giải pháp distributed caching. 
- Redis giúp lưu trữ dữ liệu được truy cập thường xuyên ngay trên bộ nhớ đệm, nhờ đó những request sau có thể lấy dữ liệu trực tiếp mà không cần truy vấn xuống database chậm hơn ở tầng dưới.

-- What are the Benefits of using Redis Distributed Cache in ASP.NET Core Web API? --
1. Truy xuất dữ liệu tốc độ cao
  - Redis là một in-memory data store, vì vậy tốc độ đọc/ghi dữ liệu nhanh hơn nhiều so với việc truy vấn trực tiếp từ cơ sở dữ liệu truyền thống. 
  - Điều này giúp API phản hồi nhanh hơn, nâng cao trải nghiệm người dùng.
2. Khả năng mở rộng và cân bằng tải (Scalability & Load Balancing)
  - Khi dữ liệu được lưu trong Redis tập trung, nhiều application server có thể chia sẻ chung cache. 
  - Nhờ đó, hệ thống dễ dàng scale-out bằng cách thêm server mới mà không cần lo mỗi server phải duy trì cache riêng. Cách tiếp cận tập trung này giúp đảm bảo tính nhất quán dữ liệu và hiệu quả trong môi trường load-balanced.
3. Giảm tải cho Database  
  - Việc phục vụ dữ liệu từ cache Redis giúp giảm tần suất truy vấn cơ sở dữ liệu. 
  - Nhờ vậy, database ít bị quá tải hơn, tiết kiệm tài nguyên, và tăng độ ổn định cũng như hiệu năng chung của hệ thống.
4. Khả năng chịu lỗi (Fault Tolerance)
  - Không giống như in-memory cache nội bộ từng server, Redis cache không gắn liền với vòng đời của một application instance. 
  - Nếu một server gặp sự cố hoặc bị down, dữ liệu trong Redis vẫn còn nguyên vẹn và sẵn sàng phục vụ cho các server khác. 
  - Điều này nâng cao khả năng chịu lỗi và tính sẵn sàng (high availability) của hệ thống.
5. Tùy biến cache linh hoạt
  - Redis hỗ trợ nhiều cấu trúc dữ liệu nâng cao (strings, hashes, lists, sets, …) và cho phép kiểm soát chi tiết cách dữ liệu được lưu trữ, hết hạn (expiration), hoặc làm mới (sliding expiration). 
  - Điều này giúp lập trình viên dễ dàng triển khai các chiến lược caching phù hợp với từng use case cụ thể, ví dụ preload dữ liệu được truy cập nhiều.
6. Tối ưu hiệu năng trong môi trường high-traffic
  - Với khả năng xử lý lượng lớn các thao tác đọc/ghi cùng lúc với độ trễ rất thấp, Redis đặc biệt phù hợp cho các API có lưu lượng truy cập cao.
  - Nhờ đó, hệ thống duy trì được hiệu năng ổn định ngay cả khi chịu tải lớn.