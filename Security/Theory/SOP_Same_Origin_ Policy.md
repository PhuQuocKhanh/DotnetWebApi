-- Same Origin Policy là gì--
- Chính sách Cùng Nguồn gốc (SOP) là một cơ chế bảo mật cơ bản được tất cả các trình duyệt web hiện đại thực thi để bảo vệ người dùng và dữ liệu của họ. 
- Nói cách khác, SOP ngăn một trang web bạn truy cập sử dụng JavaScript hoặc các lệnh gọi AJAX để truy cập dữ liệu từ một trang web khác trong trình duyệt của bạn mà không có sự cho phép.
- Một "nguồn gốc" (origin) được định nghĩa bởi sự kết hợp của ba yếu tố trong một URL:
  - Giao thức (Protocol): HTTP hoặc HTTPS
  - Tên miền (Domain): Tên miền của trang web (ví dụ: example.com)
  - Cổng (Port): Số cổng được sử dụng cho kết nối (ví dụ: 80 cho HTTP, 443 cho HTTPS)
- Nếu bất kỳ yếu tố nào trong số này (giao thức, tên miền, hoặc cổng) khác nhau, chúng được coi là các nguồn gốc khác nhau. Ví dụ, https://example.com:443 là một nguồn gốc khác với http://example.com:80 vì giao thức và cổng khác nhau.

-- Same Origin Policy (SOP) hoạt động như thế nào? -- 
- Khi một trang web được tải từ một nguồn gốc (ví dụ: https://example.com:443), SOP ngăn mã JavaScript (các lệnh gọi AJAX) trên trang đó thực hiện yêu cầu đến một nguồn gốc khác (ví dụ: https://anotherdomain.com hoặc thậm chí cùng tên miền nhưng khác cổng như https://example.com:8080). 
- Hạn chế này bảo vệ người dùng bằng cách ngăn một trang web truy cập dữ liệu nhạy cảm trên một trang web khác mà không được phép thông qua JavaScript hoặc jQuery AJAX.
- Khi một trang web cố gắng thực hiện yêu cầu AJAX đến một nguồn gốc khác, trình duyệt sẽ áp dụng SOP để quyết định xem yêu cầu có được phép hay không. Hãy cùng tìm hiểu các kịch bản qua ví dụ.
- Hãy xem xét các kịch bản sau:
1. Yêu cầu Cùng Nguồn gốc (Same-Origin Request)
  - URL Client (trang web): https://example.com/client
  - URL Server (API): https://example.com/api
  - Kết quả: Yêu cầu AJAX thành công vì cả hai URL đều có cùng nguồn gốc, cùng giao thức (https), tên miền (example.com), và cổng (mặc định 443). Không cần quyền đặc biệt. CORS không cần thiết ở đây vì yêu cầu nằm trong cùng một nguồn gốc.

2. Yêu cầu Chéo Nguồn gốc khi CORS chưa được kích hoạt
  - URL Client (trang web): https://myclient.com
  - URL Server (API): https://example.com/api
  - Kết quả: Trình duyệt chặn yêu cầu AJAX vì các nguồn gốc (myclient.com và example.com) khác nhau. Máy chủ chưa chỉ định rằng nó cho phép các yêu cầu từ https://myclient.com, vì vậy trình duyệt thực thi SOP và chặn yêu cầu vì lý do bảo mật.

3. Yêu cầu Chéo Nguồn gốc khi CORS đã được kích hoạt
  - URL Client (trang web): https://myclient.com
  - URL Server (API): https://example.com/api (với CORS được kích hoạt cho myclient.com)
  - Kết quả: Yêu cầu AJAX thành công vì máy chủ (example.com) đã cho phép một cách rõ ràng các yêu cầu từ myclient.com bằng cách kích hoạt CORS. Điều này cho trình duyệt biết rằng yêu cầu này an toàn mặc dù có nguồn gốc khác nhau.
