-- Xác thực SSO là gì? -- 
- Xác thực SSO (Single Sign-On) là một quy trình xác thực người dùng tập trung, cho phép người dùng đăng nhập một lần bằng một bộ thông tin đăng nhập duy nhất (chẳng hạn như tên người dùng và mật khẩu) và truy cập vào nhiều ứng dụng hoặc hệ thống khác nhau mà không cần phải xác thực lại cho từng ứng dụng. 
- Bằng cách tập trung quá trình xác thực trên một máy chủ chuyên dụng, đáng tin cậy, SSO nâng cao cả trải nghiệm người dùng lẫn tính bảo mật của tổ chức.

-- Các điểm chính về xác thực SSO -- 
- Xác thực tập trung: 
  - Người dùng chỉ cần xác thực một lần thông qua một Authentication Server (máy chủ xác thực) chuyên dụng hoặc Identity Provider (IdP) (nhà cung cấp định danh), nơi quản lý thông tin đăng nhập và trạng thái xác thực của người dùng.
- Truy cập dựa trên token: 
  - Sau khi xác thực thành công, hệ thống sẽ cấp một token bảo mật, thường là JSON Web Token (JWT).
  - Token này mang các claim (thông tin) của người dùng như ID, vai trò, email và được sử dụng để truy cập các dịch vụ khác nhau mà không cần xác thực lại.
- Tăng cường bảo mật và quản lý: 
  - Việc tập trung xác thực cho phép áp dụng các biện pháp bảo mật mạnh mẽ hơn như xác thực đa yếu tố (MFA), chính sách mật khẩu chuẩn hóa và quản lý tài khoản tập trung (ví dụ: thu hồi quyền truy cập ngay lập tức khi nhân viên nghỉ việc).

-- SSO hoạt động như thế nào? --
- Chúng ta hãy phân tích một luồng SSO điển hình trong môi trường ASP.NET Core Web API, nơi JSON Web Tokens (JWTs) được sử dụng để xác thực an toàn, không trạng thái. 
- Quá trình này bao gồm một số thành phần làm việc cùng nhau như sau:
  1. Authentication Server (Identity Provider): 
    - chịu trách nhiệm xác minh thông tin đăng nhập của người dùng, cấp token JWT và quản lý các token SSO.
  2. Resource Server: 
    - lưu trữ các tài nguyên được bảo vệ (API, dữ liệu, dịch vụ) yêu cầu xác thực người dùng. Các ứng dụng khách sẽ sử dụng các dịch vụ này.
  3. Client Applications:
    - nhiều ứng dụng liên quan mà người dùng muốn truy cập bằng SSO.

-- Quy trình từng bước của xác thực SSO --
1. Bước 1: Đăng nhập ban đầu tại Ứng dụng khách một
- Mục tiêu chính của bước này là xác thực danh tính của người dùng lần đầu tiên trong hệ thống bằng cách xác minh thông tin đăng nhập của họ. 
- Điều này thiết lập một phiên đáng tin cậy và tạo ra một token truy cập xác nhận trạng thái xác thực của người dùng.
  - Hành động:
    - Hành động của người dùng: 
      - Người dùng truy cập ứng dụng khách đầu tiên (Ứng dụng một) và gửi thông tin đăng nhập của họ, chẳng hạn như tên người dùng và mật khẩu, qua một form đăng nhập an toàn.
    - Giao tiếp với Server: 
      - Ứng dụng một gửi thông tin đăng nhập này một cách an toàn đến Authentication Server để tạo token.
    - Tạo Token: 
      - Authentication Server xác minh thông tin đăng nhập. Sau khi xác thực thành công, nó tạo ra một token JWT chứa các thông tin chính của người dùng (chẳng hạn như ID người dùng, vai trò và email).
2. Bước 2: Chuyển giao Token và lưu trữ an toàn
- Bước này nhằm mục đích chuyển giao token xác thực một cách an toàn đến ứng dụng khách và lưu trữ nó một cách an toàn trên phía client để duy trì phiên đã xác thực cho các yêu cầu trong tương lai.
  - Hành động:
    - Chuyển giao Token: 
      - Authentication Server trả về token JWT cho Ứng dụng một sau khi xác thực thành công.
    - Lưu trữ an toàn: 
      - Ứng dụng một lưu trữ token này một cách an toàn trong trình duyệt của người dùng, thường sử dụng HTTP-only cookies hoặc session storage. 
      - Việc làm này giúp ngăn các script phía client truy cập vào token và giảm thiểu rủi ro của các cuộc tấn công XSS.
- Token JWT hiện được sử dụng để xác minh danh tính của người dùng cho tất cả các yêu cầu tài nguyên tiếp theo trong Ứng dụng một.
3. Bước 3: Bắt đầu SSO cho một ứng dụng khác
- Bước này cho phép người dùng truy cập một ứng dụng khách thứ hai mà không cần phải nhập lại thông tin đăng nhập. 
- Điều này được thực hiện bằng cách yêu cầu một token SSO đặc biệt từ Authentication Server, token này hoạt động trên nhiều ứng dụng.
  - Hành động:
    - Điều hướng người dùng: 
      - Khi người dùng muốn truy cập Ứng dụng khách hai, Ứng dụng một cung cấp một liên kết hoặc nút SSO.
    - Chuyển tiếp Token: 
      - Nhấp vào liên kết SSO này sẽ kích hoạt Ứng dụng một chuyển tiếp token JWT đã lưu trữ đến Authentication Server. 
      - Yêu cầu này hướng dẫn server tạo một token SSO mới, được thiết kế đặc biệt để xác thực chéo ứng dụng.
4. Bước 4: Tạo, Lưu trữ và Trả về Token SSO
- Bước này tập trung vào việc xác thực token JWT hiện có và tạo ra một token SSO an toàn, tạm thời có thể được sử dụng để xác thực người dùng trên các ứng dụng khách khác.
- Hành động:
  - Xác thực và Tạo Token: 
    - Authentication Server kiểm tra tính hợp lệ của token JWT đến. Nếu nó hợp lệ, server sẽ tạo ra một token SSO duy nhất, có thời gian tồn tại ngắn (chẳng hạn như một GUID hoặc một chuỗi an toàn mã hóa).
  - Lưu trữ: 
    - Token SSO, cùng với thông tin người dùng liên quan và thời gian hết hạn của nó, được lưu trữ an toàn trong cơ sở dữ liệu hoặc bộ nhớ cache trong bộ nhớ để theo dõi và xác thực.
  - Phản hồi: Token SSO mới được tạo được trả về cho Ứng dụng một.
5. Bước 5: Chuyển hướng người dùng đến Ứng dụng hai
- Bước này nhằm mục đích chuyển người dùng từ ứng dụng đầu tiên sang ứng dụng thứ hai, chuyển giao token SSO một cách an toàn và có thể xác minh được.
  - Hành động:
    - Chuyển hướng người dùng: 
      - Khi Ứng dụng khách một nhận được token SSO từ Authentication Server, Ứng dụng một sẽ chuyển hướng người dùng đến Ứng dụng khách hai. 
      - Trong quá trình này, token SSO được đính kèm như một tham số truy vấn trong URL hoặc trong một header HTTP, cho phép Ứng dụng hai bắt đầu quá trình xác thực.
6. Bước 6: Xác thực Token SSO tại Ứng dụng hai
- Bước này đảm bảo rằng chỉ những người dùng hợp pháp, đã được xác thực mới có thể truy cập vào các tài nguyên được bảo vệ trong Ứng dụng hai bằng cách xác thực token SSO với Authentication Server.
  - Hành động:
    - Xác thực Token SSO: 
      - Ứng dụng hai nhận được token SSO và gửi nó đến Authentication Server để xác thực.
    - Nếu hợp lệ: 
      - Authentication Server xác minh tính xác thực của token SSO, truy xuất thông tin người dùng tương ứng và cấp một token JWT mới dành riêng cho Ứng dụng hai. 
      - Ứng dụng hai lưu trữ JWT mới này và cấp quyền truy cập vào các tài nguyên được bảo vệ cho người dùng.
    - Nếu không hợp lệ: 
      - Nếu token SSO đã hết hạn, không hợp lệ hoặc đã được sử dụng, Ứng dụng hai sẽ từ chối quyền truy cập, thường chuyển hướng người dùng đến trang đăng nhập hoặc hiển thị thông báo từ chối truy cập.
- Lưu ý quan trọng: Token SSO thường được thiết kế để sử dụng một lần và được cấu hình để hết hạn nhanh chóng. Thiết kế này bảo vệ chống lại các cuộc tấn công phát lại và truy cập trái phép, đảm bảo tính toàn vẹn của quy trình SSO.

-- Xác thực bên ngoài (External Authentication), như Google hoặc Facebook, có giống với Xác thực SSO không? -- 
- Các nhà cung cấp xác thực bên ngoài (chẳng hạn như Google, Facebook, Microsoft và Twitter) cho phép người dùng đăng nhập vào ứng dụng của bạn bằng thông tin đăng nhập của họ từ các nhà cung cấp đáng tin cậy này. 
- Điều này thường được gọi là "Đăng nhập xã hội" (Social Login).
- Các nhà cung cấp bên ngoài này sử dụng các giao thức OAuth 2.0/OpenID Connect đằng sau hậu trường để xác thực người dùng và cung cấp thông tin danh tính.
- Từ một góc độ rộng, việc sử dụng đăng nhập bằng Google hoặc Facebook có thể được coi là một hình thức của SSO, bởi vì người dùng xác thực một lần với nhà cung cấp bên ngoài và có thể truy cập nhiều ứng dụng tin tưởng nhà cung cấp đó.
- Tuy nhiên, SSO trong môi trường doanh nghiệp thường đề cập đến xác thực tập trung trong một tổ chức hoặc giữa các hệ thống doanh nghiệp đáng tin cậy, sử dụng các giao thức như SAML hoặc OAuth 2.0/OpenID Connect (JWT).
- Vì vậy:
  - Xác thực bên ngoài qua Google hoặc Facebook là một loại SSO dành cho người tiêu dùng, sử dụng các nhà cung cấp danh tính của bên thứ ba.
  - Tuy nhiên, không phải tất cả các triển khai SSO đều liên quan đến đăng nhập bên ngoài hoặc xã hội; nhiều SSO doanh nghiệp là nội bộ và được sử dụng để giao tiếp giữa các hệ thống doanh nghiệp đáng tin cậy.