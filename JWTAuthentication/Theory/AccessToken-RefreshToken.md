-- Access Token và Refresh Token trong Xác thực JWT là gì? -- 
1. Access Token:
  - Cấp quyền truy cập vào các tài nguyên được bảo vệ (API) trong một khoảng thời gian giới hạn.
  - Có thời gian sống ngắn (ví dụ: từ 15 phút đến 1 giờ), không trạng thái (stateless) và tự chứa (self-contained).
   - Được đính kèm trong các yêu cầu API (thường ở header Authorization) để xác thực và cấp quyền truy cập.
2. Refresh Token:
  - Dùng để lấy một access token mới mà không cần người dùng phải đăng nhập lại.
  - Có thời gian sống dài (ví dụ: vài ngày, vài tuần hoặc vài tháng), được lưu trữ an toàn ở phía client và thường liên kết với một người dùng và một ứng dụng client cụ thể.
  - Được gửi đến một endpoint chuyên biệt (ví dụ: /refresh-token) để yêu cầu cấp một access token mới khi token hiện tại hết hạn.

-- Tại sao nên sử dụng Refresh Token? --
  - Tăng cường bảo mật: 
    - Việc giữ cho access token có thời gian sống ngắn giúp giảm thiểu rủi ro khi token bị lộ.
  - Cải thiện trải nghiệm người dùng: 
    - Người dùng duy trì trạng thái đăng nhập mà không cần đăng nhập thường xuyên, vì các access token mới có thể được lấy một cách liền mạch bằng refresh token.
  - Kiểm soát truy cập: 
    - Refresh token có thể được thu hồi độc lập với access token, giúp kiểm soát phiên làm việc của người dùng tốt hơn.