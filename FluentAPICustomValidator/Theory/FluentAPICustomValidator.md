-- Fluent API Custom Validators là gì? --
- Fluent API Custom Validators cho phép lập trình viên tạo ra logic kiểm tra (validation) tùy chỉnh, vượt ra ngoài những phương thức kiểm tra có sẵn như NotEmpty(), Length(), EmailAddress(),…
- Chúng cực kỳ hữu ích khi bạn cần những quy tắc kiểm tra đặc thù mà thư viện FluentValidation không cung cấp sẵn, ví dụ:
  1. Quy tắc kiểm tra phức tạp: 
    - Thực hiện các quy tắc nghiệp vụ liên quan đến nhiều thuộc tính hoặc phụ thuộc vào dịch vụ bên ngoài.
  2. Quy tắc nghiệp vụ riêng: 
    - Định nghĩa các điều kiện/giới hạn chỉ có trong domain của bạn.
  3. Kiểm tra có điều kiện: 
    - Áp dụng logic tùy theo trạng thái runtime, mối quan hệ giữa các thuộc tính, hoặc các yếu tố động khác.
  4. Tái sử dụng:
    - Gom logic kiểm tra lại một chỗ, dùng đi dùng lại cho nhiều model hoặc service khác nhau.

-- Cách sử dụng FluentValidation Custom Validators -- 
- Thư viện hỗ trợ nhiều cách viết custom validation: Must, MustAsync, Custom, CustomAsync. 
- Tùy trường hợp mà chọn đồng bộ (sync) hay bất đồng bộ (async), đồng thời là mức property-level (theo từng thuộc tính) hay object-level (toàn bộ đối tượng).

1. Must – Property-Level Synchronous Validation
- Áp dụng cho một thuộc tính, kiểm tra đồng bộ.
- Sử dụng khi logic chỉ liên quan đến chính property đó và không gọi tác vụ ngoài.
- Predicate trả về true nếu hợp lệ, false nếu không hợp lệ.

2. MustAsync – Property-Level Asynchronous Validation
- Áp dụng cho một thuộc tính, nhưng chạy bất đồng bộ.
- Dùng khi cần gọi DB, API ngoài, hoặc tác vụ async khác.

3. Custom – Object-Level Synchronous Validation
- Áp dụng ở cấp độ object, kiểm tra đồng bộ.
- Dùng khi validation cần so sánh nhiều property hoặc quy tắc nghiệp vụ phức tạp.
 
4. CustomAsync – Object-Level Asynchronous Validation
- Áp dụng ở cấp độ object, kiểm tra bất đồng bộ.
- Thường dùng khi validation object phải liên hệ đến nguồn ngoài (DB, API…).

- Khi nào nên sử dụng phương thức nào?
- Must / MustAsync: 
  - Sử dụng các phương thức này cho các xác thực ở cấp thuộc tính. 
  - Dùng Must cho các xác thực đơn giản, đồng bộ và MustAsync cho các xác thực yêu cầu các lệnh gọi bên ngoài (ví dụ: kiểm tra tính duy nhất trong cơ sở dữ liệu).
- Custom / CustomAsync: 
  - Sử dụng các phương thức này cho các xác thực ở cấp đối tượng, nơi nhiều thuộc tính cần được so sánh với nhau (ví dụ: đảm bảo Password và ConfirmPassword khớp nhau hoặc xác thực rằng một thành phố thuộc về một quốc gia đã chỉ định). Sử dụng CustomAsync nếu logic liên quan đến các hoạt động bất đồng bộ.