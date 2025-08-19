-- Clusters, Cache Server, Nodes, Data, and Partitions: -- 
- Việc hiểu rõ các khái niệm Cluster, Cache Server, Node, Data và Partition trong NCache sẽ giúp chúng ta nắm được cách hệ thống hoạt động:
1. Cluster:
  - Một Cluster trong NCache là tập hợp các Cache Server (mỗi Cache Server bao gồm nhiều Node, và mỗi Node sẽ lưu trữ một partition dữ liệu cache) được cấu hình để hoạt động như một đơn vị logic duy nhất.
  - Việc clustering giúp cải thiện hiệu năng, khả năng mở rộng và tính sẵn sàng (high availability) của cache.
  - Nếu một node bị lỗi, các node khác có thể tiếp quản khối lượng công việc mà không làm mất dữ liệu hoặc gián đoạn dịch vụ.
2. Cache Server:
  - Trong NCache, Cache Server là một instance server chịu trách nhiệm lưu trữ và quản lý một phần cache phân tán.
  - Mỗi Cache Server đảm bảo tính sẵn sàng của cache, duy trì tính toàn vẹn dữ liệu và cung cấp khả năng truy cập dữ liệu nhanh chóng.
3. Node:
  - Đây là các instance riêng lẻ bên trong môi trường Cache Server. Mỗi instance cache được gọi là một Node.
  - Nếu một Cache Server chạy nhiều instance NCache, thì có thể coi Cache Server đó bao gồm nhiều Node.
  - Mỗi Node lưu trữ một phần của distributed cache. 
  - Các Node giao tiếp với nhau để đồng bộ dữ liệu, cân bằng tải (load balancing) và cung cấp khả năng failover.
4. Data:
  - Trong NCache, Data là dữ liệu ứng dụng được lưu trữ trong cache. 
  - Dữ liệu này có thể là key-value pair đơn giản hoặc các object phức tạp.
  - Việc lưu dữ liệu trong cache giúp giảm nhu cầu truy xuất từ database (chậm hơn vì dựa trên disk), từ đó cải thiện hiệu năng ứng dụng.
5. Partition:
  - Partitioning trong NCache là cơ chế phân phối dữ liệu trên nhiều Node trong cluster. 
  - Mỗi Partition thường giữ một tập con dữ liệu cache duy nhất, giúp đảm bảo dữ liệu được phân tán đồng đều trên toàn cluster.
  - Nếu một Node gặp sự cố, chỉ những partition trên Node đó bị ảnh hưởng. 
  - Dữ liệu có thể được khôi phục từ bản sao (replica) nếu đã được cấu hình replication.

-- Types of NCache Clusters in Distributed Caching --
- NCache cung cấp nhiều cấu hình cluster khác nhau, được thiết kế cho từng nhu cầu và kịch bản cụ thể:
1. Mirrored Cache (Bộ nhớ đệm dạng nhân bản hai nút): 
  - Gồm hai node, trong đó một node hoạt động chính (active node) và một node dự phòng (passive standby) phản chiếu dữ liệu của node chính. Khi node chính gặp sự cố, node dự phòng sẽ tự động tiếp quản, giảm thiểu downtime và mất dữ liệu.
2. Replicated Cache (Bộ nhớ đệm dạng sao chép toàn bộ): 
  - Mỗi node trong cluster lưu toàn bộ dữ liệu cache. 
  - Điều này đảm bảo tính sẵn sàng cao và khôi phục nhanh khi một node gặp lỗi, nhưng lại tiêu tốn nhiều tài nguyên.
3. Partitioned Cache (Bộ nhớ đệm dạng phân mảnh dữ liệu):
  - Dữ liệu được chia nhỏ (partition) và phân tán trên các node, mỗi node lưu một phần dữ liệu. 
  - Cách này tăng khả năng mở rộng và phân tải (load balancing).
4. Partition-Replica Cache (Kết hợp phân mảnh và sao chép): 
  - Dữ liệu vừa được phân mảnh trên nhiều node, vừa có bản sao trên các node khác. 
  - Cấu hình này cân bằng giữa hiệu năng (scalability, load distribution) và độ sẵn sàng dữ liệu (high availability).

-- How Does the Mirrored Cache Work in Distributed Caching?-- 
- Mirrored Cache bao gồm hai node: một node Primary (chính) và một node Secondary (backup). Hai node này kết hợp để tạo thành cặp dự phòng (high-availability pair).
- Quy trình hoạt động:
1. Thao tác trên Primary Node: 
  - Các thao tác ghi (write) như thêm, cập nhật hoặc xóa dữ liệu được thực thi trên node chính.
2. Asynchronous Mirroring (Nhân bản bất đồng bộ): 
  - Sau khi ghi thành công trên node chính, thay đổi sẽ được gửi sang node dự phòng. 
  - Việc nhân bản này là asynchronous — tức node chính không chờ phản hồi từ node dự phòng mà tiếp tục xử lý request khác, giúp tăng hiệu năng.
3. Đồng bộ dữ liệu (Data Consistency): 
  - Node dự phòng nhận dữ liệu và cập nhật để phản ánh thay đổi từ node chính. 
  - Thường quá trình này diễn ra rất nhanh, nhưng vẫn có một khoảng thời gian ngắn dữ liệu giữa hai node chưa hoàn toàn đồng bộ. 
  - Điều này tạo ra trade-off giữa hiệu năng và an toàn dữ liệu: nếu node chính “crash” ngay tại thời điểm đó, dữ liệu chưa kịp sync có thể bị mất.
4. Failover Scenario (Chuyển đổi khi lỗi): 
  - Khi node chính gặp sự cố, node dự phòng sẽ tiếp quản gần như toàn bộ dữ liệu (trừ những thay đổi cuối chưa sync kịp) và trở thành node chính để duy trì dịch vụ liên tục.
5. Mục đích: 
  - Mirrored Cache đảm bảo High Availability. Khi node chính hỏng, node dự phòng sẵn sàng takeover với bản sao dữ liệu, giúp giảm thiểu downtime và hạn chế mất mát dữ liệu.

-- Mirrored Cache Use When: --
  - Ưu tiên High Availability: Phù hợp khi yêu cầu hệ thống luôn sẵn sàng và có node backup takeover ngay khi lỗi.
  - Triển khai đơn giản: Chỉ gồm 2 node, dễ cài đặt và bảo trì.
  - Hạn chế mất dữ liệu: Dữ liệu được mirror ngay lập tức, thích hợp cho các hệ thống không chấp nhận mất mát dữ liệu, dù chỉ một phần nhỏ.
- Tình huống áp dụng thực tế:
  - Hệ thống giao dịch tài chính quan trọng, nơi mỗi transaction đều bắt buộc phải an toàn.
  - Các hệ thống quy mô nhỏ, nơi không cần cluster phức tạp nhưng vẫn muốn đảm bảo tính sẵn sàng cao.

-- How Does the Replicated Cache Work in Distributed Caching? --
- Replicated Cache được thiết kế để mỗi node trong cluster đều duy trì một bản sao giống hệt dữ liệu cache. 
- Cách này giúp tăng khả năng sẵn sàng dữ liệu (availability) và cải thiện hiệu suất đọc (read performance) trên toàn hệ thống.

1. Ghi dữ liệu trên bất kỳ node nào: 
  - Khác với Mirrored Cache (chỉ có primary node xử lý ghi), trong Replicated Cache, thao tác ghi (add, update, delete) có thể thực hiện tại bất kỳ node nào. 
  - Node nhận request ghi sẽ đóng vai trò coordinator để đảm bảo đồng bộ (replication) đến các node khác.
2. Truyền dữ liệu (Data Propagation): 
  - Sau khi ghi thành công trên node coordinator, thay đổi đó sẽ được propagate đồng bộ (synchronous replication) đến tất cả các node còn lại trong cluster. 
  - Mỗi node phải xác nhận (acknowledge) update trước khi transaction được đánh dấu hoàn tất.
3. Xử lý khi node bị lỗi (Failover): 
  - Vì tất cả node đều có bản sao đầy đủ dữ liệu, nên hệ thống có khả năng chống chịu lỗi cao (fault tolerance). 
  - Một node có thể gặp sự cố mà không gây mất dữ liệu, các node khác vẫn có thể phục vụ dataset đầy đủ.
4. Mục đích: Ưu điểm chính của Replicated Cache là:
  - High availability (tính sẵn sàng cao).
  - Fast read access (tốc độ đọc nhanh).
- Vì tất cả các node đều có dữ liệu, nên request đọc có thể được xử lý bởi bất kỳ node nào, giúp phân tải (load balancing) và giảm latency.
- Khi nào nên dùng Replicated Cache?
  - Ứng dụng đọc nhiều (read-heavy): Khi hệ thống có lượng lớn truy vấn đọc và cần phân tán trên nhiều node để tăng throughput.
  - Yêu cầu tính nhất quán dữ liệu (data consistency): Khi mọi node phải có cùng view dữ liệu tại mọi thời điểm.
  - Cần khả năng chịu lỗi (fault tolerance): Hệ thống vẫn hoạt động ổn định dù một hoặc nhiều node bị hỏng. 

-- How Does the Partitioned Cache Work in Distributed Caching? --
- Partitioned Cache là một cấu hình phổ biến trong hệ thống distributed caching, được thiết kế để tăng khả năng scalability và load balancing bằng cách chia dữ liệu cache thành nhiều partition và phân tán cho nhiều node trong cluster. 
- Mỗi node chỉ giữ một phần dữ liệu thay vì toàn bộ, cho phép hệ thống scale out khi thêm node mới. 
- Cơ chế này đặc biệt hữu ích khi dung lượng cache lớn, giúp tối ư u bộ nhớ và đảm bảo tốc độ truy xuất nhanh.
1. Cơ chế phân phối dữ liệu
- Partitioning Logic: 
  - Dữ liệu được chia thành các partition dựa trên consistent hashing hoặc thuật toán tương tự.
  - Mỗi key được ánh xạ (map) tới một partition nhất định.
- Node Assignments: 
  - Mỗi partition được gán cho một node trong cluster. Node đó sẽ chịu trách nhiệm lưu trữ và quản lý subset dữ liệu cache tương ứng.
- Dynamic Rebalancing: 
  - Khi có node mới join hoặc node bị remove, cache cluster sẽ tự động rebalance dữ liệu giữa các node để đảm bảo phân phối đồng đều. 
  - Quá trình này bao gồm việc migrate data từ node cũ sang node mới.
2. Xử lý Read/Write
- Read Operation: 
  - Khi client request một key, hệ thống dùng hash function để xác định partition chứa key đó, sau đó điều hướng request đến node tương ứng. Nếu key tồn tại thì trả về dữ liệu, nếu không thì cache miss.
  - Write Operation: Với write cũng tương tự. Dữ liệu mới được hash → xác định partition → ghi vào node quản lý partition đó. Cách này đảm bảo consistency cho mỗi key vì chỉ có một node chịu trách nhiệm.
3. Scalability
- Horizontal Scaling: 
  - Partitioned Cache cho phép scale out tuyến tính. 
  - Khi thêm nhiều node vào cluster, dữ liệu và workload được phân phối rộng hơn → hệ thống xử lý nhiều request hơn mà không ảnh hưởng lớn đến performance.
- Load Distribution: 
  - Vì mỗi node chỉ quản lý một phần dữ liệu, nên tải được cân bằng tự nhiên → tránh tình trạng bottleneck trên một node duy nhất.
4. Fault Tolerance
- Single Point of Failure: 
  - Nếu một node down, dữ liệu trên node đó sẽ mất → đây là rủi ro lớn trong Partitioned Cache cơ bản.
- Replication for Resilience: 
  - Để khắc phục, Partitioned Cache thường được kết hợp với replica strategy.
  - Ví dụ: mỗi partition sẽ có một hoặc nhiều replica lưu trên node khác (Partition-Replica). 
  - Khi node chính bị lỗi, hệ thống vẫn có thể phục hồi dữ liệu từ replica.
5. Use Cases
- Ứng dụng quy mô lớn: 
  - Partitioned Cache phù hợp khi dữ liệu lớn đến mức không thể lưu toàn bộ trên một hoặc vài node.
- Performance-Critical Systems: 
  -     Các hệ thống cần high throughput và low latency, nhờ khả năng phân tán tải và giảm thời gian truy xuất.

👉 Partitioned Cache thường được dùng khi:
  - Cần high write performance và scalability: dữ liệu được phân mảnh (sharded) trên nhiều node → giảm contention.
  - Yêu cầu linear scalability: performance tăng tuyến tính theo số node trong cluster.

👉 Tình huống điển hình:
  - E-commerce platform có dataset lớn, đa dạng, update thường xuyên.
  - Hệ thống quản lý session data khối lượng lớn, cần phân phối qua nhiều server.

-- How does the Partition-Replica Cache in Distributed Caching? --
- Partition-Replica Cache trong NCache kết hợp ưu điểm của cả hai chiến lược Partitioned Cache và Replicated Cache để vừa đảm bảo tính sẵn sàng dữ liệu (high availability), khả năng chịu lỗi (fault tolerance), vừa duy trì khả năng mở rộng (scalability) như Partitioned Cache.

1. Partitioning of Data (Phân mảnh dữ liệu)
  - Data Distribution: 
    - Toàn bộ dữ liệu cache được chia thành nhiều partition. 
    - Mỗi dữ liệu sẽ được gán vào một partition dựa trên consistent hashing hoặc thuật toán tương tự, giúp phân phối dữ liệu đồng đều trên các node.
  - Primary Nodes: 
    - Mỗi partition được quản lý bởi một node chính (primary node).
    - Node này chịu trách nhiệm xử lý toàn bộ read/write cho partition đó.
2. Replication of Partitions (Sao chép partition)
  - Replica Nodes: 
    - Với mỗi partition chính, hệ thống sẽ tạo ra một hoặc nhiều replica đặt trên các node khác trong cluster để làm bản sao dự phòng.
  - Synchronization: 
    - Khi dữ liệu trong primary partition thay đổi (insert/update/delete), các thay đổi sẽ được đồng bộ hóa sang replica.
  - Synchronous replication: 
    - primary chờ replica xác nhận mới tiếp tục.
  - Asynchronous replication: 
    - primary không chờ, giúp tăng tốc độ ghi.
3. Handling Failures (Xử lý sự cố)
  - Failover Mechanism: 
    - Nếu node chính (primary) bị lỗi, một replica sẽ promote thành primary mới để duy trì tính sẵn sàng của dữ liệu.
  - Data Integrity & Consistency: 
    - Trong quá trình failover, hệ thống đảm bảo dữ liệu mới nhất và nhất quán, kể cả khi có giao dịch đang xử lý dở.
4. Read & Write Operations (Xử lý đọc/ghi)
  - Read: 
    - Thông thường đọc từ primary, nhưng có thể cấu hình để replica cũng xử lý đọc nhằm cân bằng tải trong hệ thống read-heavy.
  - Write: 
    - Tất cả ghi đều đi vào primary. Sau khi ghi, dữ liệu sẽ được replicate sang các bản sao.
5. Scalability & Load Balancing (Khả năng mở rộng & cân bằng tải)
  - Horizontal Scaling: 
    - Giống partitioned cache, có thể thêm node mới để phân phối lại partition và replica, giúp tăng dung lượng và hiệu năng.
  - Load Distribution: 
    - Hệ thống có thể phân tán cả read và write qua nhiều node, giảm bottleneck, tối ưu cho high-throughput systems.
6. Use Cases (Ứng dụng điển hình)
  - High Availability Systems: 
    - Ứng dụng cần uptime 100%, không chấp nhận downtime.
  - Large Distributed Applications: 
    - Quản lý lượng dữ liệu lớn, phân tán đa vùng địa lý, yêu cầu truy cập nhanh và ổn định.

✅ Partition-Replica Cache phù hợp khi:
  - Cần scalability cho cả đọc & ghi.
  - High availability và fault tolerance là yếu tố bắt buộc.
  - Cần hiệu năng cân bằng giữa đọc và ghi mà vẫn đảm bảo tính sẵn sàng dữ liệu.
- Ví dụ thực tế:
  - Website traffic rất cao, không được phép downtime.
  - Hệ thống phân tán toàn cầu, dữ liệu lớn, yêu cầu phản hồi nhanh ở nhiều location.

-- Advantages of NCache Clustering in Distributed Caching -- 
- Load Balancing (Cân bằng tải):
  - Cân bằng tải trong NCache cluster giúp phân phối các request của client và dữ liệu trên tất cả các server trong cluster. 
  - Điều này ngăn chặn tình trạng một server bị quá tải (bottleneck), từ đó tăng thông lượng (throughput) và hiệu năng tổng thể của cache.
- Scalability (Khả năng mở rộng):
  - NCache cluster có khả năng mở rộng rất cao. 
  - Bạn có thể thêm server mới vào cluster mà không cần downtime, và NCache sẽ tự động phân phối lại dữ liệu theo cấu hình mới. 
  - Điều này cực kỳ quan trọng để giữ vững hiệu năng khi nhu cầu ứng dụng tăng lên.
- Data Synchronization (Đồng bộ dữ liệu):
  - Cơ chế đồng bộ trong NCache đảm bảo mọi thay đổi trong cache (update, delete, add) đều được propagate (truyền) một cách nhất quán giữa các node. 
  - Điều này giúp duy trì tính toàn vẹn (integrity) và nhất quán (consistency) của dữ liệu trong toàn cluster.
- High Availability (Khả dụng cao):
  - Khả dụng cao đạt được nhờ cơ chế replication (sao chép dữ liệu) và automatic failover (tự động chuyển đổi khi có sự cố) trong cluster. 
  - Nếu một node gặp sự cố, cluster vẫn tiếp tục hoạt động bằng cách chia tải sang các node còn lại hoặc kích hoạt standby node (nếu được cấu hình).
- Dynamic Cluster Membership (Thay đổi thành viên động):
  - NCache cluster hỗ trợ dynamic membership, tức là có thể thêm hoặc gỡ bỏ server trong cluster một cách linh hoạt, ngay cả khi hệ thống đang chạy. 
  - Điều này rất hữu ích cho việc bảo trì hoặc mở rộng/thu hẹp cluster để đáp ứng tải thay đổi mà không ảnh hưởng đến availability của cache.