-- Mã hóa (Encryption) là gì? --
- Mã hóa là một kỹ thuật bảo mật nền tảng được sử dụng để bảo vệ dữ liệu bằng cách chuyển đổi nó từ dạng có thể đọc được (gọi là plaintext - dữ liệu gốc) sang một định dạng không thể đọc được gọi là ciphertext (dữ liệu đã mã hóa).
- Quá trình chuyển đổi này sử dụng một thuật toán mật mã (đối xứng hoặc bất đối xứng) và một khóa mã hóa.
- Mục đích là để đảm bảo rằng nếu những người không được ủy quyền chặn được dữ liệu trong quá trình truyền tải hoặc lưu trữ, họ không thể hiểu hoặc lạm dụng nó.
  - Plaintext là dữ liệu gốc mà bạn muốn bảo vệ.
  - Ciphertext là đầu ra đã bị xáo trộn, không thể đọc được của quá trình mã hóa.
  - Khóa mã hóa (Encryption key) là một giá trị bí mật được thuật toán sử dụng để thực hiện việc chuyển đổi. Nếu không có khóa chính xác, việc giải mã ciphertext để lấy lại plaintext gần như là không thể.
- Trong ASP.NET Core Web API, mã hóa giúp bảo vệ các dữ liệu nhạy cảm, chẳng hạn như mật khẩu, khóa API hoặc thông tin người dùng, được truyền giữa client và server, đảm bảo tính bí mật trước những kẻ tấn công.

-- Giải mã (Decryption) là gì? -- 
- Giải mã là quá trình ngược lại của mã hóa. 
- Nó chuyển đổi ciphertext trở lại định dạng có thể đọc được ban đầu (plaintext). 
- Quá trình này yêu cầu khóa giải mã tương ứng, có thể là cùng một khóa đã sử dụng để mã hóa (trong mã hóa đối xứng) hoặc một khóa khác (trong mã hóa bất đối xứng).
- Chỉ những bên được ủy quyền có khóa chính xác mới có thể giải mã dữ liệu.
- Giải mã đảm bảo rằng ngay cả khi dữ liệu bị chặn trong quá trình truyền, chỉ người nhận dự kiến mới có thể truy cập thông tin gốc.
- Trong ASP.NET Core Web API, giải mã cho phép server hoặc client đọc các thông điệp đã được mã hóa một cách an toàn, qua đó cung cấp tính bảo mật và riêng tư cho dữ liệu.

-- Tại sao chúng ta cần Mã hóa và Giải mã? -- 
- Sau đây là những lý do tại sao Mã hóa và Giải mã lại rất quan trọng trong các ứng dụng web:
  - Bảo mật Dữ liệu (Data Security):
    - Nếu ai đó chặn được dữ liệu của bạn (như trong một cuộc tấn công xen giữa - man-in-the-middle attack), dữ liệu đã được mã hóa sẽ vô nghĩa đối với họ. 
    - Do đó, nó ngăn chặn việc truy cập trái phép vào thông tin nhạy cảm, chẳng hạn như dữ liệu cá nhân, chi tiết tài chính hoặc bí mật kinh doanh.
  - Toàn vẹn Dữ liệu (Data Integrity):
    - Mặc dù mã hóa không trực tiếp ngăn chặn việc giả mạo, nó giúp phát hiện liệu dữ liệu có bị thay đổi trong quá trình truyền hay không, vì quá trình giải mã sẽ thất bại hoặc tạo ra đầu ra không hợp lệ nếu dữ liệu bị sửa đổi.
  - Tuân thủ các Quy định về Quyền riêng tư (Privacy Compliance): 
    - Nhiều quy định, như GDPR, HIPAA và PCI DSS, yêu cầu dữ liệu nhạy cảm (bao gồm chi tiết cá nhân, hồ sơ y tế và thông tin thanh toán) phải được mã hóa trong quá trình lưu trữ và truyền tải để bảo vệ quyền riêng tư của người dùng và tuân thủ các tiêu chuẩn pháp lý.
  - Giao tiếp An toàn (Secure Communication): 
    - Các API thường truyền dữ liệu qua Internet, vốn dĩ là không an toàn. 
    - Mã hóa đảm bảo rằng ngay cả khi các gói dữ liệu bị chặn, thông tin vẫn được bảo vệ.
- Trong ASP.NET Core Web API, các cơ chế mã hóa bảo vệ cả yêu cầu (request) và phản hồi (response) của API, bảo vệ dữ liệu người dùng và duy trì sự tin cậy của hệ thống. 
- Ví dụ, hãy xem xét một hệ thống đăng nhập trong ASP.NET Core Web API. Thông tin đăng nhập của người dùng nên được mã hóa khi truyền từ client đến server. Nếu bạn lưu trữ token hoặc cấu hình nhạy cảm, chúng cũng nên được mã hóa.

-- Các Kỹ thuật Mã hóa và Giải mã trong ASP.NET Core: -- 
- Có hai loại mã hóa và giải mã chính:
1. Mã hóa/Giải mã Đối xứng (Symmetric Encryption/Decryption):
  - Sử dụng cùng một khóa cho cả việc mã hóa và giải mã.
  - Nhanh và hiệu quả, phù hợp để mã hóa khối lượng lớn dữ liệu.
  - Thách thức chính là việc chia sẻ khóa bí mật một cách an toàn giữa người gửi và người nhận.
  - Ví dụ:
    - AES (Advanced Encryption Standard): 
      - Thuật toán mã hóa đối xứng được sử dụng rộng rãi nhất, được công nhận vì tính bảo mật cao và hiệu quả.
    - DES (Data Encryption Standard): 
      - Một tiêu chuẩn cũ hơn, hiện được coi là không an toàn do độ dài khóa ngắn và các lỗ hổng.
2. Mã hóa/Giải mã Bất đối xứng (Asymmetric Encryptions/Decryptions):
  - Sử dụng một cặp khóa: một khóa công khai (public key) (để mã hóa) và một khóa riêng tư (private key) (để giải mã).
  - Khóa công khai có thể được chia sẻ công khai, trong khi khóa riêng tư phải được giữ bí mật.
  - An toàn hơn cho việc phân phối khóa, nhưng chậm hơn mã hóa đối xứng.
  - Ví dụ:
    - RSA: Một thuật toán bất đối xứng phổ biến dựa trên độ khó của việc phân tích các số lớn thành thừa số.
    - Elliptic Curve Cryptography (ECC): Cung cấp độ bảo mật tương tự RSA nhưng với kích thước khóa nhỏ hơn và hiệu suất cải thiện.
- Trong ASP.NET Core, mã hóa bất đối xứng thường được sử dụng để trao đổi khóa đối xứng một cách an toàn hoặc cho chữ ký số, trong khi mã hóa đối xứng được sử dụng để mã hóa dữ liệu hàng loạt.


-- Advanced Encryption Standard (AES) là gì? -- 
  - Chuẩn Mã hóa Tiên tiến (AES) là một thuật toán mã hóa khóa đối xứng được sử dụng rộng rãi trên toàn thế giới để bảo mật dữ liệu. 
  - Nó được Viện Tiêu chuẩn và Công nghệ Quốc gia Hoa Kỳ (NIST) thiết lập làm tiêu chuẩn mã hóa vào năm 2001. 
  - AES được sử dụng để bảo vệ thông tin nhạy cảm trong cả lĩnh vực chính phủ và thương mại.
  - Mã hóa dữ liệu theo các khối có kích thước cố định (mỗi khối 128 bit). 
  - Điều này có nghĩa là AES xử lý dữ liệu theo các đoạn chính xác 128 bit (tức là 16 byte) tại một thời điểm.
  - Hỗ trợ các khóa 128, 192 và 256 bit; khóa dài hơn đồng nghĩa với bảo mật cao hơn.
  - Rất nhanh và hiệu quả, làm cho nó phù hợp để mã hóa lượng lớn dữ liệu (ví dụ: tệp, trường cơ sở dữ liệu, lưu lượng mạng).
  - Được sử dụng bởi các chính phủ, tổ chức tài chính và ứng dụng thương mại để bảo mật dữ liệu nhạy cảm.
  - Thuật toán bao gồm nhiều vòng thay thế, hoán vị, trộn và cộng khóa để tạo ra ciphertext.
  - Trong ASP.NET Core Web API, AES thường được sử dụng để mã hóa dữ liệu "tĩnh" (trong cơ sở dữ liệu và tệp) hoặc "động" (trong quá trình giao tiếp API) để duy trì tính bảo mật.

-- Thuật toán Chuẩn Mã hóa Dữ liệu (DES) là gì? -- 
- DES là một thuật toán mã hóa đối xứng cũ hơn sử dụng khóa 56-bit. 
- Nó đã được sử dụng rộng rãi trong thế kỷ 20 nhưng hiện được coi là không an toàn do dễ bị tấn công brute-force. 
- Các phương pháp thay thế hiện đại như AES được khuyến nghị.
  - Sử dụng khóa 56-bit, hiện nay quá ngắn để chống lại các cuộc tấn công brute-force hiện đại.
  - DES đã được sử dụng rộng rãi trong quá khứ nhưng hiện được coi là lỗi thời và không an toàn.
  - Đã được thay thế bởi AES và các tiêu chuẩn an toàn hơn khác.
  - Trong các ứng dụng ASP.NET Core hiện đại, tránh sử dụng DES do rủi ro bảo mật và thay vào đó hãy dựa vào AES.

-- Luồng hoạt động của Thuật toán AES: --
- AES sử dụng cùng một khóa để mã hóa và giải mã dữ liệu, có nghĩa là cả người gửi và người nhận phải có quyền truy cập vào cùng một khóa bí mật.

Bước 1: Chia sẻ Khóa (Kênh An toàn):
  - Cả người gửi và người nhận phải chia sẻ cùng một khóa bí mật một cách an toàn trước khi bất kỳ quá trình mã hóa hoặc giải mã nào có thể xảy ra.
  - Khóa phải được truyền qua một kênh được bảo vệ (ví dụ: HTTPS, TLS) để tránh bị chặn.
Bước 2: Quá trình Mã hóa AES (Từ Plaintext sang Ciphertext):
  - Người gửi chuẩn bị dữ liệu plaintext cần bảo vệ.
  - Plaintext được gửi đến Server Mã hóa, nơi AES mã hóa nó bằng khóa bí mật đã chia sẻ.
  - Plaintext được chuyển đổi thành ciphertext, một dạng không thể đọc được.
  - Ciphertext sau đó được truyền qua mạng đến người nhận. Ngay cả khi bị chặn, nó cũng vô nghĩa nếu không có khóa.
Bước 3: Quá trình Giải mã (Từ Ciphertext sang Plaintext):
  - Người nhận nhận được ciphertext.
  - Ciphertext được gửi đến Server Giải mã, nơi sử dụng cùng một khóa bí mật đã chia sẻ.
  - AES giải mã ciphertext trở lại plaintext ban đầu.
  - Người nhận nhận được dữ liệu có thể đọc được, hoàn thành quá trình giao tiếp an toàn.