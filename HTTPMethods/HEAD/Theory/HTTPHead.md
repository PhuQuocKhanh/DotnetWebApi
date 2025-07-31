-- HTTP HEAD Method --
- Phương thức HTTP HEAD tương tự như phương thức GET ở chỗ nó gửi yêu cầu đến server để lấy thông tin về một tài nguyên cụ thể.
- Tuy nhiên, không giống như GET, phương thức HEAD không trả về phần thân (body) của phản hồi, mà chỉ trả về phần header (tiêu đề). 
- Điều này khiến HEAD trở nên đặc biệt hữu ích trong các tình huống cần lấy thông tin meta (siêu dữ liệu) từ header mà không cần tải toàn bộ nội dung tài nguyên.
- Các mục đích chính của việc sử dụng phương thức HEAD bao gồm:
  1. Kiểm tra sự tồn tại của tài nguyên: 
    - Cho phép client xác nhận rằng một tài nguyên có tồn tại trên server hay không, mà không cần tải về nội dung.
    - Điều này hữu ích trong việc kiểm tra liên kết (link checking) hoặc xác minh tài nguyên đã được cập nhật.
  2. Theo dõi thay đổi tài nguyên:
    - Client có thể kiểm tra các header như Last-Modified để xác định xem tài nguyên đã thay đổi kể từ lần truy cập trước hay chưa, mà không cần tải toàn bộ nội dung.
  3. Kiểm thử tính hợp lệ của liên kết: 
    - HEAD có thể được sử dụng để kiểm tra tính khả dụng của một URL (liên kết có trả về phản hồi hợp lệ không) mà không gây tốn băng thông như GET.
  4. Lấy thông tin header: 
    - Client có thể truy xuất các thông tin như kiểu nội dung (Content-Type), độ dài nội dung (Content-Length) và các metadata khác về tài nguyên chỉ thông qua phần header.
  5. Tiết kiệm băng thông: 
    - Do không có body trong phản hồi, nên HEAD giúp giảm thiểu lưu lượng truyền tải, đặc biệt hữu ích trong các thao tác chỉ cần thông tin định danh hoặc xác thực.
-- When Should We Use HTTP HEAD Method in ASP.NET Core Web API? --
- Trong ASP.NET Core Web API, phương thức HTTP HEAD được sử dụng trong các tình huống mà client muốn lấy header của một resource mà không cần tải toàn bộ nội dung của resource đó. 
- Điều này đặc biệt hữu ích trong một số trường hợp sau:
1. Kiểm tra sự tồn tại hoặc khả năng truy cập của resource: 
  - Trước khi tải về một tệp lớn hoặc resource nào đó, client có thể gửi một request HEAD để xác minh xem resource đó có tồn tại và có thể truy cập được hay không.
  - Điều này giúp tiết kiệm băng thông và tài nguyên xử lý, tránh việc tải những tệp không cần thiết hoặc không hợp lệ.
2. Lấy metadata của resource: 
  - Client có thể chỉ cần thông tin metadata như ngày sửa đổi gần nhất (Last-Modified), kiểu nội dung (Content-Type) hoặc độ dài nội dung (Content-Length) mà không cần toàn bộ nội dung resource.
  - HEAD giúp lấy thông tin này một cách hiệu quả.
3. Xác thực cache (cache validation): 
  - Phương thức HEAD có thể được dùng để kiểm tra tính hợp lệ của dữ liệu đã cache. 
  - Ví dụ, client có thể gửi request HEAD kèm theo header If-Modified-Since để kiểm tra xem resource đã bị thay đổi kể từ lần cuối lưu cache hay chưa. 
  - Nếu server trả về mã trạng thái 304 Not Modified, client có thể tiếp tục sử dụng dữ liệu cache mà không cần tải lại, giúp giảm tải cho server và tiết kiệm băng thông.
4. Giám sát tình trạng hoạt động của server hoặc resource:
  - Các hệ thống tự động có thể sử dụng HEAD request để kiểm tra tình trạng sẵn sàng (availability) và phản hồi (responsiveness) của web server hoặc một resource cụ thể. 
  - Đây là cách nhẹ nhàng và hiệu quả để đảm bảo hệ thống hoạt động ổn định mà không phải xử lý toàn bộ nội dung của resource.