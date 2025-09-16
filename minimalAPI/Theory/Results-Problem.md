-- Results.Problem là gì? -- 
- Results.Problem là một helper method do ASP.NET Core Minimal APIs cung cấp, dùng để tạo ra một response theo chuẩn ProblemDetails.
- ProblemDetails cung cấp một cách có cấu trúc để trả về thông tin lỗi. Thay vì phải tự viết JSON trả về lỗi cho từng trường hợp thất bại của API, bạn có thể dùng Results.Problem() để sinh ra payload lỗi một cách nhất quán, chuẩn hóa và dễ dàng cho client phân tích.
- Một đối tượng ProblemDetails trả về từ Results.Problem() thường bao gồm các thuộc tính sau:
  1. Status (int): 
    - HTTP status code mô tả tình trạng lỗi (ví dụ: 400, 404, 500). 
    - Thuộc tính này cho client biết loại lỗi tổng quát đã xảy ra.
  2. Detail (string): 
    - Mô tả chi tiết, dễ đọc về lỗi.
    - Giúp developer hoặc người dùng hiểu nguyên nhân hoặc bản chất của lỗi.
  3. Title (string): 
    - Tiêu đề ngắn gọn, dễ hiểu, mô tả loại lỗi (ví dụ: "Not Found", "Internal Server Error").
  4. Type (string, optional): 
    - Một URI tham chiếu đến tài liệu mô tả chi tiết về loại lỗi. 
    - Thuộc tính này hữu ích khi bạn muốn liên kết đến docs hoặc guideline lỗi cụ thể.
  5. Instance (string, optional): 
    - URI tham chiếu đến instance cụ thể của lỗi, chẳng hạn như URL của request hoặc resource gây ra lỗi đó.
- Việc triển khai error handling và logging bài bản trong Minimal APIs sẽ:
  - Giúp ứng dụng ổn định hơn, dễ bảo trì hơn.
  - Tăng khả năng debug và giám sát.
  - Đem lại cho client thông tin phản hồi chuẩn hóa và đáng tin cậy, giúp API trở nên chuyên nghiệp và production-ready.