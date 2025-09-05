-- Xác thực HMAC trong ASP.NET Core Web API là gì? -- 
- HMAC là viết tắt của Hash-Based Message Authentication Code (Mã Xác thực Tin nhắn dựa trên Băm). 
- Đây là một cơ chế bảo mật được sử dụng để xác minh tính toàn vẹn và tính xác thực của một tin nhắn được trao đổi giữa client và server.
- Nó là một cơ chế mã hóa cung cấp hai điều chính:
  1. Message Authentication Code (MAC): 
    - Đây là một đoạn thông tin ngắn (một mã) được sử dụng để xác nhận rằng một tin nhắn không bị thay đổi và thực sự đến từ người gửi mong đợi. 
    - Nó đảm bảo tính xác thực và tính toàn vẹn của tin nhắn.
  2. Hash-Based: Nó sử dụng một hàm băm như SHA-256, SHA-512, hoặc MD5 để tạo ra Mã Xác thực Tin nhắn (MAC).
- Điểm quan trọng nhất cần ghi nhớ là khi tạo Mã Xác thực Tin nhắn bằng Hàm băm, một khóa bí mật chung (shared secret key) phải được sử dụng. Hơn nữa, Khóa bí mật chung đó phải được chia sẻ giữa Client và Server tham gia vào việc gửi và nhận dữ liệu.

-- Tại sao chúng ta sử dụng Xác thực HMAC? -- 
- HMAC đặc biệt hữu ích trong các Web API để bảo vệ các yêu cầu từ client và ngăn chặn truy cập hoặc thao túng trái phép. 
- Trong một kịch bản Web API điển hình, các yêu cầu từ client có thể bị chặn hoặc bị giả mạo trong quá trình truyền tải. Để đảm bảo rằng:
  - Dữ liệu yêu cầu không bị thay đổi (tính toàn vẹn dữ liệu)
  - Yêu cầu thực sự đến từ một client đáng tin cậy, người biết khóa bí mật (xác thực),
=> chúng ta sử dụng Xác thực HMAC.

-- Xác thực HMAC hoạt động như thế nào? --
- Xác thực HMAC là một quy trình đảm bảo tính toàn vẹn và tính xác thực của dữ liệu trong các yêu cầu HTTP giữa client và server. 
- Nó thực hiện điều này bằng cách tạo ra một chữ ký mã hóa duy nhất (gọi là HMAC) cho mỗi yêu cầu bằng cách sử dụng một khóa bí mật chung và nội dung của yêu cầu. 
- Giá trị băm này sau đó được gửi cùng với Request HTTP. Việc này cần được thực hiện bởi client.
- Quy trình này được thiết kế cẩn thận để ngăn chặn kẻ tấn công giả mạo yêu cầu.
- Server xác minh tính xác thực của mỗi yêu cầu bằng cách tạo lại HMAC và so sánh nó với HMAC do client gửi.

Bước 1: Chia sẻ Khóa bí mật (Secret Key Sharing)
  - Trước khi bất kỳ giao tiếp an toàn nào bắt đầu, client và server ban đầu chia sẻ một khóa bí mật.
  - Khóa bí mật này không bao giờ được gửi cùng với các yêu cầu.
  - Nó được lưu trữ an toàn ở cả hai phía.
  - Tại sao? Khóa bí mật là xương sống của xác thực HMAC. Nếu ai đó có được khóa này, họ có thể tạo ra các HMAC hợp lệ và mạo danh client.

Bước 2: Client Chuẩn bị Tin nhắn (Message Preparation by Client)
  - Client chuẩn bị dữ liệu được sử dụng để tạo HMAC. Dữ liệu này bao gồm:
    1. Tin nhắn (Message): 
      - Kết hợp của phương thức HTTP (GET, POST, v.v.), đường dẫn URL và body của yêu cầu (nếu có). Ví dụ: POST /api/orders {order details}
    2. Khóa bí mật (Secret Key): 
      - Khóa bí mật chung chỉ client và server biết.
    3. Hàm băm (Hash Function): 
      - Một hàm băm mã hóa, chẳng hạn như SHA-256, SHA-512, hoặc MD5, được sử dụng để tạo HMAC.
      - Cả Client và Server sẽ sử dụng cùng một hàm băm mã hóa.
    4. Dấu thời gian (Timestamp): 
      - Thời gian hiện tại được bao gồm để ngăn chặn các cuộc tấn công phát lại (replay attacks - các yêu cầu cũ bị gửi lại).
    5. Nonce: 
      - Một giá trị ngẫu nhiên duy nhất được tạo cho mỗi yêu cầu để đảm bảo tính duy nhất. 
      - Nó được sử dụng để ngăn chặn các yêu cầu trùng lặp được chấp nhận.
  - Tại sao? Nếu ai đó cố gắng gửi lại (phát lại) một yêu cầu trước đó, server sẽ thấy cùng một timestamp/nonce và từ chối nó.
  - Các thành phần này được sử dụng trong thuật toán HMAC để tạo ra một giá trị băm an toàn sẽ được gửi đến server cùng với tin nhắn thực tế.

Bước 3: Tạo HMAC (HMAC Generation)
  - Client sử dụng khóa bí mật chung và hàm băm để tính toán một HMAC trên tin nhắn đã chuẩn bị, timestamp và nonce.
  - Kết quả là một chữ ký băm mã hóa duy nhất (chữ ký HMAC) đại diện cho yêu cầu.
  - Chữ ký HMAC này sau đó được đưa vào yêu cầu HTTP gửi đi.
  - Tại sao? Bất kỳ thay đổi nào trong tin nhắn, timestamp, nonce, hoặc khóa sẽ dẫn đến một HMAC khác. Đây là cách phát hiện sự giả mạo.

Bước 4: Gửi yêu cầu HTTP với HMAC (Sending the HTTP Request with HMAC)
  - Client gửi yêu cầu HTTP đến server. Yêu cầu bao gồm:
    - Dữ liệu yêu cầu HTTP (body, headers, URL, v.v.)
    - Header HMAC chứa chữ ký HMAC đã tạo, Client ID (để xác định client), Timestamp, và Nonce.
    - HMAC thường được bao gồm trong header Authorization, ví dụ: Authorization: HMAC ClientId|<HMAC Signature>|Nonce|Timestamp
  - Tại sao? Server cần những giá trị bổ sung này để tự xây dựng lại HMAC.

Bước 5: Server Nhận Yêu cầu (Server Receives the Request)
  - Server nhận yêu cầu HTTP và trích xuất:
    - Dữ liệu yêu cầu (message/body)
    - HMAC (từ header)
    - Timestamp và nonce
    - Client ID (để tra cứu khóa bí mật chính xác nếu có nhiều client)
  - Tại sao? Tất cả những điều này là cần thiết để tính toán lại HMAC và xác minh tính xác thực.

Bước 6: Server Chuẩn bị để Tạo lại HMAC (Server Prepares for HMAC Regeneration)
- Server lấy các thông tin sau để tạo lại HMAC:
  - Tin nhắn (nội dung yêu cầu)
  - Khóa bí mật được liên kết với Client ID (được lưu trữ an toàn trên server)
  - Cùng một hàm băm được client sử dụng
  - Timestamp và nonce từ header
  - Tại sao? Sử dụng chính xác các đầu vào giống như client đảm bảo rằng HMAC được tính toán lại sẽ chỉ khớp nếu không có sự giả mạo nào xảy ra.

Bước 7: Tạo HMAC Sử dụng Cùng Thuật toán (Generate HMAC Using Same Algorithm)
  - Server sử dụng cùng một thuật toán HMAC và các đầu vào (tin nhắn, khóa bí mật, timestamp, nonce) để tạo ra một chữ ký HMAC một cách độc lập.
  - Tại sao? Nếu tính toán của server tạo ra cùng một HMAC như HMAC đã nhận, tin nhắn là xác thực.

Bước 8: So sánh HMAC (HMAC Comparison)
  - Server so sánh HMAC mà nó đã tạo với HMAC do client gửi.
  - Nếu các chữ ký khớp nhau, điều này có nghĩa là:
    - Yêu cầu không bị thay đổi trong quá trình truyền.
    - Yêu cầu được tạo bởi một client sở hữu khóa bí mật.
    - Nếu chúng không khớp, điều đó có nghĩa là:
    - Yêu cầu đã bị giả mạo.
    - Yêu cầu không đến từ một client được ủy quyền.
Bước 9: Xác thực Yêu cầu (Request Validation)
  - Nếu cả hai chữ ký HMAC khớp nhau, server chấp nhận và xử lý yêu cầu.
  - Nếu các chữ ký không khớp, server từ chối yêu cầu vì không được ủy quyền hoặc không hợp lệ.

-- Xác thực Yêu cầu với Middleware Xác thực HMAC -- 
- Một middleware tùy chỉnh sẽ xác thực các yêu cầu đến:
  - Kiểm tra xem HMAC có được bật không.
  - Xác thực sự hiện diện và cấu trúc của header Authorization.
  - Trích xuất token HMAC, nonce, timestamp, và clientId từ các header.
  - Xác minh tính mới của timestamp và tính duy nhất của nonce để ngăn chặn tấn công phát lại.
  - Tính toán lại chữ ký HMAC sử dụng logic băm giống như client.
  - Cho phép hoặc từ chối yêu cầu tương ứng.

-- UNIX Timestamp là gì? -- 
- UNIX timestamp (Epoch time) là một biểu diễn số của thời gian dưới dạng số giây kể từ ngày 1 tháng 1 năm 1970, 00:00:00 UTC. Ví dụ, 1718170871 → tương ứng với một thời điểm cụ thể. Được sử dụng trong HMAC để:
  - Đảm bảo tính mới của các yêu cầu.
-- Ngăn chặn tấn công phát lại (replay attacks). -- 
- Tấn công phát lại là khi một kẻ tấn công bắt được một yêu cầu hợp lệ và gửi lại nó đến server để lặp lại thao tác một cách gian lận. 
- Vì chữ ký HMAC là hợp lệ, server có thể chấp nhận nó nếu không được bảo vệ.
- Chiến lược phòng chống:
  - Timestamps: Chỉ chấp nhận các yêu cầu trong một khoảng thời gian ngắn.
  - Nonce: Lưu trữ các nonce đã sử dụng gần đây và từ chối các bản sao.
  - Cùng nhau, chúng đảm bảo mỗi yêu cầu là duy nhất và mới.