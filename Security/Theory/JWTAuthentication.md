-- Tại Sao Chúng Ta Cần Xác Thực Dựa Trên Token trong ASP.NET Core Web API? -- 
- ASP.NET Core Web API là một framework hiện đại, mạnh mẽ do Microsoft phát triển để xây dựng các dịch vụ dựa trên HTTP trên nền tảng .NET Core. Các Web API này đóng vai trò là dịch vụ backend có thể được sử dụng bởi nhiều loại client khác nhau, 
- bao gồm nhưng không giới hạn ở:
  - Trình duyệt Web: Các Ứng dụng trang đơn (SPA) dựa trên JavaScript như Angular, React, hoặc Blazor.
  - Ứng dụng di động: Các ứng dụng Android và iOS giao tiếp với dịch vụ backend.
  - Ứng dụng Desktop: Phần mềm máy tính cho Windows, Mac, hoặc đa nền tảng.
  - Thiết bị IoT: Các thiết bị thông minh và cảm biến tương tác với API trên cloud để lấy dữ liệu và điều khiển.
- Khi việc áp dụng Web API ngày càng phát triển nhanh chóng trong các ngành công nghiệp, nhu cầu xây dựng các API an toàn, có khả năng mở rộng và dễ bảo trì đã trở nên cực kỳ quan trọng. 
- Việc chỉ phát triển các API có chức năng thôi là chưa đủ; bảo mật các dịch vụ này là điều cần thiết để bảo vệ dữ liệu nhạy cảm và duy trì lòng tin.

-- Tầm Quan Trọng của Bảo Mật trong Web API -- 
- Web API thường expose (phơi bày) các logic nghiệp vụ quan trọng và dữ liệu nhạy cảm.
- Nếu không có cơ chế bảo mật phù hợp:
  1. Truy cập trái phép (Unauthorized Access): 
    - Kẻ tấn công hoặc người dùng độc hại có thể giành được quyền truy cập trái phép vào các tài nguyên riêng tư hoặc dữ liệu nhạy cảm.
  2. Rủi ro rò rỉ dữ liệu (Data Breach Risks): 
    - Thông tin nhạy cảm, bao gồm chi tiết người dùng, dữ liệu tài chính, hoặc bí mật kinh doanh, có thể bị xâm phạm.
  3. Lạm dụng dịch vụ (Service Abuse): 
    - Người dùng không được phép có thể khai thác các API, dẫn đến hỏng dữ liệu, từ chối dịch vụ, hoặc gian lận.
- Do đó, việc thực thi xác thực (authentication - xác minh người dùng hoặc client là ai) và phân quyền (authorization - họ được phép làm gì) là một yêu cầu nền tảng cho bất kỳ Web API nào. 
- Một trong những phương pháp ưa thích nhất để bảo mật Web API là Xác thực dựa trên Token (Token-Based Authentication).

-- Xác Thực Dựa Trên Token JWT là gì? -- 
- JSON Web Token (JWT) là một phương pháp được chuẩn hóa và sử dụng rộng rãi cho việc xác thực dựa trên token trong các ứng dụng web và API hiện đại.
- Nó cho phép trao đổi thông tin an toàn giữa các bên thông qua một token nhỏ gọn, khép kín (self-contained) có thể dễ dàng được xác minh mà không cần truy vấn cơ sở dữ liệu lặp đi lặp lại. 
- JWT được sử dụng rộng rãi vì chúng:
  1. Nhỏ gọn (Compact): 
    - Token có kích thước nhỏ, cho phép nó dễ dàng được gửi qua URL hoặc HTTP header (ví dụ: Authorization: Bearer <token>).
  2. Khép kín (Self-Contained): 
    - Tất cả thông tin cần thiết về người dùng (hay còn gọi là subject), như bên phát hành, ID người dùng, vai trò và dữ liệu tùy chỉnh, đều được lưu trữ ngay trong chính token, giảm nhu cầu tra cứu cơ sở dữ liệu nhiều lần.
  3. Dễ xác minh (Easy to verify): 
    - Token có thể được xác thực nhanh chóng bằng cơ chế chữ ký.
- Trong một ASP.NET Core Web API, sau khi người dùng đăng nhập và chứng minh danh tính, máy chủ của bạn sẽ cấp một JWT.
- Trong các yêu cầu tiếp theo, client sẽ gửi token đó trong header Authorization: Bearer <token>.
- Middleware của API sẽ đọc token, xác minh chữ ký và các claim của nó, và nếu mọi thứ đều hợp lệ, sẽ cấp quyền truy cập vào các endpoint được bảo vệ.

-- Cấu trúc của một JWT -- 
- Một token JWT điển hình bao gồm ba phần, mỗi phần được phân tách bằng dấu chấm (.):
  1. Header
  2. Payload
  3. Signature
- Ba phần này được mã hóa riêng biệt bằng Base64Url và được nối với nhau bằng dấu chấm. 
- Ví dụ: <Base64Url-Header>.<Base64Url-Payload>.<Base64Url-Signature>.

1. JWT Header:
- Header cung cấp thông tin về cách JWT được xây dựng, đặc biệt là thuật toán nào đã được sử dụng để ký nó. Nó chủ yếu chỉ định:
- Loại token (thường là JWT).
- Thuật toán ký được sử dụng để tạo chữ ký của token (ví dụ: HS256 cho HMAC SHA256, RS256 cho RSA SHA256).
- Header là một đối tượng JSON đơn giản. Ví dụ:
    {
    "alg": "HS256",
    "typ": "JWT"
    }
- alg (Algorithm): Định nghĩa thuật toán mã hóa được sử dụng để ký token.
- typ (Type): Cho biết loại token, luôn là "JWT" đối với JSON Web Token.
- Khi API của bạn nhận được một token, nó sẽ nhìn vào alg để biết cách xác minh chữ ký. 
- Đối tượng JSON này sau đó được mã hóa Base64Url để trở thành phần đầu tiên của JWT.

1. JWT Payload: 
- Payload chứa các claim, là những mẩu thông tin về người dùng hoặc thực thể, cũng như bất kỳ siêu dữ liệu bổ sung nào mà máy chủ muốn bao gồm. 
- Các claim có thể là:
- Registered claims: Các trường được tiêu chuẩn hóa bởi đặc tả JWT để đảm bảo khả năng tương tác. Ví dụ:
  - iss (Issuer): Bên đã phát hành token (ví dụ: URL máy chủ xác thực của bạn).
  - sub (Subject): Định danh cho người dùng (ví dụ: User ID, Username, hoặc Email).
  - aud (Audience): Đối tượng dự kiến nhận token, chẳng hạn như ứng dụng frontend của bạn.
  - exp (Expiration Time): Thời điểm token hết hạn, được biểu thị bằng Unix time.
  - iat (Issued At): Thời điểm token được phát hành.
  - jti (JWT ID): Một định danh duy nhất cho token để chống sử dụng lại.
- Custom claims: Dữ liệu dành riêng cho ứng dụng như vai trò người dùng, quyền hạn, hoặc ID hồ sơ.
- Ví dụ về Payload JSON:
    {
    "iss": "http://example.com",
    "sub": "user12345",
    "role": "admin",
    "profileId": "PID123"
    }
- Giống như header, payload JSON được mã hóa Base64Url để trở thành phần thứ hai của JWT.
- Lưu ý: Không bao giờ đưa dữ liệu nhạy cảm (như mật khẩu) vào trong payload. 
- Mặc dù token được ký và bảo vệ khỏi việc giả mạo, nội dung của nó không được mã hóa và có thể được xem bởi bất kỳ ai có token.

- JWT Signature:
- Chữ ký được sử dụng để xác minh rằng token được tạo ra bởi người gửi và nội dung của nó không bị thay đổi. 
- Nó bảo vệ token khỏi việc giả mạo. Chữ ký được tạo bằng cách:
  1. Lấy header và payload đã được mã hóa Base64Url.
  2. Nối chúng lại với một dấu chấm . ở giữa.
  3. Áp dụng một thuật toán ký mật mã hóa sử dụng một khóa bí mật (secret key) hoặc khóa riêng tư (private key).
- Ví dụ, với thuật toán HMAC SHA256 (HS256):
  - HMACSHA256(base64UrlEncode(header) + "." + base64UrlEncode(payload), secretKey)
- Chữ ký cũng được mã hóa Base64Url để tạo thành phần thứ ba của JWT.

-- Cách Hoạt Động Của Xác Thực JWT trong Ứng Dụng Client-Server -- 
Hãy phân tích luồng xác thực từng bước để hiểu cách các thành phần này tương tác.
- Bước 1: Người dùng Đăng nhập và Gửi thông tin xác thực
  - Quá trình bắt đầu khi người dùng đăng nhập. 
  - Client gửi thông tin xác thực (username/password) trong một yêu cầu an toàn (thường là HTTPS POST) đến máy chủ xác thực (Authorization Server).
- Bước 2: Máy chủ Xác thực xác thực và cấp Token
  - Authorization Server kiểm tra thông tin xác thực. Nếu hợp lệ, máy chủ sẽ tạo ra hai loại token:
    1. Access Token: 
      - Một JWT có thời hạn ngắn (thường từ 15 phút đến 1 giờ) mà client dùng để xác thực các yêu cầu API đến Resource Server.
    2. Refresh Token: 
      - Một token có thời hạn dài hơn (vài ngày hoặc vài tuần) được sử dụng để lấy Access Token mới mà không cần người dùng đăng nhập lại. 
      - Refresh Token được lưu trữ an toàn và chỉ được sử dụng khi Access Token hết hạn.

  - Tại sao lại cần hai token?
    - Access Token có thời hạn ngắn để tăng cường bảo mật; nếu bị lộ, thiệt hại sẽ bị giới hạn. 
    - Refresh Token cho phép trải nghiệm người dùng liền mạch bằng cách làm mới quyền truy cập mà không yêu cầu người dùng nhập lại thông tin đăng nhập thường xuyên.

- Bước 3: Client nhận và lưu trữ Token một cách an toàn
  - Client nhận cả Access Token và Refresh Token và lưu trữ chúng một cách an toàn.
  - Ứng dụng Web: 
    - Thường sử dụng cookie HTTP-only an toàn cho refresh token và có thể dùng bộ nhớ (memory), localStorage, hoặc sessionStorage cho access token (mỗi cách đều có ưu và nhược điểm về bảo mật và chống tấn công XSS).
  - Ứng dụng di động: 
    - Có thể sử dụng các vùng lưu trữ an toàn do HĐH cung cấp (ví dụ: Keychain cho iOS, Keystore cho Android).

- Bước 4: Client thực hiện các yêu cầu đã được xác thực đến Resource Server
  - Đối với bất kỳ lệnh gọi API nào đến các endpoint được bảo vệ, client sẽ đính kèm Access Token trong HTTP header Authorization bằng cách sử dụng scheme Bearer.
  - Ví dụ HTTP header: Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9…

- Bước 5: Resource Server xác thực Token
- Khi nhận được yêu cầu, Resource Server (API của bạn) sẽ xác thực Access Token bằng cách:
  - Kiểm tra chữ ký số để đảm bảo token là thật và không bị giả mạo.
  - Kiểm tra thời gian hết hạn (claim exp) để xác nhận token vẫn còn hiệu lực.
  - Xác thực các claim issuer (iss) và audience (aud) để đảm bảo token được cấp bởi một nguồn đáng tin cậy và dành cho máy chủ này.
- Nếu tất cả các bước xác thực đều thành công, Resource Server sẽ xử lý yêu cầu.

- Bước 6: Truy cập bị từ chối hoặc Gia hạn Token
- Nếu Access Token đã hết hạn hoặc không hợp lệ, Resource Server sẽ trả về lỗi 401 Unauthorized. 
- Lúc này, client có thể sử dụng Refresh Token để yêu cầu một Access Token mới từ Authorization Server mà không cần người dùng đăng nhập lại.
- Authorization Server xác minh Refresh Token. 
- Nếu hợp lệ, nó sẽ cấp một Access Token mới (và thường là một Refresh Token mới - một quy trình được gọi là xoay vòng refresh token (refresh token rotation)), đồng thời vô hiệu hóa Refresh Token cũ. 
- Client giờ có thể tiếp tục thực hiện các yêu cầu bằng Access Token mới.

-- Triển khai Xác thực JWT trong ASP.NET Core Web API -- 
- Xác thực JWT trong ASP.NET Core Web API thường liên quan đến ba thành phần chính làm việc cùng nhau:
  - Client: Ứng dụng mà người dùng tương tác (trình duyệt, app di động).
  - Authorization Server: Máy chủ xử lý quá trình xác thực. Nó xác minh thông tin đăng nhập và cấp token JWT.
  - Resource Server: Là API hoặc backend chứa các tài nguyên được bảo vệ mà client muốn truy cập. Nó xác thực các JWT để đảm bảo yêu cầu đến từ các client đã được xác thực.
- Trong ví dụ của chúng ta, chúng ta sẽ tạo một dự án Web API duy nhất bao gồm cả Authorization Server và Resource Server. 
- Chúng ta sẽ tạo hai controller: một controller xử lý các chức năng của máy chủ xác thực, và một controller khác cung cấp các API endpoint được bảo vệ bằng xác thực JWT để client sử dụng.    