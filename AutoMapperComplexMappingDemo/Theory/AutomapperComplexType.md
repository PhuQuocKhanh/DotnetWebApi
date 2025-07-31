-- What is AutoMapper Complex Type Mapping? --
- AutoMapper là một thư viện ánh xạ (mapper) đối tượng-đối tượng giúp tự động ánh xạ (map) các thuộc tính từ một đối tượng nguồn (source) sang đối tượng đích (destination), nhờ đó giảm thiểu việc viết code ánh xạ thủ công.
- Khi làm việc với các entity trong ứng dụng — đặc biệt khi sử dụng Entity Framework Core — ta thường cần ánh xạ giữa các entity (đại diện cho bảng trong cơ sở dữ liệu) và các DTO (Data Transfer Objects – Đối tượng truyền dữ liệu), những đối tượng chỉ chứa thông tin cần thiết để trả về cho client qua API.
- Complex Type Mapping trong AutoMapper xuất hiện trong các trường hợp sau:
  1. Khi cả đối tượng nguồn và đích đều có các thuộc tính dạng phức (complex type), tức là thuộc tính đó lại là một class khác (không phải kiểu dữ liệu đơn giản như int, string, v.v.).
  2. Khi cấu trúc hoặc tên thuộc tính giữa source và destination không giống nhau và cần xử lý đặc biệt.
  3. Khi đối tượng đích có thuộc tính dạng complex hoặc danh sách (collection) mà cần được ánh xạ từ nhiều nguồn dữ liệu khác nhau.
  4. Khi cần có các quy tắc ánh xạ đặc biệt, ví dụ: gộp hai thuộc tính từ source thành một thuộc tính trong destination.

-- When to Use AutoMapper Complex Type Mapping? --
1. Thực thể lồng nhau hoặc có quan hệ (Nested or Related Entities):
- Khi cần chuyển đổi một thực thể (entity) chứa các đối tượng con hoặc danh sách đối tượng sang DTO phẳng (flat DTO) hoặc ngược lại.
- Ví dụ: ánh xạ từ một Order chứa danh sách OrderItem sang một OrderDTO bao gồm thông tin đơn hàng và danh sách chi tiết đơn hàng.

2. Chuyển đổi tùy chỉnh (Custom Transformations):
- Khi cần xử lý dữ liệu đặc biệt như gộp (concatenate) các trường, định dạng lại ngày giờ, chuyển đổi kiểu dữ liệu, v.v. 
- Ví dụ: ánh xạ FirstName và LastName thành một trường FullName trong DTO.

3. Tổng hợp dữ liệu (Data Aggregation):
- Khi cần tổng hợp dữ liệu từ nhiều thực thể vào một DTO duy nhất, thường dùng cho các mô hình phản hồi API chứa dữ liệu tổng hợp (ví dụ: tổng số lượng, tổng tiền, trạng thái đơn hàng,...).