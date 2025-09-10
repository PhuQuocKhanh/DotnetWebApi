-- JSON Web Key Sets --
- Các thành phần chính của JSON Web Key Sets (JWKS)
1. kty (Key Type - Loại khóa):
  - Trường này xác định loại khóa mã hóa.
  - Ví dụ: “RSA” cho khóa công khai RSA. 
  - Nó giúp các ứng dụng biết được khóa này liên quan đến thuật toán nào và cách sử dụng nó.
2. use (Public Key Use - Mục đích sử dụng khóa công khai):
  - Trường này chỉ ra mục đích sử dụng của khóa. 
  - Ví dụ: “sig” (signature) nghĩa là khóa được dùng để ký và xác minh chữ ký số. 
  - Bằng cách này, người dùng biết cách áp dụng khóa một cách chính xác.
3. kid (Key ID - Định danh khóa):
  - Đây là một mã định danh duy nhất cho khóa. 
  - Nó giúp các client hoặc server phân biệt nhiều khóa trong một bộ JWK (JWKS). 
  - Thông thường, kid được dùng để khớp khóa trong JWKS với khóa được chỉ định trong header của một JSON Web Token (JWT), cho phép người dùng chọn đúng khóa công khai để xác thực chữ ký khi có nhiều khóa.
4. alg (Algorithm - Thuật toán):
  - Trường này chỉ rõ thuật toán dự định sẽ được sử dụng với khóa. Ví dụ:
    - “RS256”: Thuật toán ký RSA sử dụng SHA-256.
    - “RS512”: Thuật toán ký RSA sử dụng SHA-512.
  - Nó giúp người dùng biết chính xác thuật toán mật mã nào cần dùng với khóa.
5. n (Modulus - Số mũ):
  - Đây là modulus (số mũ) của khóa công khai RSA, được biểu diễn dưới dạng giá trị đã được mã hóa Base64 URL. 
  - Trong RSA, khóa công khai bao gồm một modulus (n) và một số mũ (e). n là một số nguyên lớn, cấu thành nên cấu trúc toán học của khóa RSA, dùng trong cả quá trình mã hóa (khi sử dụng khóa công khai) và xác minh chữ ký.
6. e (Exponent - Số mũ):
  - Đây là số mũ công khai (exponent) của khóa công khai RSA, cũng được mã hóa Base64 URL. 
  - Trong phần lớn các trường hợp, số mũ (e) là một giá trị nhỏ. 
  - Nó hoạt động cùng với modulus để xác định khóa công khai, được sử dụng để mã hóa RSA hoặc xác minh chữ ký.