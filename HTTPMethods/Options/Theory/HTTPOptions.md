-- HTTP OPTIONS Method --
- Phương thức HTTP OPTIONS được sử dụng để mô tả các tùy chọn giao tiếp (communication options) mà tài nguyên đích (target resource) hỗ trợ. 
- Đây là một phần của giao thức HTTP – nền tảng cho việc truyền tải dữ liệu trên World Wide Web. 
- Khi gửi một yêu cầu OPTIONS, client đang yêu cầu server cung cấp thông tin về các phương thức HTTP và các tùy chọn mà tài nguyên hoặc server cho phép, mà không thực thi hành động truy xuất dữ liệu hay gây ảnh hưởng đến tài nguyên.

- Đặc điểm chính của phương thức HTTP OPTIONS:
1. Mục đích: 
  - OPTIONS cho phép client kiểm tra các tính năng mà server hoặc một endpoint cụ thể hỗ trợ. 
  - Ví dụ, nó có thể dùng để xác định các phương thức HTTP nào (như GET, POST, PUT, DELETE) được phép gọi lên một endpoint cụ thể.
2. CORS – Cross-Origin Resource Sharing (Chia sẻ tài nguyên giữa các nguồn khác nhau): 
  - Trong bối cảnh CORS, OPTIONS thường được sử dụng như một preflight request (yêu cầu kiểm tra trước) do trình duyệt gửi đi trước khi thực hiện các yêu cầu thực sự có thể ảnh hưởng đến tài nguyên (như PUT, DELETE, hoặc POST với custom headers). 
  - Server sẽ phản hồi với các header liên quan đến chính sách CORS để cho trình duyệt biết liệu yêu cầu thực tế có được phép hay không.
3. Header phản hồi: 
  - Server khi xử lý OPTIONS thường trả về các header như:
    - Allow: Liệt kê các HTTP methods mà endpoint hỗ trợ (GET, POST, PUT, DELETE, v.v.).
    - Access-Control-Allow-Methods: (trong ngữ cảnh CORS) chỉ rõ những phương thức nào được phép sử dụng từ các origin khác.
    - Có thể kèm thêm Access-Control-Allow-Origin, Access-Control-Allow-Headers, v.v.
4. Không truy xuất tài nguyên: 
  - OPTIONS chỉ có tác dụng truy vấn meta-information (thông tin siêu dữ liệu) về tài nguyên chứ không truy xuất nội dung thật sự của tài nguyên đó. Đây là một phương thức an toàn, không gây side effect trên server.

-- Các Thực Tiễn Tốt Nhất (Best Practices): -- 
1. Chính sách CORS:
  - Khi sử dụng middleware CORS, hãy đảm bảo rằng các chính sách của bạn được cấu hình chính xác để ngăn chặn các yêu cầu cross-origin không mong muốn.
2. Logic tùy chỉnh: 
  - Nếu phản hồi của phương thức OPTIONS cần logic tùy chỉnh (ví dụ: cho phép các HTTP method khác nhau tùy theo yêu cầu hoặc trạng thái tài nguyên), bạn nên xử lý các yêu cầu OPTIONS một cách tường minh trong controller.
3. Bảo mật: 
  - Luôn xác thực và làm sạch dữ liệu đầu vào trong các handler cho OPTIONS để tránh các lỗ hổng bảo mật.

-- Khi Nào Nên Sử Dụng Phương Thức HTTP OPTIONS trong ASP.NET Core Web API? -- 
- Phương thức HTTP OPTIONS được sử dụng trong ASP.NET Core Web API (và các framework web khác) để mô tả các tùy chọn giao tiếp (communication options) dành cho một tài nguyên cụ thể. 
- Mục đích chính của nó là cho phép client xác định các tùy chọn hoặc yêu cầu liên quan đến một tài nguyên, hoặc năng lực (capabilities) của máy chủ, mà không thực hiện hành động lên tài nguyên đó. 
- Dưới đây là một số tình huống điển hình mà phương thức OPTIONS phát huy tác dụng trong ASP.NET Core Web API:
1. Xử lý Preflight Request trong CORS:
- Đây là tình huống phổ biến nhất khi sử dụng phương thức OPTIONS. 
- Trình duyệt sẽ gửi một HTTP OPTIONS request đến máy chủ trước khi thực hiện một yêu cầu có thể ảnh hưởng đến dữ liệu người dùng (ví dụ: POST, PUT, DELETE) đến một domain khác (cross-origin). 
- Mục đích là để kiểm tra xem server có cho phép origin đó gửi request hay không. 
- Trong ASP.NET Core, bạn có thể cấu hình chính sách CORS để tự động xử lý các preflight request này.
2. Tìm hiểu các HTTP Method được hỗ trợ:
- Client có thể gửi một request OPTIONS đến một endpoint cụ thể để biết được endpoint đó hỗ trợ những HTTP method nào (GET, POST, PUT, DELETE, v.v.). 
- Điều này đặc biệt hữu ích với các API theo chuẩn REST, nơi mỗi tài nguyên có thể hỗ trợ các phương thức khác nhau. 
- Nhờ vậy, client có thể xây dựng các ứng dụng linh hoạt, thích ứng với khả năng mà server cung cấp.
3. Khám phá và tài liệu hóa API:
- Mặc dù không phải mục tiêu chính, phương thức OPTIONS có thể được dùng để trả về metadata liên quan đến API, chẳng hạn như liên kết đến tài liệu hoặc các tài nguyên liên quan. 
- Điều này hỗ trợ lập trình viên khám phá cấu trúc API và hiểu rõ các chức năng mà không cần tham khảo tài liệu ngoài.
4. Mục đích tùy biến theo ứng dụng:
- Developer cũng có thể tận dụng phương thức OPTIONS để triển khai các cơ chế handshake hoặc negotiation tùy chỉnh giữa client và server. 
- Ví dụ: thương lượng định dạng dữ liệu, ngôn ngữ ưu tiên, hoặc phương thức xác thực trước khi gửi các request chính thức đến tài nguyên.

