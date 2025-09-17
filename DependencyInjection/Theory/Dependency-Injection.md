-- Dependency Injection (DI) là gì? -- 
 - Dependency Injection (DI) là một mẫu thiết kế (design pattern) được sử dụng để giảm sự phụ thuộc chặt chẽ (tight coupling) giữa các lớp và các dependency (thành phần phụ thuộc) của chúng.
- Nói đơn giản, DI cho phép một object (class) nhận các dependency (các service hoặc object mà nó cần để thực thi công việc) từ một nguồn bên ngoài (thường là DI Container) thay vì tự khởi tạo và quản lý chúng bên trong.
👉 Có 2 khái niệm chính:
  - Dependency: Đối tượng mà một đối tượng khác cần sử dụng.
  - Injection: Quá trình cung cấp dependency cho một class.
- DI giúp tách rời (decouple) các thành phần trong hệ thống để chúng dễ dàng thay thế, mở rộng và kiểm thử. 
- Thay vì một class tự tạo và quản lý dependency, DI sẽ inject (tiêm vào) dependency từ bên ngoài.

- Ví dụ (không dùng DI):
  - Một class tự khởi tạo dependency bên trong constructor hoặc method → dẫn đến hard-coded dependency, khó thay thế và test.
- Ví dụ (có DI):
  - Class chỉ khai báo dependency trong constructor, còn việc khởi tạo và quản lý vòng đời object sẽ do DI Container lo.

-- Tại sao cần sử dụng Dependency Injection trong ASP.NET Core Web API? -- 
- Loose Coupling: Class không cần tạo và quản lý dependency → chỉ cần khai báo, DI Container sẽ lo việc tạo và quản lý vòng đời.
- Dễ kiểm thử (Unit Test): Vì dependency được inject từ bên ngoài, nên có thể dễ dàng thay thế bằng mock hoặc stub khi viết test.
- Tăng tính linh hoạt: Có thể thay đổi hoặc thay thế implementation khác mà không cần chỉnh sửa code trong class sử dụng dependency.

-- Cơ chế hoạt động của Dependency Injection -- 
- DI thường bao gồm 3 thành phần:
  - Client: Class cần sử dụng dependency (còn gọi là dependent object).
  - Service: Dependency mà client cần (còn gọi là dependency object).
  - Injector: Thực hiện việc cấu hình và inject dependency, thường được implement bởi DI Container.
- 📌 Cách hoạt động:
  - DI Container tạo một instance của Service.
  - DI Container inject instance đó vào Client.
- Client sử dụng Service thông qua dependency đã được inject.
- Điều này giúp Client không còn trách nhiệm tạo hoặc quản lý dependency, chỉ tập trung vào logic nghiệp vụ của nó.

-- Cách Dependency Injection (DI) hoạt động trong ASP.NET Core Web API -- 
- ASP.NET Core tích hợp sẵn một DI container cho phép tự động resolve (giải quyết và cấp phát) dependency tại runtime.
- Để sử dụng, chúng ta cần đăng ký (register) các lớp (service) với một lifecycle (vòng đời) cụ thể (Singleton, Scoped, hoặc Transient) trong DI container. 
- Sau đó, DI container sẽ cung cấp (inject) dependency khi cần thiết, thường thông qua constructor injection (cách khuyến nghị nhất), hoặc thông qua property/method injection.

-- Quy trình DI cơ bản trong ASP.NET Core -- 
1. Đăng ký service (Register Services):
- Đăng ký các service (class) trong file Program.cs bằng các phương thức như AddTransient(), AddScoped(), hoặc AddSingleton().
2. Tiêm dependency (Inject Dependencies):
- Cung cấp dependency vào class thông qua constructor hoặc các cách khác. 
- Ví dụ: trong Controller hoặc Service khác, chỉ cần khai báo parameter trong constructor trùng với interface/class đã đăng ký.
3. Resolve dependency (Resolve Dependencies):
- DI container sẽ tự động khởi tạo và inject instance phù hợp khi được yêu cầu.

-- Service Lifetimes trong ASP.NET Core -- 
- DI container của ASP.NET Core hỗ trợ 3 loại vòng đời chính:
1. Singleton
  - Đặc điểm: Chỉ tạo một instance duy nhất trong toàn bộ vòng đời ứng dụng.
  - Ứng dụng: Thích hợp cho các service stateless, chứa config toàn cục, cache, hoặc dữ liệu dùng chung và không thay đổi giữa các request.
  - Ví dụ: Logging service, Configuration service.
    services.AddSingleton<IService, ServiceImplementation>();
    services.AddSingleton<ServiceImplementation>();
2. Scoped
  - Đặc điểm: Tạo một instance cho mỗi HTTP request.
  - Ứng dụng: Thích hợp cho service cần giữ trạng thái trong suốt một request nhưng không cần chia sẻ giữa nhiều request.
  - Ví dụ: DbContext trong ứng dụng web (mỗi request có một DbContext riêng).
  - Cú pháp:
    - services.AddScoped<IService, ServiceImplementation>();
    - services.AddScoped<ServiceImplementation>();
3. Transient
  - Đặc điểm: Tạo một instance mới mỗi lần được yêu cầu.
  - Ứng dụng: Thích hợp cho các service nhẹ, stateless, hoặc các thao tác ngắn hạn.
  - Ví dụ: Service sinh ID duy nhất, service gửi email.
  - Cú pháp:
    - services.AddTransient<IService, ServiceImplementation>();
    - services.AddTransient<ServiceImplementation>();

-- Triển khai Dependency Injection (DI) Design Pattern trong ASP.NET Core Web API --
1. Singleton: 
  - Dùng cho service quản lý cấu hình hoặc cache dùng chung toàn ứng dụng. 
  - Service này tồn tại trong suốt vòng đời của ứng dụng (chỉ khởi tạo một lần).
2. Scoped: 
  - gắn với từng request cụ thể. Mỗi request sẽ có một instance riêng, đảm bảo request của người dùng này không bị ảnh hưởng bởi của người khác dùng khác.
3. Transient: 
  - sinh ra mã đơn hàng duy nhất cho từng thao tác. 
  - Mỗi lần sử dụng sẽ tạo ra một instance mới, đảm bảo tính độc lập hoàn toàn giữa các lần gọi.