-- Ignore Property Mapping using Automapper in ASP.NET Core Web API. --
- Việc bỏ qua ánh xạ một thuộc tính trong AutoMapper là nhu cầu phổ biến khi bạn không muốn ánh xạ một số thuộc tính cụ thể từ đối tượng nguồn (source) sang đối tượng đích (destination).
- AutoMapper cung cấp cách cấu hình đơn giản để thực hiện điều này thông qua phương thức ForMember() kết hợp với Ignore() trong cấu hình ánh xạ.
- Thông thường, bạn sẽ muốn loại bỏ một số thuộc tính khỏi quá trình ánh xạ để tránh việc lộ dữ liệu không cần thiết hoặc đơn giản là vì thuộc tính đó không có trường tương ứng trong đối tượng đích.
- Mặc định, AutoMapper sẽ tự động ánh xạ tất cả các thuộc tính có cùng tên giữa kiểu nguồn và kiểu đích. 
- Tuy nhiên, bạn có thể tùy chỉnh cấu hình để bỏ qua một thuộc tính nhất định bằng cách sử dụng ForMember() kết hợp Ignore() trong MappingProfile.

-- Ý nghĩa của các giá trị MemberList trong AutoMapper:
- None: Không thực hiện kiểm tra ánh xạ giữa các thành viên. AutoMapper sẽ không cảnh báo hay ném lỗi nếu một số thuộc tính không được ánh xạ. Thích hợp khi bạn chỉ muốn ánh xạ một phần dữ liệu.
- Source: Đảm bảo tất cả các thuộc tính của đối tượng nguồn phải được ánh xạ sang đích.
- Destination: Đảm bảo tất cả các thuộc tính của đối tượng đích phải có ánh xạ từ nguồn.

-- Use Cases for Automapper Ignore Method in ASP.NET Core Web API --
1. Giữ Nguyên Giá Trị Ở Đối Tượng Đích (Preserving Destination Values)
- Khi cập nhật một đối tượng hiện có bằng dữ liệu từ một đối tượng khác (thường gặp trong các thao tác cập nhật thông tin), đôi khi cần giữ nguyên một số thuộc tính ở đối tượng đích.
- Ví dụ: Trong tình huống cập nhật hồ sơ người dùng, bạn có thể muốn không thay đổi các trường như PasswordHash hoặc CreationDate.

2. Thuộc Tính Chỉ Đọc (Read-Only Properties)
- Với các thuộc tính chỉ đọc hoặc không được phép chỉnh sửa sau khi khởi tạo (ví dụ như EntityId, CreatedAt), việc sử dụng Ignore sẽ giúp đảm bảo các giá trị này không bị thay đổi khi ánh xạ.

3. Cập Nhật Một Phần Dữ Liệu (Partial Updates)
- Khi xây dựng các API hỗ trợ cập nhật từng phần (như PATCH theo chuẩn REST), có thể bạn chỉ muốn cập nhật các thuộc tính mà phía client đã cung cấp.
- Trong trường hợp này, bạn có thể bỏ qua các thuộc tính có giá trị null từ đối tượng nguồn để tránh ghi đè dữ liệu hiện có một cách không mong muốn.

4. Ánh Xạ Với Logic Khởi Tạo Riêng (Custom Initialization Logic)
- Với các thuộc tính yêu cầu logic khởi tạo riêng mà AutoMapper không thể xử lý tự động, bạn có thể dùng Ignore để loại trừ khỏi ánh xạ, sau đó tự thiết lập thủ công giá trị của các thuộc tính đó sau khi ánh xạ hoàn tất.

5. Bảo Mật Dữ Liệu (Security Considerations)
- Trong các tình huống nhận dữ liệu từ người dùng hoặc nguồn bên ngoài, việc bỏ qua các thuộc tính nhạy cảm là rất quan trọng để đảm bảo an toàn.
- Ví dụ: bạn có thể sử dụng Ignore để ngăn chặn các thuộc tính như UserRole, Permissions, v.v. bị cập nhật trái phép từ phía client.