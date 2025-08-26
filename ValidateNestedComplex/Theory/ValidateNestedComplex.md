-- Xác thực Thuộc tính Phức tạp Lồng nhau hoặc Thuộc tính Dạng Tập hợp bằng Fluent API -- 
- Để xác thực một thuộc tính là kiểu phức tạp lồng nhau hoặc thuộc tính dạng tập hợp (collection) của một model, chúng ta cần sử dụng hai phương thức của Fluent API là SetValidator() và ForEach().
1. SetValidator(): 
  - Phương thức SetValidator() cho phép chúng ta sử dụng một lớp validator riêng biệt để xác thực một thuộc tính có kiểu phức tạp lồng nhau của một model. 
  - Ví dụ, nếu model của chúng ta chứa một thuộc tính phức tạp, chúng ta có thể dùng SetValidator() để xác thực các thuộc tính của kiểu phức tạp đó bằng một lớp validator riêng.
2. ForEach(): 
  - Phương thức ForEach() xác thực từng phần tử trong một thuộc tính dạng tập hợp. 
  - Ví dụ, nếu bạn có một thuộc tính dạng tập hợp trong một model, phương thức ForEach() của Fluent API cho phép chúng ta áp dụng các quy tắc xác thực cho từng phần tử trong tập hợp đó.

-- Các phương thức SetValidator() và ForEach() của Fluent API -- 
1. SetValidator()
  - Phương thức này đính kèm một validator riêng biệt vào một thuộc tính phức tạp (tức là một đối tượng được lồng bên trong một đối tượng khác). 
  - Trong OrderDTOValidator, sau khi đảm bảo NewAddress không phải là null, phương thức SetValidator() sẽ áp dụng AddressDTOValidator để xác thực các thuộc tính của AddressDTO lồng nhau.
2. ForEach()
  - Phương thức này áp dụng một tập hợp các quy tắc xác thực cho mọi phần tử trong một thuộc tính dạng tập hợp (collection). 
  - Trong OrderDTOValidator, ForEach() lặp qua tập hợp OrderItems và áp dụng OrderItemDTOValidator cho từng mặt hàng trong đơn hàng.

