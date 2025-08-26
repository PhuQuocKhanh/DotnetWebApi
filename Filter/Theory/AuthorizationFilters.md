-- Bộ lọc Phân quyền trong ASP.NET Core Web API là gì? -- 
- Bộ lọc Phân quyền trong ASP.NET Core Web API là các bộ lọc chuyên dụng được thực thi trước khi logic chính của API chạy. 
- Nhiệm vụ chính của chúng là xác minh xem người dùng hiện tại có quyền hạn phù hợp để truy cập một endpoint API cụ thể (action hoặc controller) hay không.
- Nếu người dùng được phân quyền, bộ lọc sẽ cho phép request tiếp tục.
- Nếu người dùng không được phân quyền (ví dụ: chưa đăng nhập, thiếu vai trò hoặc claim cần thiết), bộ lọc sẽ chặn request và trả về một lỗi thích hợp, thường là HTTP 401 (Unauthorized, nếu người dùng chưa được xác thực) hoặc 403 (Forbidden, nếu người dùng đã được xác thực nhưng không có quyền).
- Nói một cách đơn giản, hãy coi Bộ lọc Phân quyền như một người bảo vệ nghiêm ngặt đứng ở cửa của mọi khu vực hạn chế trong một tòa nhà. Chỉ những người có thông tin xác thực đúng mới được vào.

-- Bộ lọc Phân quyền hoạt động như thế nào trong ASP.NET Core Web API -- 
- Bước 1: Yêu cầu từ Người dùng (User Request):
  - Quá trình bắt đầu khi người dùng gửi một request đến Web API (ví dụ: cố gắng truy cập tài nguyên hoặc thực hiện một hành động).
- Bước 2: Bộ lọc Xác thực (Authentication Filter):
  - Bộ lọc này kiểm tra thông tin xác thực của người dùng (như tên người dùng/mật khẩu, token, v.v.) để xác minh danh tính của người dùng.
  - Nếu thông tin xác thực không hợp lệ, nó sẽ ngay lập tức trả về phản hồi 401 Unauthorized, cho biết người dùng chưa được xác thực. Đường đi này được thể hiện bằng mũi tên màu đỏ hướng lên trên đến thông báo lỗi có nhãn "Invalid Credentials".
  - Nếu thông tin xác thực hợp lệ, request sẽ tiếp tục đến bước tiếp theo (được thể hiện bằng mũi tên màu xanh lá cây hướng xuống dưới có nhãn "Valid Credentials").
- Bước 3: Bộ lọc Phân quyền (Authorization Filter)
  - Sau khi đã được xác thực, bộ lọc này kiểm tra xem người dùng có quyền hạn phù hợp để truy cập endpoint API hoặc thực hiện hành động được yêu cầu hay không.
  - Nếu quyền hạn không hợp lệ hoặc không đủ, nó sẽ trả về phản hồi 403 Forbidden, nghĩa là người dùng đã được xác thực nhưng không được phép thực hiện hành động. 
  - Đường đi này được thể hiện bằng mũi tên màu cam hướng lên trên đến thông báo lỗi có nhãn "Invalid Permissions".
  - Nếu quyền hạn hợp lệ, request sẽ tiếp tục đến bước cuối cùng (được thể hiện bằng mũi tên màu xanh lá cây hướng xuống dưới có nhãn "Valid Permissions").
- Bước 4: Endpoint API (Phương thức Action)
Sau khi xác thực và phân quyền thành công, request sẽ đến endpoint API, nơi phương thức action thực sự được thực thi và xử lý request tương ứng.

-- Xác thực (Authentication) là gì? -- 
- Xác thực là quá trình xác nhận danh tính của một người dùng đang cố gắng truy cập vào một hệ thống. 
- Nó trả lời câu hỏi quan trọng: "Bạn là ai?" Khi một người dùng cố gắng truy cập một hệ thống hoặc ứng dụng, xác thực sẽ xác nhận danh tính của họ bằng cách sử dụng thông tin xác thực như tên người dùng và mật khẩu, token bảo mật, hoặc dữ liệu sinh trắc học như dấu vân tay hoặc nhận dạng khuôn mặt.
- Trong ASP.NET Core, sau khi người dùng được xác thực thành công, framework sẽ tạo một đối tượng ClaimsPrincipal được lưu trữ trong đối tượng HttpContext.User. Đối tượng này mang theo danh tính và các claim (như ID người dùng, email hoặc vai trò) của người dùng đã được xác thực.

-- Phân quyền (Authorization) là gì? -- 
- Phân quyền là quá trình quyết định những gì một người dùng đã được xác thực được phép làm. 
- Nó trả lời câu hỏi: "Bạn có những quyền hạn hay đặc quyền gì?" Sau khi danh tính của người dùng được xác minh (đã xác thực), các kiểm tra phân quyền sẽ xác định xem người dùng có quyền hạn hoặc vai trò cần thiết để truy cập các tài nguyên cụ thể hoặc thực hiện các hành động cụ thể hay không. 
- ASP.NET Core hỗ trợ phân quyền dựa trên vai trò (role-based), dựa trên chính sách (policy-based) và dựa trên claim (claims-based) để thực hiện điều này.
- Trong ASP.NET Core, sau khi người dùng được xác thực, HttpContext.User sẽ được điền đầy đủ thông tin danh tính và claim.
- Lớp phân quyền sẽ đánh giá các chính sách, vai trò hoặc quy tắc tùy chỉnh dựa trên các claim liên quan để quyết định xem người dùng đó có thể truy cập một action của controller, tài nguyên hoặc hoạt động cụ thể nào không.
- Nếu người dùng thiếu quyền hạn cần thiết, bạn thường sẽ thấy phản hồi HTTP 403 (Forbidden).

-- Làm thế nào để triển khai Bộ lọc Phân quyền trong ASP.NET Core Web API? --
- Chúng ta cần tuân theo các bước dưới đây:
  - Sử dụng sẵn có (Built-in): 
    - Dùng attribute [Authorize] trên các controller hoặc action để bắt buộc phân quyền bằng cách sử dụng vai trò, chính sách hoặc claim.
  - Tùy chỉnh (Custom): 
    - Triển khai interface IAuthorizationFilter hoặc IAsyncAuthorizationFilter cho các logic phức tạp, đặc thù của nghiệp vụ.
  - Đăng ký (Registration): 
    - Các bộ lọc có thể được đăng ký toàn cục (áp dụng ở mọi nơi) hoặc cục bộ (cho controller/action cụ thể).

-- Các Attribute [Authorize] và [AllowAnonymous] trong ASP.NET Core Web API: -- 
- Mặc định, trong các ứng dụng ASP.NET Core Web API, tất cả các phương thức action của tất cả các controller đều có thể được truy cập bởi cả người dùng đã xác thực và người dùng ẩn danh. 
- Tuy nhiên, nếu bạn muốn các phương thức action chỉ khả dụng cho những người dùng đã được xác thực và phân quyền, bạn cần sử dụng Bộ lọc Phân quyền trong ASP.NET Core MVC. 
- ASP.NET Core cung cấp hai attribute tích hợp sẵn, [Authorize] và [AllowAnonymous], có thể được sử dụng làm bộ lọc phân quyền.
  1. [Authorize]: 
    - Attribute này bắt buộc phân quyền trên controller hoặc action. 
    - Khi được áp dụng, chỉ những người dùng đã được xác thực và đáp ứng các vai trò hoặc chính sách được chỉ định mới có thể truy cập endpoint. 
    - Ví dụ: [Authorize] có nghĩa là chỉ người dùng đã đăng nhập mới có thể truy cập. 
    - [Authorize(Roles = "Admin")] có nghĩa là chỉ người dùng có vai trò "Admin" mới có thể truy cập.
  2. [AllowAnonymous]: 
    - Attribute này cho phép truy cập không cần xác thực vào một controller hoặc action, ngay cả khi [Authorize] được áp dụng ở cấp độ toàn cục hoặc controller. 
    - Ví dụ, một endpoint đăng nhập hoặc đăng ký công khai sử dụng [AllowAnonymous] để bất kỳ ai cũng có thể truy cập.

-- Triển khai Bộ lọc Phân quyền trong ASP.NET Core Web API: -- 
- Bây giờ, chúng ta sẽ phát triển một ứng dụng ASP.NET Core Web API để minh họa cách bảo mật các endpoint REST API bằng xác thực dựa trên JWT và các bộ lọc phân quyền tích hợp sẵn. Nó cho phép người dùng đăng nhập bằng thông tin xác thực của họ để nhận được một JWT token. 
- Token này sau đó được yêu cầu để truy cập các endpoint được bảo vệ.
- Ứng dụng sẽ cho thấy cách hạn chế quyền truy cập vào các tài nguyên API bằng các attribute như [Authorize], [Authorize(Roles = "…")] và [AllowAnonymous], bao gồm các kịch bản thực tế phổ biến:
  - Endpoint công khai (public): Mở cho tất cả mọi người, không cần xác thực.
  - Endpoint đã xác thực: Yêu cầu một JWT token hợp lệ.
  - Truy cập theo một vai trò duy nhất: Chỉ những người dùng có vai trò cụ thể (ví dụ: Admin).
  - Nhiều vai trò (logic AND): Người dùng phải có tất cả các vai trò được chỉ định.
  - Nhiều vai trò (logic OR): Người dùng có bất kỳ vai trò nào trong số các vai trò được chỉ định đều có thể truy cập.


-- Khi nào chúng ta nên sử dụng Bộ lọc Phân quyền trong ASP.NET Core Web API? --
- Bộ lọc phân quyền là một công cụ mạnh mẽ để thực thi các chính sách bảo mật ở cấp độ API.
- Hãy sử dụng chúng bất cứ khi nào bạn cần kiểm soát ai có thể truy cập cái gì trong Web API của mình. Dưới đây là các kịch bản chính mà bộ lọc phân quyền là cần thiết:
1. Bảo vệ Dữ liệu Nhạy cảm
  - Một số endpoint API tiết lộ thông tin bí mật hoặc nhạy cảm (ví dụ: hồ sơ người dùng, dữ liệu tài chính hoặc báo cáo quản trị). 
  - Việc truy cập trái phép vào các endpoint này có thể dẫn đến rò rỉ hoặc lạm dụng dữ liệu.
  - Ví dụ: Trong một API ngân hàng, chỉ những người dùng có vai trò AccountHolder mới có thể truy cập chi tiết tài khoản của chính họ, trong khi người dùng có vai trò BankManager có thể truy cập báo cáo cho tất cả các tài khoản. Bộ lọc phân quyền đảm bảo chỉ những người dùng phù hợp mới thấy được dữ liệu thích hợp.
2. Kiểm soát Truy cập Dựa trên Vai trò (RBAC)
  - Các loại người dùng khác nhau nên có các quyền hạn khác nhau. 
  - Bộ lọc phân quyền giúp bạn cho phép hoặc từ chối quyền truy cập vào các endpoint API dựa trên vai trò của người dùng (như Admin, Manager, Employee, Customer, v.v.).
  - Ví dụ: Trong một API thương mại điện tử, chỉ những người dùng có vai trò Admin mới có thể thêm hoặc xóa sản phẩm, trong khi người dùng có vai trò Customer chỉ có thể xem sản phẩm và đặt hàng. Bộ lọc phân quyền ngăn chặn khách hàng thực hiện các hành động chỉ dành cho quản trị viên.
3. Tập trung Logic Phân quyền
  - Thay vì viết cùng một đoạn mã kiểm tra quyền hạn ở nhiều nơi, bạn có thể đặt tất cả logic phân quyền của mình vào một nơi (bộ lọc). 
  - Điều này làm cho mã của bạn sạch hơn, dễ bảo trì hơn và ít bị lỗi hơn.
  - Ví dụ: Nếu bạn muốn đảm bảo rằng chỉ những người dùng có trạng thái tài khoản "active" mới có thể sử dụng bất kỳ phần nào của API, bạn có thể đặt kiểm tra này trong một bộ lọc phân quyền. Bằng cách này, mọi endpoint đều được bảo vệ tự động, và bạn không phải lặp lại việc kiểm tra trong mỗi controller.
4. Các Quy tắc Phân quyền Tùy chỉnh
  - Đôi khi các quy tắc truy cập của bạn không chỉ đơn giản là về vai trò—chúng có thể phụ thuộc vào logic nghiệp vụ phức tạp (ví dụ: truy cập dựa trên thời gian, quy tắc theo phòng ban hoặc quyền hạn động). Bộ lọc phân quyền cho phép bạn triển khai bất kỳ logic tùy chỉnh nào bạn cần.
  - Ví dụ: Trong một API nền tảng giao dịch tài chính, chỉ những người dùng đã hoàn thành xác minh KYC (Know Your Customer) mới có thể đặt lệnh giao dịch. Hoặc, một số hành động nhất định (như chuyển tiền giá trị cao) chỉ được phép trong giờ làm việc, được thực thi thông qua logic tùy chỉnh trong một bộ lọc phân quyền.
5. Bảo mật API cho Bên thứ ba
  - Nếu API của bạn mở cho các ứng dụng hoặc đối tác bên thứ ba, bạn phải đảm bảo rằng chỉ những đối tác được tin cậy và được ủy quyền mới có thể truy cập các endpoint hoặc dữ liệu nhất định.
  - Ví dụ: Một API xử lý thanh toán có thể chỉ cho phép các nhà cung cấp đã đăng ký và được phê duyệt (chứ không phải người dùng thông thường) truy cập các endpoint để hoàn tiền hoặc tạo báo cáo tài chính.