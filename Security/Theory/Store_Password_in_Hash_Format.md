-- Mật khẩu ở dạng Hash là gì? -- 
- Mật khẩu ở dạng Hash là kết quả đầu ra sau khi áp dụng một hàm băm mật mã học (chẳng hạn như HMACSHA256 hoặc HMACSHA512) lên một mật khẩu (cùng với salt). 
- Các hàm băm mật mã học được thiết kế để nhận một đầu vào (hay 'thông điệp') và trả về một chuỗi byte có kích thước cố định. Đây không phải là mật khẩu được mã hóa (có thể giải mã); nó là một phép biến đổi một chiều. 
- Dưới đây là các Đặc điểm Chính của một Mật khẩu ở dạng Hash:
  - Tất định (Deterministic): Cùng một mật khẩu + salt + thuật toán luôn cho ra cùng một hash.
  - Kích thước cố định (Fixed Size): Kích thước đầu ra (ví dụ: 256 bit cho SHA256, 512 bit cho SHA512) luôn không đổi, bất kể độ dài đầu vào. Đầu ra của hash thường trông giống như một chuỗi ký tự thập lục phân (hexadecimal) ngẫu nhiên.
  - Thay đổi nhỏ = Khác biệt lớn: Chỉ cần thay đổi một ký tự của mật khẩu cũng sẽ làm thay đổi hoàn toàn chuỗi hash.
  - Thêm Salt (Salting): Thêm một chuỗi ngẫu nhiên gọi là "salt" (được lưu cùng với hash) đảm bảo rằng các mật khẩu giống hệt nhau sẽ có các chuỗi hash khác nhau.

-- Tại sao chúng ta cần lưu trữ mật khẩu ở dạng Hash trong Database? -- 
- Lưu trữ mật khẩu ở dạng hash là một biện pháp bảo mật cực kỳ quan trọng. Dưới đây là lý do tại sao:
  - Bảo mật dữ liệu người dùng: 
    - Lưu mật khẩu dưới dạng văn bản thuần (plain text) là một rủi ro bảo mật khổng lồ. 
    - Nếu cơ sở dữ liệu của bạn bị xâm phạm, kẻ tấn công có thể thấy trực tiếp tất cả mật khẩu của người dùng và lạm dụng chúng.
  - Bảo vệ một chiều: 
    - Hashing chuyển đổi mật khẩu thành một chuỗi có độ dài cố định không thể đảo ngược lại thành mật khẩu ban đầu, bảo vệ người dùng ngay cả khi dữ liệu bị rò rỉ.
  - Toàn vẹn dữ liệu: 
    - Các hàm hash đảm bảo rằng ngay cả một thay đổi nhỏ trong đầu vào (mật khẩu) cũng làm thay đổi đáng kể đầu ra (hash), từ đó ngăn chặn việc giả mạo.
  - Sử dụng Salt: 
    - Dùng salt (dữ liệu ngẫu nhiên kết hợp với mật khẩu) trước khi hash sẽ ngăn chặn kẻ tấn công sử dụng các bảng tra cứu được tính toán trước (rainbow tables) để bẻ khóa mật khẩu.
  - Ngăn chặn tấn công tái sử dụng mật khẩu:
    - Ngay cả khi tin tặc lấy được các mật khẩu đã hash, chúng phải tốn rất nhiều thời gian và sức mạnh tính toán để bẻ khóa, đặc biệt nếu bạn sử dụng phương pháp hash và salt đúng cách.
  - Tuân thủ các phương pháp tốt nhất và yêu cầu pháp lý: 
    - Các tiêu chuẩn quy định (như GDPR, PCI-DSS, HIPAA, v.v.) yêu cầu xử lý mật khẩu an toàn.
- Kết luận: Hashing là tiêu chuẩn ngành để lưu trữ mật khẩu một cách an toàn. Không bao giờ lưu trữ hoặc ghi log mật khẩu dưới dạng văn bản thuần.

-- Sự khác biệt giữa HMACSHA256 và HMACSHA512 là gì? -- 
- SHA256 và SHA512 đều là các hàm băm mật mã học thuộc họ SHA-2 (Secure Hash Algorithm 2), được thiết kế bởi Cơ quan An ninh Quốc gia Hoa Kỳ (NSA). 
- Trong ASP.NET Core, cả HMACSHA256 và HMACSHA512 đều là một phần của namespace System.Security.Cryptography. 
- Chúng khác nhau chủ yếu ở độ dài đầu ra và yêu cầu tính toán.

1. HMACSHA256 trong ASP.NET Core:
  - Kích thước Hash: Tạo ra một chuỗi hash dài 256 bit (32 byte).
  - Bảo mật: Cung cấp mức độ bảo mật tốt và được áp dụng rộng rãi.
  - Hiệu năng: Nhanh hơn HMACSHA512 do kích thước hash nhỏ hơn, điều này có thể là một lợi thế trong các kịch bản ưu tiên hiệu năng.

2. HMACSHA512 trong ASP.NET Core:
  - Kích thước Hash: Tạo ra một chuỗi hash dài 512 bit (64 byte).
  - Bảo mật: Cung cấp mức độ bảo mật cao hơn so với HMACSHA256 vì kích thước hash lớn hơn.
  - Hiệu năng: Thường chậm hơn HMACSHA256 vì nó xử lý các khối dữ liệu lớn hơn và tạo ra hash dài hơn. Tuy nhiên, trên các hệ thống có bộ xử lý 64-bit, sự khác biệt về hiệu năng có thể không đáng kể.

- Lựa chọn giữa HMACSHA256 và HMACSHA512
  - Nhu cầu bảo mật: Nếu bạn yêu cầu bảo mật cao hơn và ít quan tâm đến hiệu năng, HMACSHA512 có thể là lựa chọn tốt hơn. Đối với các mục đích chung, HMACSHA256 cung cấp một cách tiếp cận cân bằng.
  - Cân nhắc về hiệu năng: Trong các ứng dụng nhạy cảm về hiệu năng, tốc độ tính toán nhanh hơn của HMACSHA256 có thể phù hợp hơn.
  - Yêu cầu tuân thủ: Một số quy định hoặc tiêu chuẩn ngành có thể chỉ định việc sử dụng một kích thước hàm hash cụ thể.
