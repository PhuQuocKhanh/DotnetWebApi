-- Fluent API Async Validator là gì? -- 
- Fluent API Async Validators là một phần của thư viện FluentValidation. 
- Chúng cho phép thực hiện kiểm tra hợp lệ (validation) theo cách bất đồng bộ (asynchronous), rất hữu ích khi logic validation cần truy xuất dữ liệu từ nguồn bên ngoài hoặc thực hiện các thao tác tốn thời gian.
- Một số trường hợp thường gặp:
  - Kiểm tra dữ liệu trong cơ sở dữ liệu (ví dụ: kiểm tra trùng lặp email/username).
  - Gọi tới API bên ngoài để xác thực thông tin.
  - Áp dụng các business rule phức tạp yêu cầu tính toán nặng.
  - Việc dùng async validation giúp các kiểm tra này không chặn (block) luồng chính của request, từ đó ứng dụng vẫn giữ được khả năng phản hồi và mở rộng tốt hơn.

-- Khi nào nên dùng Fluent API Async Validators trong ASP.NET Core? -- 
- Async validators phù hợp cho các thao tác I/O-bound hoặc long-running operations. Cụ thể:
  - Database Checks: Đảm bảo email hoặc username là duy nhất; kiểm tra khóa ngoại (foreign key) có tồn tại.
  - External API Validation: Khi cần xác thực dữ liệu thông qua API bên ngoài.
  - Complex Calculations: Thực hiện các phép tính phức tạp/tốn tài nguyên mà không chặn luồng chính.

-- Cách sử dụng Fluent API Async Validators -- 
- FluentValidation cung cấp các phương thức MustAsync() và CustomAsync(), cho phép định nghĩa các rule kiểm tra bất đồng bộ.
Chúng thường được dùng trong các tình huống cần:
  - Xác thực dữ liệu đầu vào dựa trên nguồn dữ liệu bên ngoài.
  - Áp dụng logic nghiệp vụ (business logic) phức tạp mà không thể chạy đồng bộ.

-- Lợi ích của việc sử dụng Fluent API Async Validators trong ASP.NET Core Web API -- 
- Fluent API Async Validators mang lại nhiều lợi ích, đặc biệt khi logic validation liên quan đến các phụ thuộc bên ngoài như cơ sở dữ liệu hoặc dịch vụ external. 
- Một số lợi ích chính:
  1. Non-blocking Operations (Không chặn luồng): 
    - Async validators đảm bảo rằng các thao tác tốn thời gian (như gọi database hoặc API ngoài) sẽ không làm nghẽn main thread.
  2. Enhanced Scalability (Tăng khả năng mở rộng): 
    - Validation bất đồng bộ tránh tình trạng block thread, giúp server xử lý được nhiều request đồng thời hơn.
  3. Complex Business Rules (Hỗ trợ business rule phức tạp): 
    - Dễ dàng tích hợp các rule phức tạp (như kiểm tra uniqueness, logic về chiết khấu) phụ thuộc vào dữ liệu bên ngoài.
  4. Improved User Experience (Cải thiện trải nghiệm người dùng): 
    - Validation bất đồng bộ diễn ra nhanh, không gây giật lag, mang lại trải nghiệm mượt mà hơn cho client.
- Với việc kết hợp giữa validation bất đồng bộ (cho uniqueness, kiểm tra foreign key tồn tại, business rule phức tạp) và validation đồng bộ, ứng dụng có thể đảm bảo tính toàn vẹn dữ liệu và thực thi chặt chẽ các ràng buộc nghiệp vụ.