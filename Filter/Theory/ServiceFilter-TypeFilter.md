-- ServiceFilter trong ASP.NET Core Web API là gì? -- 
- ServiceFilter là một attribute cho phép chúng ta áp dụng một filter (chẳng hạn như action, resource, authorization, hoặc exception) đã được đăng ký như một service trong Dependency Injection (DI) container. 
- Khi chúng ta áp dụng [ServiceFilter(typeof(MyFilter))] trên một controller hoặc action, ASP.NET Core sẽ lấy (resolve) instance của filter đó từ DI container.
- Khi sử dụng ServiceFilter, chúng ta đang yêu cầu ASP.NET Core tìm kiếm filter như một service đã được đăng ký. 
- Điều này có nghĩa là filter phải được đăng ký trong lớp Program.cs. Nói ngắn gọn, ServiceFilter hoạt động với các filter đã được đăng ký trong DI.

-- Khi nào nên sử dụng ServiceFilter trong ASP.NET Core Web API? -- 
- Chúng ta nên sử dụng ServiceFilter khi:
  - Chúng ta có một lớp filter đã được đăng ký như một service trong DI container.
  - Chúng ta muốn chia sẻ cùng một instance hoặc để DI container tạo ra instance của filter.
  - Chúng ta cần hỗ trợ DI đầy đủ bên trong filter và muốn quản lý vòng đời (lifecycle) của filter bằng service container.
  - Chúng ta muốn việc đăng ký filter được quản lý tập trung để dễ dàng hơn.

-- TypeFilter trong ASP.NET Core Web API là gì? -- 
- Attribute TypeFilter là một cách khác để đính kèm filter vào controller/action. 
- Không giống như ServiceFilter, nó tạo một instance của filter bằng cách sử dụng service provider, nhưng nó không yêu cầu bạn phải đăng ký filter trong DI container. 
- TypeFilter cũng có thể truyền các đối số vào constructor của filter, cho phép bạn thiết lập các thuộc tính hoặc cấu hình ngay tại nơi sử dụng.

-- Khi nào nên sử dụng TypeFilter trong ASP.NET Core Web API? -- 
- Chúng ta nên sử dụng TypeFilter khi:
  - Chúng ta muốn áp dụng một filter nhưng không muốn đăng ký nó một cách tường minh trong DI.
  - Chúng ta muốn truyền các tham số cho constructor của filter một cách linh động ngay tại nơi sử dụng attribute.
  - Chúng ta muốn một cách đơn giản hơn để áp dụng filter mà không cần sửa đổi phần đăng ký DI.

-- Sự khác biệt giữa ServiceFilter và TypeFilter trong ASP.NET Core Web API -- 
- Dưới đây là những khác biệt chính giữa ServiceFilter và TypeFilter trong ASP.NET Core Web API.
1. Đăng ký DI (DI Registration):
  - ServiceFilter: Yêu cầu lớp filter phải được đăng ký trong DI container.
  - TypeFilter: Không yêu cầu đăng ký trước; nó tạo instance của filter một cách tức thời (on the fly).
2. Đối số Constructor (Constructor Arguments):
  - ServiceFilter: Không hỗ trợ truyền đối số cho constructor một cách linh động.
  - TypeFilter: Hỗ trợ truyền đối số cho constructor thông qua thuộc tính Arguments.
3. Hiệu năng (Performance):
  - ServiceFilter: Sử dụng DI để resolve instance trực tiếp, có khả năng tốt hơn đối với các vòng đời scoped/singleton.
  - TypeFilter: Sử dụng ActivatorUtilities.CreateInstance bên trong, có thể tốn một chi phí hiệu năng nhỏ nhưng mang lại sự linh hoạt.
4. Trường hợp sử dụng (Use Case):
  - Sử dụng ServiceFilter khi filter là một service mà bạn muốn quản lý thông qua DI container, đặc biệt khi bạn muốn chia sẻ các instance hoặc quản lý vòng đời một cách rõ ràng.
  - Sử dụng TypeFilter khi bạn muốn áp dụng một filter mà không cần đăng ký trước trong DI hoặc khi bạn muốn truyền các tham số constructor cụ thể một cách linh động ngay tại nơi sử dụng filter.