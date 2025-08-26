-- Custom Middleware trong ASP.NET Core Web API là gì? -- 
- Một Custom Middleware (phần mềm trung gian tùy chỉnh) trong ASP.NET Core Web API nằm trong pipeline xử lý HTTP request và xử lý các request đến cũng như các response đi. 
- Các thành phần middleware có thể thực hiện các tác vụ như ghi log, xác thực, xử lý lỗi, sửa đổi request/response, định tuyến, v.v. Bạn có thể coi middleware như một chuỗi các thành phần mà mọi HTTP request và response đều đi qua. Mỗi middleware có thể:
  - Kiểm tra hoặc sửa đổi HTTP request đến.
  - Thực hiện các hành động trước hoặc sau khi middleware/thành phần tiếp theo chạy.
  - Ngắt mạch (short-circuit) pipeline bằng cách gửi một phản hồi ngay lập tức.
- Custom middleware là middleware do người dùng định nghĩa mà chúng ta tạo ra để triển khai chức năng cụ thể cho nhu cầu ứng dụng của mình, chẳng hạn như ghi log tùy chỉnh, chèn header, hoặc xử lý ngoại lệ toàn cục.

-- Khi nào nên tạo Custom Exception Middleware trong ASP.NET Core Web API? -- 
- Chúng ta cần tạo một Custom Exception Middleware khi muốn:
  - Xử lý tất cả các ngoại lệ chưa được xử lý một cách toàn cục trên toàn bộ ứng dụng, bao gồm cả các ngoại lệ từ MVC controllers, các middleware khác, file tĩnh, hoặc API. Điều này đảm bảo phản hồi lỗi nhất quán và ghi log lỗi tập trung.
  - Đảm bảo định dạng phản hồi lỗi nhất quán cho client, chẳng hạn như trả về lỗi JSON với mã trạng thái và thông báo trong một cấu trúc thống nhất.
  - Ghi log ngoại lệ một cách tập trung để tất cả các lỗi được ghi lại ở một nơi với thông tin chẩn đoán chi tiết.
  - Kiểm soát thông báo lỗi dựa trên môi trường (development vs. production) để ngăn chặn rò rỉ các chi tiết nhạy cảm của máy chủ hoặc ngoại lệ cho người dùng cuối.
  - Bạn cần kiểm soát nhiều hơn đối với phản hồi HTTP, bao gồm việc thiết lập header hoặc sửa đổi body của response cho các trường hợp lỗi, điều này có thể khó thực hiện với exception filters.
  - Bắt ngoại lệ càng sớm càng tốt trong pipeline request, bao gồm cả những ngoại lệ được ném ra bên ngoài MVC controllers (như trong middleware hoặc endpoint routing).

