-- Xử lý lỗi và Ghi log trong ASP.NET Core Minimal API -- 
- Xử lý lỗi (Error Handling) và ghi log (Logging) là hai thành phần then chốt để xây dựng API ổn định, dễ bảo trì và có khả năng mở rộng.
- Xử lý lỗi giúp người dùng nhận được phản hồi rõ ràng, dễ hiểu, có thể hành động khi xảy ra lỗi.
- Ghi log cung cấp thông tin quan trọng về cách ứng dụng đang hoạt động bên trong, hỗ trợ lập trình viên chẩn đoán và khắc phục sự cố.

-- Xử lý lỗi (Error Handling) là gì? -- 
- Xử lý lỗi là tập hợp các kỹ thuật và thực hành dùng để xử lý những tình huống không mong đợi hoặc lỗi phát sinh trong quá trình API chạy. 
- Bao gồm:
  - Input không hợp lệ.
  - Tài nguyên không tồn tại.
  - Exception trong logic nghiệp vụ.
  - Lỗi từ hệ thống bên ngoài (như database hoặc web service).

-- Tại sao xử lý lỗi lại quan trọng? --
- Ngăn chặn crash ứng dụng: 
  - đảm bảo API không bị sập khi gặp exception.
- Trải nghiệm người dùng tốt hơn: 
  - client (frontend hoặc API khác) nhận được thông báo lỗi có ý nghĩa và đồng nhất, thay vì raw stack trace hoặc thông điệp khó hiểu.
- Tăng tính bảo mật: 
  - kiểm soát thông tin lỗi trả về để tránh lộ dữ liệu nhạy cảm.
- Dễ bảo trì: 
  - cấu trúc xử lý lỗi rõ ràng giúp debug nhanh và xác định nguyên nhân sự cố dễ dàng.

-- Ghi log (Logging) là gì? -- 
- Logging là quá trình ghi lại thông tin về hành vi runtime của ứng dụng (sự kiện, cảnh báo, lỗi, trace) vào nơi lưu trữ lâu dài (file, database hoặc dịch vụ logging).
- Tại sao ghi log lại quan trọng?
  - Ghi lại thông tin runtime: lưu lại các sự kiện, warning, lỗi và thông điệp trong khi ứng dụng chạy.
  - Hỗ trợ debug: giúp lập trình viên lần theo luồng xử lý và tìm nguyên nhân gốc rễ của vấn đề.
  - Hỗ trợ giám sát: dễ dàng tích hợp với công cụ monitoring/alerting để phát hiện vấn đề sớm.