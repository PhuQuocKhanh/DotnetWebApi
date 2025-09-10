-- Tại sao chúng ta cần Xác thực dựa trên Token trong ASP.NET Core Web API? --
- ASP.NET Core Web API là một framework hiện đại do Microsoft cung cấp để xây dựng các dịch vụ dựa trên HTTP trên nền tảng .NET Core.
- Các dịch vụ này được sử dụng rộng rãi bởi nhiều client khác nhau, chẳng hạn như:
  - Trình duyệt Web
  - Ứng dụng di động
  - Ứng dụng Desktop
  - Thiết bị IoT, v.v.
- Ngày nay, việc sử dụng Web API đang tăng lên nhanh chóng. Vì vậy, chúng ta nên biết cách phát triển Web API. Chỉ phát triển Web API thôi là chưa đủ nếu không có bảo mật. 
- Do đó, việc triển khai bảo mật trong các dịch vụ Web API của chúng ta cũng rất cần thiết, điều này đảm bảo rằng chỉ những người dùng hoặc ứng dụng đã được xác thực và cấp quyền mới có thể truy cập các tài nguyên được bảo vệ. 
- Nếu không có các biện pháp bảo mật phù hợp, dữ liệu nhạy cảm có thể bị lộ và người dùng trái phép có thể giành được quyền truy cập.
- Một trong những phương pháp ưa thích nhất để bảo mật Web API là Xác thực dựa trên Token, trong đó:
  1. Người dùng xác thực với server và nhận được một token đã được ký.
  2. Token này chứa đủ thông tin để nhận dạng người dùng và phải được gửi kèm theo mọi yêu cầu tiếp theo để truy cập các endpoint được bảo mật.

-- Xác thực dựa trên Token JWT là gì? -- 
- JSON Web Token (JWT) là một tiêu chuẩn được sử dụng rộng rãi cho việc xác thực dựa trên token. 
- Nó thường được sử dụng trong các hệ thống xác thực hiện đại vì nó nhỏ gọn, khép kín và dễ dàng xác minh.
https://jwt.io/
- Như bạn có thể thấy trên trang trên, một JWT thường chứa ba phần: 
  1. Header
  2. Payload
  3. Signature. 

1. JWT Header
- JWT Header chứa siêu dữ liệu (metadata) về loại token và thuật toán mã hóa được sử dụng để ký token. 
- Nó thường bao gồm hai trường:
  - alg: Chỉ định thuật toán ký (ví dụ: HMACSHA256, RSA, v.v.).
  - typ: Chỉ định loại token, thường là “JWT”.
- HMAC (Symmetric Key - Khóa đối xứng): Sử dụng cùng một secret để signing và xác minh.
- RSA (Asymmetric Key - Khóa bất đối xứng): Một khóa riêng tư (private key) được sử dụng để signing và một khóa công khai (public key) được sử dụng để xác minh.

2. JWT Payload
- Payload của JWT là phần rất quan trọng nơi dữ liệu được lưu trữ. 
- Payload bao gồm một tập hợp các claim, là các mệnh đề về một thực thể (thường là người dùng) và dữ liệu bổ sung. 
- Payload được biểu diễn dưới dạng một đối tượng JSON và có thể bao gồm các claim khác nhau:
  - iss (Issuer): Định danh bên đã phát hành JWT (ví dụ: tên miền của server xác thực).
  - sub (Subject): ID người dùng hoặc tên người dùng mà token đại diện.
  - aud (Audience): Người nhận (hoặc những người nhận) dự kiến của token. Đó có thể là tên miền của ứng dụng client.
  - exp (Expiration Time): Thời điểm token sẽ hết hạn. Thường được biểu diễn bằng thời gian Unix (còn gọi là Epoch time).
  - iat (Issued At): Thời điểm token được phát hành, cũng bằng thời gian Unix.
  - jti (JWT ID): Một mã định danh duy nhất cho token.

3. JWT Signature
- Chữ ký JWT được sử dụng để xác minh rằng dữ liệu của token không bị thay đổi. 
- Để tạo chữ ký, máy chủ sẽ kết hợp header và payload đã được mã hóa với một khóa bí mật và áp dụng thuật toán ký được chỉ định trong header.

-- How JWT Authentication Works in ASP.NET Core Web API: --
- Xác thực JWT (JSON Web Token) trong ASP.NET Core Web API thường bao gồm ba thành phần chính:
  - Client (Máy khách): Thực thể (như trình duyệt, ứng dụng di động, hoặc thiết bị IoT) yêu cầu truy cập các tài nguyên được bảo vệ.
  - Authorization Server (Máy chủ ủy quyền): Máy chủ chịu trách nhiệm xác thực client và cấp JWT.
  - Resource Server (Máy chủ tài nguyên): Máy chủ lưu trữ các tài nguyên được bảo vệ và xác minh JWT do client cung cấp.

-- Cách thức hoạt động của JWT Authentication trong ứng dụng client-server --
1. Gửi thông tin đăng nhập (Credential Submission)
- Quy trình bắt đầu khi người dùng (thường thông qua trình duyệt, ứng dụng di động, hoặc một client khác) gửi thông tin đăng nhập của họ (ví dụ: tên người dùng và mật khẩu) đến Client. 
- Client tiếp nhận thông tin này và gửi chúng đến Authorization Server để xác minh.

2. Authorization Server cấp Access Token và Refresh Token
- Authorization Server xác thực thông tin đăng nhập nhận được từ Client bằng cách kiểm tra chúng trong cơ sở dữ liệu. 
- Nếu thông tin hợp lệ, Authorization Server sẽ tạo ra hai loại token và gửi lại cho client:
  1. Access Token: 
    - Đây là một token có thời gian sống ngắn, được dùng để xác thực người dùng khi yêu cầu truy cập các tài nguyên bảo mật trên Resource Server. 
    - Nó thường hết hạn trong một thời gian ngắn (ví dụ: 15 phút đến 1 giờ).
  2. Refresh Token: 
    - Đây là một token có thời gian sống dài hơn (ví dụ: có giá trị trong vài ngày), dùng để làm mới Access Token khi nó hết hạn. 
    - Refresh Token cho phép người dùng tiếp tục sử dụng ứng dụng mà không cần đăng nhập lại.

3. Client nhận Token
- Client nhận Access Token và Refresh Token từ Authorization Server. 
- Các token này thường được client lưu trữ an toàn (trong local storage, session storage, hoặc cookies) để sử dụng cho các yêu cầu tiếp theo.

4. Client gửi yêu cầu đến Resource Server
- Đối với mỗi yêu cầu tiếp theo để truy cập các tài nguyên được bảo vệ trên Resource Server, Client phải đính kèm Access Token vào HTTP Authorization header của request. 
- Token này thường tuân theo schema Bearer: Authorization: Bearer <access_token>. 
- Điều này cho phép Client tự xác thực mà không cần gửi lại tên người dùng và mật khẩu.

5. Resource Server xác thực Token
- Resource Server nhận request và xác thực Access Token. 
- Quá trình xác thực này bao gồm việc kiểm tra chữ ký của token, thời gian hết hạn, issuer (bên phát hành), và audience (đối tượng) để đảm bảo token vẫn còn hiệu lực. 
- Nếu Access Token hợp lệ, Resource Server sẽ xử lý yêu cầu và trả về tài nguyên được yêu cầu cho client (ví dụ: hồ sơ người dùng, danh sách đơn hàng, v.v.).

6. Từ chối truy cập hoặc làm mới Token
- Nếu Access Token đã hết hạn hoặc không hợp lệ, Resource Server sẽ từ chối truy cập vào tài nguyên được yêu cầu, thường trả về phản hồi 401 Unauthorized. 
- Trong trường hợp này, Client có thể gửi Refresh Token đến Authorization Server để nhận một Access Token mới (và có thể cả một Refresh Token mới). Client sau đó sẽ sử dụng Access Token mới cho các yêu cầu trong tương lai.

