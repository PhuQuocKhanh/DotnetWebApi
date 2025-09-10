-- Refresh Token trong Xác thực dựa trên JWT là gì? --
- Refresh Token là một loại chứng danh (credential) được sử dụng để lấy một access token mới mà không yêu cầu người dùng phải đăng nhập hoặc xác thực lại.
- Trong phương thức xác thực dựa trên JWT, access token (chính là các JWT) thường có thời gian sống ngắn. 
- Một khi access token hết hạn, client không thể truy cập vào các tài nguyên được bảo vệ nữa.

- Refresh token là một token có thời gian sống dài, cho phép client yêu cầu một access token mới mà không cần người dùng xác thực lại. 
- Bản thân refresh token không phải là một JWT; nó thường là một chuỗi ký tự ngẫu nhiên được lưu trữ an toàn ở phía máy chủ (server-side) và được cấp cho client trong quá trình xác thực ban đầu.

-- Cách Refresh Token hoạt động -- 
- Việc sử dụng refresh token giúp tăng cường bảo mật và trải nghiệm người dùng bằng cách cho phép các phiên làm việc (sessions) được kéo dài mà không yêu cầu người dùng phải đăng nhập nhiều lần.
- Để hiểu rõ hơn về cách Refresh Token hoạt động trong các Dịch vụ Restful (Restful Services), hãy xem sơ đồ sau:
- Sơ đồ trên minh họa quy trình sử dụng refresh token trong các Web API để quản lý và làm mới access token cho người dùng đã được xác thực. Chúng ta hãy tìm hiểu quy trình này từng bước một:

1. Bước 1: Yêu cầu Cấp quyền từ Người dùng (User Authorization Request)
  - Ứng dụng khách (client application), thường là một ứng dụng web hoặc di động, gửi một yêu cầu đến máy chủ ủy quyền (authorization server) để xác thực người dùng và xin quyền truy cập vào tài nguyên được bảo vệ. 
  - Quá trình này thường bao gồm việc gửi thông tin xác thực của người dùng (ví dụ: tên người dùng và mật khẩu) hoặc sử dụng OAuth với các loại cấp phép (authorization grant), tức là xác thực qua một bên thứ ba.

2. Bước 2: Cấp Access Token và Refresh Token (Access Token and Refresh Token Issuance)
  - Máy chủ ủy quyền xác thực thông tin của người dùng. Sau khi xác thực thành công, máy chủ sẽ cấp hai loại token:
    - Access Token: Một token có thời gian sống ngắn để truy cập các tài nguyên được bảo vệ.
    - Refresh Token: Một token có thời gian sống dài để yêu cầu access token mới sau khi access token cũ hết hạn.
  - Cả hai token này được gửi trả về cho ứng dụng khách.

3. Bước 3-4: Truy cập Tài nguyên được Bảo vệ (Accessing Protected Resources)
  - Client sử dụng access token để truy cập các tài nguyên được bảo vệ trên máy chủ tài nguyên (resource server). 
  - Máy chủ tài nguyên sẽ xác thực access token để đảm bảo nó hợp lệ, chưa hết hạn và đã được ký (signed) đúng cách. 
  - Nếu token hợp lệ, máy chủ sẽ cấp quyền truy cập vào tài nguyên được yêu cầu.

4. Bước 5-6: Access Token Hết hạn (Access Token Expiry)
  - Theo thời gian, access token sẽ hết hạn (vì nó được cố ý thiết lập thời gian sống ngắn vì lý do bảo mật). 
  - Máy chủ tài nguyên sẽ trả về lỗi Invalid Token Error khi client gửi một access token đã hết hạn hoặc không hợp lệ.
  - Client không thể tiếp tục truy cập các tài nguyên được bảo vệ bằng token đã hết hạn.

5. Bước 7: Làm mới Access Token (Refreshing the Access Token)
  - Ứng dụng khách gửi refresh token đến máy chủ ủy quyền để yêu cầu một access token mới. 
  - Refresh token này sẽ được máy chủ ủy quyền xác thực để đảm bảo nó vẫn hợp lệ, chưa hết hạn hoặc chưa bị thu hồi (revoked).

6. Bước 8: Cấp Access Token và Refresh Token mới (New Access Token and Refresh Token Issued)
  - Sau khi xác thực thành công, máy chủ ủy quyền sẽ cấp:
    - Một access token mới để truy cập các tài nguyên được bảo vệ.
    - Tùy chọn, một refresh token mới để thay thế cái cũ (tùy thuộc vào cách triển khai).
    - Client lưu trữ các token mới và tiếp tục truy cập các tài nguyên được bảo vệ.

- Refresh token thường có thời gian sống dài, trong khi access token có thời gian hết hạn ngắn hơn để đảm bảo an ninh.
- Cơ chế này cũng hạn chế việc lộ thông tin xác thực của người dùng và giảm thiểu rủi ro liên quan đến việc sử dụng các access token có thời gian sống dài.

-- Tại sao không sử dụng access token có thời gian sống dài thay vì dùng refresh token? --
1. Cập nhật động các "claims" của access token
- Thách thức với long-lived access token
  - Access token là các token tự chứa (self-contained), mang tất cả thông tin cần thiết về người dùng (gọi là các claims) tại thời điểm cấp phát.
  - Ví dụ: bạn cấp cho một user tên Anurag một access token có giá trị trong một tháng với claim là vai trò "User". Token này sẽ chứa thông tin vai trò "User" của anh ấy.
  - Tuy nhiên, nếu ba ngày sau, vai trò của Anurag thay đổi thành "Admin", access token hiện tại vẫn chứa thông tin cũ ("User") và không thể cập nhật.
  - Việc yêu cầu người dùng đăng nhập lại để nhận token mới chứa thông tin cập nhật là không khả thi trong nhiều trường hợp.

- Giải pháp với refresh token
  - Bạn có thể khắc phục hạn chế này bằng cách cấp access token có thời gian sống ngắn (ví dụ: 30 phút) cùng với refresh token có thời gian sống dài (ví dụ: một năm). 
  - Khi vai trò của Anurag thay đổi:
    - Access token hiện tại của anh ta sẽ sớm hết hạn (sau 30 phút).
    - Khi token này hết hạn, ứng dụng client sẽ sử dụng refresh token để yêu cầu một access token mới.
    - Server xác thực sẽ tạo access token mới, bao gồm vai trò đã cập nhật của Anurag ("Admin").
    - Cách tiếp cận này đảm bảo rằng các claims trong access token luôn được cập nhật, phản ánh bất kỳ thay đổi nào trong vai trò hoặc quyền hạn của người dùng mà không cần họ đăng nhập lại.

2. Thu hồi quyền truy cập một cách hiệu quả
- Thách thức với long-lived access token
- Khi người dùng có access token với thời gian sống dài, họ có thể truy cập tài nguyên trên server miễn là token đó chưa hết hạn. 
- Không có cách nào chuẩn để thu hồi một access token đã cấp, trừ khi server xác thực triển khai logic tùy chỉnh để lưu trữ tất cả các token đã tạo trong cơ sở dữ liệu và kiểm tra cho mỗi yêu cầu. Điều này sẽ làm tăng độ phức tạp và gây ảnh hưởng đến hiệu suất.

- Giải pháp với refresh token
- Refresh token cung cấp một cơ chế thu hồi quyền truy cập dễ quản lý hơn:
  1. Điểm kiểm soát duy nhất: 
    - Refresh token được lưu trữ ở phía server (ví dụ: trong cơ sở dữ liệu). 
    - Quản trị viên hệ thống có thể thu hồi quyền truy cập bằng cách xóa refresh token khỏi cơ sở dữ liệu bất cứ lúc nào.
  2. Hiệu quả của việc thu hồi: 
    - Khi một refresh token bị thu hồi, người dùng không thể sử dụng nó để lấy access token mới. 
    - Access token hiện có sẽ hết hạn sau một thời gian ngắn (ví dụ: 30 phút) và không thể được làm mới, từ đó chấm dứt quyền truy cập một cách hiệu quả.
- Phương pháp này cung cấp một cách an toàn và hiệu quả để thu hồi quyền truy cập của người dùng mà không cần theo dõi mọi access token đã cấp.

3. Trải nghiệm người dùng tốt hơn mà không cần xác thực thường xuyên
- Thách thức với long-lived access token
  - Nếu chỉ phụ thuộc vào access token với thời gian sống dài (ví dụ: một tháng), bạn có thể tránh được việc người dùng phải đăng nhập lại thường xuyên, nhưng cách tiếp cận này lại làm giảm bảo mật và tính linh hoạt. 
  - Hơn nữa, việc duy trì tính toàn vẹn của phiên (session integrity) trở nên khó khăn trong các môi trường mà việc thu hồi token không đơn giản.

- Giải pháp với refresh token
  - Kết hợp refresh token cải thiện đáng kể trải nghiệm người dùng bằng cách giảm thiểu việc họ phải nhập lại thông tin đăng nhập:
    - Làm mới token liền mạch: 
      - Người dùng chỉ cần xác thực một lần, nhận cả access token và refresh token. 
      - Ứng dụng client có thể tự động sử dụng refresh token để lấy access token mới khi cần mà không làm gián đoạn người dùng.
    - Phiên kéo dài: 
      - Refresh token có thời gian sống dài hơn (ví dụ: một năm), cho phép người dùng duy trì phiên đăng nhập trong thời gian dài mà không cần phải đăng nhập lại thường xuyên.
    - Giảm rủi ro lộ thông tin: 
      - Bằng cách tránh việc truyền lại tên người dùng và mật khẩu nhiều lần, rủi ro bị lộ thông tin đăng nhập sẽ được giảm thiểu.
- Trong các ứng dụng web, ứng dụng di động và SPA (Single-Page Applications), refresh token mang lại trải nghiệm liền mạch bằng cách giảm tần suất người dùng phải đăng nhập, trong khi vẫn duy trì được tính bảo mật và linh hoạt của xác thực dựa trên JWT.