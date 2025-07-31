-- What is Validation? --
- Validation (xác thực dữ liệu) là quá trình đảm bảo rằng dữ liệu được gửi từ phía client (thường là qua HTTP request) đáp ứng đúng định dạng, ràng buộc và các quy tắc đã được định nghĩa trong ứng dụng trước khi xử lý.
- Validation đóng vai trò quan trọng cả ở phía client và server nhằm đảm bảo dữ liệu là chính xác, đầy đủ và an toàn trước khi thực hiện các thao tác như lưu vào cơ sở dữ liệu hoặc xử lý trong logic nghiệp vụ.
- Trong ngữ cảnh của ASP.NET Core Web API, validation thường được hiểu là quá trình kiểm tra dữ liệu từ client dựa trên một tập hợp các quy tắc trước khi truyền vào logic xử lý của API. Những kiểm tra này có thể bao gồm:
  - Xác minh các trường không được để trống.
  - Đảm bảo giá trị số nằm trong khoảng cho phép.
  - Kiểm tra định dạng email hợp lệ, v.v.
  - Mục tiêu là ngăn chặn dữ liệu không hợp lệ gây ra lỗi, mất tính nhất quán hoặc rủi ro bảo mật trong hệ thống.

-- Why Do We Need Validation in ASP.NET Core Web API? --
- ASP.NET Core Web API là backend phục vụ nhiều loại client như: trình duyệt web, ứng dụng di động hoặc dịch vụ khác. Việc thực hiện validation trong API là cần thiết vì:
  - Đảm bảo tính toàn vẹn dữ liệu (Data Integrity): Đảm bảo dữ liệu được lưu vào hệ thống là chính xác, phù hợp với yêu cầu của ứng dụng.
  - Cải thiện trải nghiệm người dùng (User Experience): Phản hồi rõ ràng khi dữ liệu sai giúp người dùng dễ dàng sửa lỗi và giảm sự khó chịu.
  - Bảo mật (Security): Giảm thiểu rủi ro tấn công như SQL Injection, XSS (Cross-site Scripting), CSRF (Cross-Site Request Forgery) thông qua việc kiểm tra và làm sạch dữ liệu đầu vào.
  - Ngăn ngừa lỗi (Error Prevention): Bắt các lỗi liên quan đến dữ liệu sai định dạng ngay từ đầu pipeline request, trước khi nó gây ra lỗi logic hoặc hệ thống.

-- Types of Validation --
- Validation thường được phân thành 3 loại chính:
  1. Client-Side Validation (Xác thực phía client):
    - Được thực hiện tại trình duyệt hoặc ứng dụng di động trước khi gửi request lên server.
    - Giúp phản hồi nhanh cho người dùng và giảm lượng request không cần thiết.
    - Tuy nhiên, không thể tin cậy hoàn toàn vì có thể bị bypass.
  2. Server-Side Validation (Xác thực phía server):
    - Diễn ra tại server sau khi nhận dữ liệu từ client.
    - Là nơi áp dụng các quy tắc nghiệp vụ và đảm bảo tính toàn vẹn dữ liệu.
    - Là bắt buộc và đáng tin cậy hơn client-side validation.
  3. Database Validation (Xác thực tại cơ sở dữ liệu):
    - Được áp dụng thông qua các constraint như NOT NULL, CHECK, UNIQUE trong cơ sở dữ liệu.
    - Đóng vai trò như lớp bảo vệ cuối cùng khi các lớp validation phía trên bị bỏ qua hoặc không đầy đủ.

-- Different Ways to Implement Server-Side Validations in ASP.NET Core Web API --
- ASP.NET Core Web API cung cấp nhiều phương pháp để thực hiện kiểm tra dữ liệu phía server:
1. Kiểm tra hợp lệ bằng Data Annotations
  - Đây là cách phổ biến và đơn giản nhất, sử dụng các thuộc tính (attributes) như Required, Range, StringLength, v.v. để xác định các ràng buộc trực tiếp trên model.
  - Phù hợp với các logic kiểm tra cơ bản và dễ triển khai.
2. Sử dụng FluentValidation
  - Đây là một thư viện mạnh mẽ trong .NET, cho phép định nghĩa các quy tắc kiểm tra phức tạp bằng cú pháp Fluent API. 
  - FluentValidation giúp tách riêng logic kiểm tra ra khỏi model, tăng tính linh hoạt và dễ kiểm soát.
3. Kiểm tra thủ công (Manual Validation)
  - Trong một số tình huống, validation cần được xử lý thủ công trong controller hoặc service layer, đặc biệt khi logic kiểm tra vượt quá khả năng biểu đạt của Data Annotations hoặc FluentValidation. 
  - Cách này phù hợp với các kiểm tra mang tính nghiệp vụ đặc thù hoặc phụ thuộc nhiều vào dữ liệu động.

-- Data Annotation Attributes in ASP.NET Core Web API: --
1. [Required]
- Đảm bảo rằng thuộc tính không được null hoặc rỗng. Thường dùng cho các trường bắt buộc phải có dữ liệu.
2. [StringLength]
Giới hạn độ dài tối thiểu và tối đa của chuỗi. Dùng để kiểm tra độ dài dữ liệu nhập vào.
3. [MaxLength] / [MinLength]
Xác định độ dài tối đa hoặc tối thiểu cho chuỗi hoặc mảng.
4. [Range]
Kiểm tra giá trị số nằm trong một khoảng xác định (thường dùng cho tuổi, giá tiền, v.v).
5. [RegularExpression]
Kiểm tra chuỗi có khớp với một biểu thức chính quy (Regex) hay không. Dùng cho các định dạng phức tạp như mã bưu điện, số điện thoại,...
6. [EmailAddress]
Xác minh dữ liệu có phải là email hợp lệ không.
7. [Compare]
So sánh giá trị giữa hai thuộc tính, thường dùng để kiểm tra xác nhận mật khẩu.
8. [Phone]
Kiểm tra số điện thoại có hợp lệ hay không.
9. [Url]
Kiểm tra định dạng URL hợp lệ.
10. [CreditCard]
Kiểm tra số thẻ tín dụng có hợp lệ hay không.