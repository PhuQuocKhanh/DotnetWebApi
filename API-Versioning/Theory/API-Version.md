-- API Versioning trong ASP.NET Core Web API là gì? -- 
- API Versioning trong ASP.NET Core Web API là kỹ thuật quản lý nhiều phiên bản của API, cho phép các client tiếp tục sử dụng phiên bản cũ trong khi các phiên bản mới được phát triển và triển khai. Khi chúng ta xây dựng API, các client như mobile app, web app hoặc hệ thống khác sẽ bắt đầu sử dụng chúng.
- Theo thời gian, yêu cầu thay đổi, bug được sửa, hoặc tính năng mới được bổ sung. Nếu chúng ta thay đổi hoặc loại bỏ một phần của API, client hiện tại có thể bị lỗi.
- API Versioning giúp chúng ta giới thiệu thay đổi mà không làm gián đoạn các client đang dựa vào phiên bản cũ.
- Trong ASP.NET Core, API Versioning là một cách tiếp cận có hệ thống, cho phép:
  - Xuất bản các phiên bản API mới (ví dụ: v1, v2) đồng thời vẫn giữ phiên bản cũ hoạt động.
  - Ngưng sử dụng (deprecate) hoặc gỡ bỏ phiên bản cũ khi đã an toàn.
  - Duy trì khả năng tương thích ngược và giảm rủi ro gây lỗi cho client hiện tại.

-- Tại sao cần Web API Versioning? -- 
1. Khi triển khai API, sẽ có nhiều client bắt đầu sử dụng
  - Một khi Web API được release, nhiều ứng dụng client khác nhau (mobile app, web app, tích hợp với bên thứ ba, desktop app, …) sẽ tiêu thụ API đó. 
  - Mỗi client thường được xây dựng dựa trên cấu trúc cụ thể của API, bao gồm endpoint, format request, và response data.

2. Yêu cầu kinh doanh luôn thay đổi
  - Khi doanh nghiệp phát triển, nhu cầu với API cũng thay đổi. 
  - Có thể cần thêm tính năng mới, cải tiến chức năng hiện có, hoặc sửa bug. 
  - Đôi khi những thay đổi này làm thay đổi dữ liệu trả về, cách gửi request, hoặc hành vi của endpoint.

3. Thách thức: Làm sao thay đổi API mà không làm hỏng client hiện có?
  - Nếu bạn thay đổi API theo cách không tương thích với client cũ, chúng sẽ bị lỗi. 
  - Ví dụ: đổi tên một field trong JSON response hoặc bỏ một tham số thì client cũ sẽ không parse được response hoặc gửi request sai định dạng. Đây là lý do API Versioning trở nên quan trọng.

4. Giải pháp lý tưởng: Giữ nguyên phiên bản cũ, giới thiệu phiên bản mới
  - Để đáp ứng yêu cầu mới mà không làm gián đoạn client cũ:
  - Giữ nguyên API phiên bản cũ để phục vụ các client đang phụ thuộc vào nó.
  - Đồng thời tạo phiên bản API mới với các thay đổi, tính năng mới. Các client mới (hoặc muốn nâng cấp) có thể bắt đầu dùng phiên bản này.

-- Lợi ích chính của Web API Versioning -- 
1. Backward Compatibility (Khả năng tương thích ngược)
  - Các client xây dựng trên phiên bản API cũ vẫn hoạt động bình thường, ngay cả khi phiên bản mới được thêm vào.
  - Ví dụ: Một mobile app phát triển năm ngoái dùng API v1.0. Nếu bạn đổi contract của API (bỏ/đổi tên field) mà không versioning, app sẽ crash. Với versioning, bạn có thể:
  - Giữ nguyên v1 để client cũ tiếp tục chạy.
  - Thêm logic mới vào v2 mà không lo phá vỡ client cũ.
2. Avoid Breaking Changes (Tránh thay đổi phá vỡ)
  - Breaking change là bất kỳ thay đổi nào trong API làm client cũ bị lỗi. Ví dụ:
  - Đổi tên hoặc xóa field trong response.
  - Thay đổi tham số bắt buộc trong request.
  - Đổi URL path hoặc HTTP method.
- Không có versioning → client sẽ hỏng ngay lập tức.
- Có versioning → thay đổi chỉ áp dụng cho phiên bản mới, phiên bản cũ không bị ảnh hưởng.
3. Smooth Migration (Hỗ trợ migration mượt mà)
  - Client có thể nâng cấp từ phiên bản cũ sang mới theo tốc độ riêng, thay vì buộc phải thay đổi ngay. Điều này quan trọng vì:
  - Ứng dụng lớn hoặc nhiều bên thứ ba cần thời gian test và update.
  - Buộc thay đổi ngay có thể gây downtime hoặc bug cho client.
  - API Versioning giúp migration có kiểm soát, giảm rủi ro và cải thiện trải nghiệm cho developer.
4. Multiple Client Support (Hỗ trợ nhiều loại client đồng thời)
  - Mỗi loại client có thể yêu cầu phiên bản khác nhau cùng lúc. Ví dụ:
    - Web app dùng API mới nhất với đầy đủ tính năng.
    - Mobile app cũ vẫn chạy trên API version cũ vì chưa update.
    - Đối tác thứ ba tích hợp với phiên bản API ổn định để đảm bảo compliance.
    - API Versioning cho phép chạy song song nhiều phiên bản để đáp ứng nhu cầu khác nhau mà không xung đột.

-- Các tùy chọn versioning trong ASP.NET Core Web API -- 
- ASP.NET Core hỗ trợ nhiều cách để triển khai API Versioning. Các tùy chọn phổ biến gồm:
  1. Query String Versioning: Truyền version qua query string của URL.
    - Ví dụ: api/products?api-version=1.0
  2. URL Path Versioning: Đặt version trực tiếp trong URL path.
    - Ví dụ: /api/v1/products
  3. Header Versioning: Truyền version qua custom header (thường dùng tên api-version).
    - Ví dụ: Header: api-version: 1.0
  4. Media Type Versioning (Accept Header Versioning): Truyền version trong header Accept (content negotiation).
    - Ví dụ: Accept: application/json;v=1.0

-- Tại sao Swagger không hoạt động khi dùng API Versioning? -- 
- Khi bật API Versioning trong ASP.NET Core Web API, thường gặp trường hợp nhiều action trong controller có cùng phương thức HTTP (GET, POST, …) và cùng template route (ví dụ /api/products), chỉ khác nhau bởi attribute [ApiVersion] chỉ định phiên bản API mà chúng thuộc về.
- Swagger (qua Swashbuckle) theo mặc định không hiểu các attribute versioning này và coi những endpoint đó là trùng lặp vì:
  - Nó dùng tổ hợp phương thức HTTP + route để nhận diện endpoint.
  - Vì phương thức và route của v1 và v2 giống nhau, Swagger coi đó là conflict/duplicate endpoint.
  - Điều này dẫn tới lỗi. Ví dụ: GetV1() và GetV2() đều trả về GET api/products nhưng khác phiên bản — Swagger nhìn thấy như cùng một endpoint xuất hiện hai lần.
  - Cách khắc phục: cấu hình Swagger để hỗ trợ API Versioning

- Cần phải chỉ rõ cho Swagger cách phân tách và tài liệu hóa các phiên bản API khác nhau bằng cách:
  - Tạo tài liệu Swagger (Swagger document) riêng cho mỗi phiên bản API, ví dụ một doc cho v1.0, một doc cho v2.0.
  - Lọc các endpoint đưa vào từng Swagger doc dựa trên attribute phiên bản của chúng, nghĩa là các endpoint thuộc v1 chỉ xuất hiện trong doc v1, các endpoint v2 chỉ vào doc v2.