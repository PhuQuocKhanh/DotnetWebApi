-- Xác thực và Phân quyền trong Web API -- 
- Trong thế giới kỹ thuật số ngày nay, bảo mật là ưu tiên hàng đầu cho bất kỳ ứng dụng hoặc dịch vụ nào dựa trên nền tảng web. 
- Khi ngày càng có nhiều dữ liệu và chức năng được cung cấp thông qua Web API, việc đảm bảo chỉ những người dùng hợp lệ mới có thể truy cập thông tin nhạy cảm trở nên cực kỳ quan trọng. 
- Hai khái niệm nền tảng giúp đạt được điều này là xác thực (authentication) và phân quyền (authorization). Trong các ứng dụng web:
  - Xác thực là việc xác minh danh tính của người dùng khi họ đăng nhập bằng các thông tin như tên người dùng và mật khẩu hoặc dữ liệu sinh trắc học.
  - Phân quyền là việc xác định những tính năng, trang hoặc dữ liệu mà người dùng đã được xác thực có thể truy cập, dựa trên vai trò hoặc quyền hạn được gán cho họ.
  - Quy trình hai bước này là thiết yếu để duy trì an ninh hệ thống, bảo vệ dữ liệu nhạy cảm và đảm bảo người dùng chỉ có thể thực hiện các hành động mà họ được phép.

-- Xác thực (Authentication) là gì? --
- Xác thực là quá trình mà hệ thống máy tính xác nhận và kiểm tra danh tính của một người dùng hoặc một thực thể đang cố gắng truy cập. 
- Xác thực trả lời câu hỏi cơ bản: Bạn là ai? Đây là bước mà hệ thống yêu cầu người dùng chứng minh danh tính của mình trước khi cho phép vào trong.
- Trong bất kỳ hệ thống an toàn nào, dù là ngân hàng, trường học, văn phòng hay một dịch vụ trực tuyến, bạn đều không muốn người lạ đột nhập và giả mạo người khác. 
- Do đó, trước khi cho phép bất kỳ ai truy cập tài nguyên (dữ liệu, tiền bạc, dịch vụ, v.v.), hệ thống phải xác minh danh tính của họ. 

-- Xác thực hoạt động như thế nào? --
- Người dùng cung cấp Thông tin xác thực (Credentials): 
  - Khi ai đó cố gắng đăng nhập vào một hệ thống, dù đó là một trang web, một ứng dụng hay một máy chủ bảo mật, họ sẽ cung cấp một hình thức nhận dạng, thường được gọi là credentials. Các thông tin phổ biến nhất là:
        - Tên người dùng (hoặc email)
        - Mật khẩu
- Hệ thống kiểm tra thông tin xác thực với một nguồn dữ liệu: 
  - Hệ thống xác minh các thông tin này dựa trên dữ liệu được lưu trữ. 
  - Dữ liệu này thường được lưu trong một cơ sở dữ liệu an toàn, nơi hệ thống duy trì tên người dùng hợp lệ và mật khẩu đã được mã hóa của tất cả người dùng đã đăng ký.
- Quá trình xác minh:
  - Nếu thông tin xác thực khớp với những gì được lưu trong cơ sở dữ liệu, hệ thống sẽ xác nhận danh tính của người dùng.
  - Nếu không khớp, hệ thống sẽ từ chối nỗ lực đăng nhập.
- Cấp quyền hoặc Từ chối truy cập: 
  - Một khi hệ thống xác nhận danh tính của người dùng, nó sẽ cho phép người dùng truy cập vào hệ thống hoặc các tài nguyên mà họ được cấp quyền.

-- Phân quyền (Authorization) là gì? -- 
- Phân quyền là quá trình xác định những gì một người dùng đã được xác thực được phép làm hoặc truy cập trong một hệ thống. 
- Nó kiểm soát các quyền hạn hoặc đặc quyền được gán cho một người dùng sau khi danh tính của họ đã được xác minh.
- Phân quyền trả lời câu hỏi: 
  - Bạn được phép làm gì? Tất cả đều xoay quanh quyền hạn, quyết định xem phần nào của hệ thống hoặc hoạt động nào có sẵn cho một người dùng.
- Ngay cả sau khi đã xác nhận danh tính của ai đó (xác thực), bạn không thể để mọi người làm mọi thứ. Một số người dùng cần nhiều quyền hơn (chẳng hạn như quản trị viên - admin), trong khi những người khác chỉ nên có quyền truy cập hạn chế (như người dùng thông thường).

-- Phân quyền hoạt động như thế nào? -- 
- Xác thực trước tiên: 
  - Trước khi phân quyền diễn ra, hệ thống phải xác nhận danh tính của người dùng thông qua xác thực (ví dụ: kiểm tra tên người dùng và mật khẩu).
- Gán Quyền hạn: 
  - Mỗi người dùng được gán các vai trò hoặc quyền hạn cụ thể, xác định các hành động họ có thể thực hiện và dữ liệu họ có thể truy cập.
  - Các vai trò và quyền hạn này thường được quản lý bởi quản trị viên hệ thống hoặc được định nghĩa bởi các quy tắc nghiệp vụ.
- Kiểm tra Quyền truy cập: 
  - Khi người dùng cố gắng truy cập một tài nguyên cụ thể (chẳng hạn như một tệp, trang web hoặc một endpoint của API), hệ thống sẽ kiểm tra xem các quyền hạn được gán cho người dùng đó có cho phép họ thực hiện hành động đó hay không.
- Cho phép hoặc Từ chối truy cập:
  - Nếu người dùng có các quyền cần thiết, hệ thống sẽ cấp quyền truy cập vào tài nguyên hoặc cho phép hành động được yêu cầu.
  - Nếu không, quyền truy cập sẽ bị từ chối, thường đi kèm với một thông báo lỗi như 403 Forbidden hoặc Access Denied.

-- Tại sao chúng ta cần Xác thực trong Web API? -- 
- Web API (Giao diện lập trình ứng dụng) được thiết kế để cho phép các ứng dụng phần mềm khác nhau giao tiếp với nhau qua internet. 
- Hầu hết các Web API hiện đại đều tuân theo kiến trúc REST (Representational State Transfer), trong đó có một nguyên tắc quan trọng là tính phi trạng thái (statelessness).

-- Tính phi trạng thái nghĩa là gì? -- 
- Trong một hệ thống phi trạng thái, máy chủ (server) không lưu giữ bất kỳ thông tin nào (trạng thái phiên - session state) về máy khách (client) giữa các yêu cầu.
- Mỗi yêu cầu HTTP từ client đến server phải tự chứa đựng (self-contained), nghĩa là nó phải bao gồm tất cả thông tin cần thiết để server hiểu và thực hiện yêu cầu.
- Server coi mọi yêu cầu là mới và độc lập, không dựa vào các tương tác trong quá khứ.

-- Tại sao tính phi trạng thái lại quan trọng đối với Xác thực? -- 
- Vì server không ghi nhớ danh tính của client từ các yêu cầu trước, thông tin xác thực phải được đính kèm với mọi yêu cầu. 
- Điều này đảm bảo server biết chính xác ai đang thực hiện yêu cầu tại bất kỳ thời điểm nào.

-- Xác thực hoạt động như thế nào trong một Web API phi trạng thái? -- 
- Client gửi thông tin xác thực với mỗi yêu cầu: Vì server không có bộ nhớ về các yêu cầu trước đó, client phải cung cấp bằng chứng về danh tính (credentials) với mỗi yêu cầu. 
- Các thông tin này có thể là:
    - Một token (như JWT — JSON Web Token)
    - Tên người dùng và mật khẩu (ít phổ biến hơn trong API vì lý do bảo mật)
    - Khóa API (API keys) hoặc các header xác thực khác
- Server xác thực thông tin: 
  - Server kiểm tra thông tin xác thực nhận được so với kho dữ liệu của nó (cơ sở dữ liệu, dịch vụ xác thực token, v.v.) để xác nhận danh tính người dùng.
- Cấp quyền hoặc Từ chối truy cập:
  - Nếu thông tin xác thực hợp lệ, server sẽ xử lý yêu cầu và gửi lại phản hồi (như dữ liệu hồ sơ người dùng).
  - Nếu thông tin xác thực không hợp lệ hoặc bị thiếu, server sẽ trả về mã trạng thái 401 Unauthorized, cho biết client phải xác thực đúng cách.

-- Các loại Xác thực trong Dịch vụ Web: -- 
- ASP.NET Core Web API hỗ trợ nhiều phương thức xác thực, mỗi phương thức được thiết kế để phù hợp với các nhu cầu bảo mật và kịch bản ứng dụng khác nhau. 
- Một số loại phổ biến bao gồm:
1. Xác thực cơ bản (Basic Authentication): 
  - Đây là một trong những cơ chế xác thực đơn giản nhất. 
  - Client gửi tên người dùng và mật khẩu với mọi yêu cầu HTTP, được mã hóa theo chuẩn Base64.
2. Xác thực dựa trên Token (Token-Based Authentication): 
  - Sử dụng một token (như JWT – JSON Web Token) mà client nhận được sau lần đăng nhập thành công đầu tiên. 
  - Token này sau đó được gửi cùng với các yêu cầu tiếp theo, thay vì tên người dùng và mật khẩu.
3. OAuth/OpenID Connect: 
  - OAuth và OpenID Connect là các tiêu chuẩn mở cho việc xác thực và phân quyền, cho phép người dùng xác thực bằng các dịch vụ của bên thứ ba, chẳng hạn như Google, Facebook và Microsoft.