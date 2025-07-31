-- Phương thức HTTP DELETE trong ASP.NET Core Web API -- 
- Phương thức HTTP DELETE được sử dụng trong ASP.NET Core Web API để xoá các tài nguyên được xác định bằng URI. 
- Việc triển khai một endpoint DELETE trong Web API cho phép client gửi yêu cầu xoá tài nguyên trên server.
- Các yêu cầu DELETE được xem là idempotent (tính chất bất biến khi lặp lại), nghĩa là nhiều yêu cầu DELETE giống hệt nhau sẽ mang lại kết quả như nhau như thể chỉ thực hiện một lần. 
- Đây là tính chất quan trọng giúp API hoạt động ổn định và dễ dự đoán. 
- Sử dụng DELETE trong ASP.NET Core Web API giúp client có thể gọi lại cùng một yêu cầu mà không gây ra hiệu ứng phụ, với giả định rằng tài nguyên tồn tại và có thể bị xoá.
- Khi xử lý DELETE, cần trả về mã trạng thái HTTP phù hợp để phản ánh kết quả của thao tác xoá. Một số mã thường dùng gồm:
  - 204 No Content: Xoá thành công, không trả về nội dung.
  - 404 Not Found: Tài nguyên không tồn tại.
  - 403 Forbidden hoặc 401 Unauthorized: Yêu cầu bị từ chối do thiếu quyền truy cập hoặc chưa xác thực.

-- When Should We Use HTTP DELETE Method in ASP.NET Core Web API? --
- Phương thức HTTP DELETE được sử dụng trong ASP.NET Core Web API để xóa một tài nguyên khỏi máy chủ. 
- Dưới đây là những trường hợp bạn nên cân nhắc sử dụng phương thức DELETE:

1. ✅ Xóa tài nguyên
- Trường hợp sử dụng chính của phương thức DELETE là để xóa một tài nguyên được định danh bởi URI. 
- Khi client gửi một yêu cầu DELETE đến server, nghĩa là client yêu cầu server xóa tài nguyên tương ứng.
- Trong ASP.NET Core Web API, điều này thường đồng nghĩa với việc xóa một thực thể khỏi cơ sở dữ liệu, chẳng hạn như hồ sơ người dùng, bài viết blog, sản phẩm, hoặc bất kỳ thực thể dữ liệu nào mà API đang quản lý.

2. Khi không nên sử dụng DELETE:
- ❌ Không dùng để cập nhật: 
  - DELETE chỉ nên dùng để xóa.
  - Nếu bạn muốn thay đổi hoặc cập nhật dữ liệu, hãy sử dụng phương thức PUT hoặc PATCH.

- ❌ Không dùng cho tài nguyên không được phép xóa vĩnh viễn: 
  - Nếu quy trình nghiệp vụ yêu cầu tài nguyên không bị xóa hoàn toàn, ví dụ như yêu cầu lưu trữ (archive) hoặc hủy kích hoạt (deactivate) để phục vụ mục đích kiểm tra sau này, thì bạn nên chọn cách thay đổi trạng thái của tài nguyên thay vì thực hiện xóa vật lý.

-- Best Practices khi sử dụng HTTP DELETE: -- 
1. Kiểm tra quyền và xác thực đầu vào: 
  - Trước khi thực hiện thao tác DELETE, API cần kiểm tra các điều kiện như: người dùng có quyền xóa không? tài nguyên có đang ở trạng thái được phép xóa không?
  - Điều này giúp ngăn chặn thao tác không hợp lệ hoặc trái phép.
2. Xác nhận trước khi xóa (từ phía client):
  - Ở phía ứng dụng client, nên có bước xác nhận (confirm) trước khi gửi yêu cầu DELETE để tránh xóa nhầm.
3. Bảo vệ endpoint DELETE: 
  - Luôn áp dụng các cơ chế xác thực (Authentication) và ủy quyền (Authorization) để đảm bảo chỉ người dùng được phép mới có thể thao tác xóa.
4. Xử lý xóa dây chuyền và tác dụng phụ:
  - Trong nhiều trường hợp, việc xóa một tài nguyên có thể kéo theo việc xóa các dữ liệu liên quan. 
  - Ví dụ:
    - Xóa một tài khoản người dùng có thể kéo theo việc xóa bài viết, bình luận, đơn hàng,... của người dùng đó.
    - Cần lập kế hoạch và tài liệu hóa rõ ràng các hành vi liên quan đến thao tác xóa.
5. Phản hồi rõ ràng cho client: 
  - API cần trả về HTTP status code và thông điệp rõ ràng khi thao tác xóa không thực hiện được (ví dụ: 404 Not Found, 403 Forbidden, 409 Conflict...).
6. Ghi log (Logging): 
  - Luôn ghi lại thông tin thao tác xóa (ai xóa, xóa cái gì, thời điểm...) để hỗ trợ debug và audit sau này.

