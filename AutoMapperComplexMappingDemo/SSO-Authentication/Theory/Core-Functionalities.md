-- Các Chức năng cốt lõi của Máy chủ Xác thực --
- Máy chủ Xác thực này sẽ xử lý các hoạt động bảo mật thiết yếu như Đăng ký Người dùng, Đăng nhập, Tạo Mã thông báo SSO và Xác thực Mã thông báo SSO, vốn là nền tảng cho việc xác thực Đăng nhập một lần (SSO).
1. Đăng ký người dùng
- Đây là quá trình người dùng mới tạo tài khoản trong hệ thống. 
- Nó bao gồm việc thu thập thông tin chi tiết của người dùng như tên người dùng, email và mật khẩu, sau đó lưu trữ chúng một cách an toàn trong cơ sở dữ liệu.
- ASP.NET Core Identity tự động xử lý việc tạo người dùng, băm mật khẩu và lưu trữ dữ liệu.
- Việc kiểm tra hợp lệ (validation) chính xác đảm bảo rằng dữ liệu là chính xác và an toàn trước khi tài khoản được tạo.

2. Đăng nhập người dùng
- Sau khi đăng ký, người dùng có thể đăng nhập bằng cách cung cấp tên người dùng và mật khẩu của họ.
- Hệ thống xác minh thông tin đăng nhập so với dữ liệu đã lưu trữ.
- Khi xác thực thành công, máy chủ sẽ tạo một mã JWT token.
- JWT token này đóng vai trò là bằng chứng xác thực và được yêu cầu để truy cập các điểm cuối API được bảo vệ mà không cần đăng nhập lại nhiều lần.

3. Tạo Mã thông báo SSO
- Sau khi đăng nhập, người dùng có thể yêu cầu một mã thông báo SSO.
- Máy chủ sẽ tạo một mã thông báo duy nhất, có giới hạn thời gian (thường là một GUID) và lưu trữ nó cùng với các siêu dữ liệu (metadata), bao gồm thời gian hết hạn, thông tin người dùng và trạng thái sử dụng.
- Mã thông báo SSO này cho phép người dùng xác thực trên nhiều dịch vụ mà không cần nhập lại thông tin đăng nhập của họ.
- Nó cải thiện trải nghiệm người dùng và tăng cường bảo mật bằng cách hạn chế việc sử dụng lại mã thông báo và đặt thời gian hết hạn.

4. Xác thực Mã thông báo SSO
- Khi người dùng trình bày một mã thông báo SSO để truy cập một dịch vụ khác:
  - Máy chủ kiểm tra xem mã thông báo đó:
    - Có tồn tại trong cơ sở dữ liệu không,
    - Chưa được sử dụng,
    - Vẫn còn trong thời gian hiệu lực.
  - Nếu hợp lệ, mã thông báo sẽ được đánh dấu là đã sử dụng và một mã JWT token mới sẽ được cấp để truy cập.
  - Nếu không hợp lệ hoặc đã hết hạn, quyền truy cập sẽ bị từ chối.