-- Why Content Negotiation in Restful Services? --
- Content Negotiation (thỏa thuận nội dung) là một phần thiết yếu trong các dịch vụ RESTful vì nó cho phép server trả về dữ liệu theo nhiều định dạng khác nhau, dựa trên yêu cầu của client.
- Việc này đặc biệt quan trọng trong các tình huống sau:

1. Tính linh hoạt cho client:
- Mỗi client có thể có nhu cầu hoặc ưu tiên định dạng dữ liệu khác nhau. 
- Ví dụ, một ứng dụng web hiện đại có thể ưa chuộng định dạng JSON vì nhẹ và dễ xử lý, trong khi một hệ thống cũ có thể yêu cầu XML để tương thích.
2. Khả năng tương tác (interoperability): 
- Việc hỗ trợ nhiều định dạng dữ liệu giúp API có thể tương thích và giao tiếp với đa dạng client mà không gặp vấn đề về định dạng.
3. Tối ưu hiệu suất: 
- Client có thể lựa chọn định dạng dữ liệu hiệu quả nhất, từ đó giảm băng thông và tăng hiệu năng xử lý.

-- What happens if we specify both application/JSON and application/XML in the Accept Header? --
- Khi gửi một HTTP Request, bạn có thể đặt nhiều giá trị cho header Accept, cách nhau bằng dấu phẩy. 
- Trong trường hợp này, giá trị đứng đầu sẽ được ưu tiên. Nghĩa là nếu client gửi cả application/json và application/xml trong header Accept, thì server sẽ chọn media type đầu tiên mà nó hỗ trợ, theo thứ tự do client cung cấp.
- Cụ thể:
  1. Nếu client gửi header: Accept: application/json, application/xml và server hỗ trợ JSON, thì JSON sẽ được ưu tiên.
  2. Nếu thứ tự được đảo ngược: Accept: application/xml, application/json, thì XML sẽ được ưu tiên nếu server hỗ trợ nó.