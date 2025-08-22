-- Fluent API Validation trong ASP.NET Core Web API -- 
- Trong bài viết này, chúng ta sẽ tìm hiểu Fluent API Validation trong ứng dụng ASP.NET Core Web API thông qua một ví dụ thực tế. (Trong bài trước chúng ta đã thảo luận về Caching trong ASP.NET Core Web API).
- Validation là một phần quan trọng trong bất kỳ ứng dụng web nào nhằm đảm bảo dữ liệu nhận từ phía client hợp lệ, nhất quán và tuân theo các quy tắc nghiệp vụ. 
- Trong ASP.NET Core Web API, validation có thể được triển khai theo nhiều cách khác nhau, bao gồm:
  1. Data Annotations: 
    - Sử dụng các attribute gắn trực tiếp lên property của model
    - ví dụ [Required], [StringLength], … Đây là cách validation dựa trên attribute.
  2. Fluent Validation: 
    - Viết các rule validation trong một class riêng biệt với cú pháp fluent interface. 
    - Cách này tách biệt logic validation khỏi model.
  3. Manual Validation: 
    - Tự viết logic validation trong controller hoặc service khi cần.
- Trong bài viết này, ta tập trung vào Fluent API Validation trong ASP.NET Core Web API với Entity Framework Core và SQL Server Database.

-- Fluent API Validation trong ASP.NET Core là gì? -- 
- Fluent API Validation (thường được triển khai thông qua thư viện FluentValidation) là cách định nghĩa rule validation bằng lập trình (fluent style). 
- Khác với Data Annotations (sử dụng attribute ngay trên model), Fluent API Validation tách logic validation ra thành một validator class (kế thừa từ AbstractValidator<T>).

-- Khi nào nên dùng Fluent API Validation? -- 
- Một số trường hợp điển hình:
1. Complex Validation Rules (quy tắc phức tạp)
  - Khi validation phụ thuộc vào nhiều property hoặc điều kiện bên ngoài.
  - Ví dụ: sản phẩm thuộc nhóm luxury thì giá phải > 500$, và nếu có Discount thì không vượt quá 10% giá bán.
  - Các rule dạng này dễ triển khai hơn với Fluent Validation.
2. Separation of Concerns (tách biệt trách nhiệm)
  - Ví dụ Product model được dùng ở nhiều nơi (create, update, list).
  - Thay vì nhồi nhét validation bằng attribute trực tiếp vào model, ta tạo validator class riêng.
  - Điều này giúp model gọn gàng và dễ maintain/test.
3. Dynamic hoặc Conditional Validation (validation động hoặc theo điều kiện)
  - Rule có thể thay đổi theo runtime condition hoặc business logic.
  - Ví dụ: ShippingDate chỉ cần validate khi Status = Shipped.
  - Reusable Validation Rules (tái sử dụng rule)
  - Ví dụ nhiều model (User, Admin, Vendor) đều có property Email với rule giống nhau.
  - Ta có thể tạo một EmailValidator riêng và reuse thay vì lặp lại rule.

-- Các nhóm phương thức trong Fluent API Validation -- 
1. Basic Validation Methods (cơ bản)
  - NotEmpty(): Không null, không rỗng, không whitespace.
  - NotNull(): Không được null.
  - Length(min, max): Độ dài chuỗi trong khoảng [min, max].
  - InclusiveBetween(min, max): Giá trị nằm trong khoảng (bao gồm cả boundary).
  - ExclusiveBetween(min, max): Giá trị nằm trong khoảng (không bao gồm boundary).
  - GreaterThan(value): Lớn hơn giá trị chỉ định.
  - GreaterThanOrEqualTo(value): Lớn hơn hoặc bằng.
  - LessThan(value): Nhỏ hơn.
  - LessThanOrEqualTo(value): Nhỏ hơn hoặc bằng.
2. String Validation Methods (chuỗi)
  - Matches(regex): Phải match regex pattern.
  - EmailAddress(): Đúng định dạng email.
  - MaximumLength(max): Không vượt quá max.
  - MinimumLength(min): Không nhỏ hơn min.
  - NotEqual(value): Không bằng giá trị chỉ định.
  - StartsWith(value): Bắt đầu bằng chuỗi chỉ định.
  - EndsWith(value): Kết thúc bằng chuỗi chỉ định.
3. Custom Validation Methods (tùy chỉnh)
  - Must(condition): Rule sync với điều kiện custom.
  - MustAsync(asyncCondition): Rule async với điều kiện custom.
  - WithMessage(message): Tùy chỉnh message lỗi.
  - Custom(): Tạo rule phức tạp, có thể check nhiều property (sync).
  - CustomAsync(): Rule phức tạp nhưng cần async (gọi DB, API…).
4. Conditional Validation Methods (theo điều kiện)
  - When(condition): Chỉ validate khi condition = true.
  - Unless(condition): Chỉ validate khi condition = false.
5. Chaining Rules (chuỗi rule)
  - Cho phép kết hợp nhiều rule fluent trên cùng một property, ví dụ:
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Required")
        .Length(5, 50).WithMessage("The length must be between 5 and 50 characters");
6. Validation for Complex Types (validate object con)
  - SetValidator(): Gọi validator khác cho nested object/child property.
7. Collection Validation (validate collection)
  - ForEach(): Áp rule cho từng phần tử trong collection.

-- Cách sử dụng Fluent API Validation trong ASP.NET Core Web API --
1. Tạo lớp Validator
  - Tạo một lớp mới kế thừa từ AbstractValidator<T>, trong đó T là model cần được validate. 
  - Các quy tắc kiểm tra dữ liệu (validation rules) sẽ được định nghĩa trong constructor của lớp này.
2. Đăng ký FluentValidation trong Program.cs 
  - Thêm FluentValidation vào Dependency Injection Container trong lớp Program.
3. Sử dụng Auto Validation trong Controller
  - Khi có request gửi đến API, FluentValidation sẽ tự động validate model dựa trên các rule đã định nghĩa trong Validator class. Cơ chế này chỉ hoạt động nếu Auto Validation đã được bật.
4. Validation thủ công (Manual Validation)
  - Nếu muốn kiểm soát chặt chẽ hơn, bạn có thể tắt auto validation và gọi validator một cách thủ công trong action method của controller.

-- Khi nào nên triển khai Manual Fluent API Validation trong ASP.NET Core Web API? -- 
- Manual Fluent API validation phù hợp trong các tình huống:
  - Validation cần áp dụng theo điều kiện dựa trên business logic mà ta chưa biết trước khi khởi động ứng dụng.
  - Có những kịch bản phức tạp, mỗi ngữ cảnh áp dụng một bộ rule khác nhau.
  - Cần kiểm soát rõ ràng khi nào và cách thức validation được kích hoạt.
  - Muốn custom response chi tiết cho lỗi validation, không phụ thuộc vào cách xử lý mặc định của ModelState.

-- Sự khác biệt giữa Fluent API Validation và Data Annotation Validation trong ASP.NET Core Web API --
  - Data Annotations: phù hợp cho các kịch bản đơn giản, nơi các quy tắc validation trực tiếp và không cần tái sử dụng giữa nhiều model. Cách này dễ triển khai và dễ hiểu.
  - Fluent API Validation: thích hợp cho các tình huống phức tạp, có tính tái sử dụng hoặc động (dynamic). Việc tách logic validation ra khỏi model giúp code gọn gàng và dễ bảo trì hơn. Đây cũng là lựa chọn tốt khi cùng một logic validation cần áp dụng cho nhiều model khác nhau.

- Cách tiếp cận lai (Hybrid Approach):
  - Có thể kết hợp cả hai. 
  - Ví dụ: áp dụng các rule đơn giản, quen thuộc (như [Required]) bằng Data Annotations, còn dùng Fluent Validation cho các rule phức tạp, có điều kiện hoặc cần tái sử dụng. 
  - Cách kết hợp này giúp giữ validation đơn giản ngay trong model, đồng thời tận dụng sức mạnh và sự linh hoạt của Fluent Validation khi cần.
- Lưu ý:
  - Gói FluentValidation.AspNetCore hiện không còn được duy trì.
  - Microsoft khuyến nghị sử dụng gói FluentValidation core và triển khai manual validation.
  - Điều này giúp giải quyết hạn chế của pipeline validation mặc định của ASP.NET Core (vốn không hỗ trợ bất đồng bộ tốt) và loại bỏ sự phức tạp từ việc auto-validation.
  - Trong các bài tiếp theo, mình sẽ hướng dẫn cách dùng trực tiếp gói FluentValidation.