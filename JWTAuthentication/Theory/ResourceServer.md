--  Resource Server và Client với Xác thực JWT -- 
1. Ứng dụng Resource Server
  - Đây sẽ là một dự án ASP.NET Core Web API, cung cấp các endpoint được bảo mật để các client sử dụng.
2. Ứng dụng Client (Máy khách): 
  - Đây sẽ là một ứng dụng .NET Core Console. 
  - Ứng dụng này trước tiên sẽ tạo access token bằng cách sử dụng endpoint do ứng dụng Máy chủ xác thực (Authentication Server) cung cấp. 
  - Sau đó, nó sẽ dùng access token này để truy cập các endpoint an toàn từ Ứng dụng Resource Server.
