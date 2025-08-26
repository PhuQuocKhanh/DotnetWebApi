-- Filter trong ASP.NET Core Web API là gì? -- 
- Trong ASP.NET Core Web API, Filter (Bộ lọc) là các thành phần có thể tái sử dụng (dưới dạng attribute hoặc class) cho phép lập trình viên thực thi logic tùy chỉnh trước hoặc sau khi một phương thức hành động (action method) trong controller được gọi. 
- Filter giúp tách biệt các mối quan tâm (concerns) như xác thực dữ liệu (validation), ghi nhật ký (logging), bảo mật (security), và xử lý ngoại lệ (exception handling), làm cho code trở nên module hóa và dễ bảo trì hơn.
- Hãy tưởng tượng Filter giống như các trạm kiểm soát an ninh tại sân bay. Trước khi một hành khách (yêu cầu HTTP) có thể lên máy bay (action của controller), họ phải đi qua nhiều trạm kiểm soát (filter), mỗi trạm thực hiện một nhiệm vụ cụ thể, chẳng hạn như kiểm tra giấy tờ (xác thực - authentication), quét hành lý (xác thực dữ liệu - validation), và kiểm tra thẻ lên máy bay (ủy quyền - authorization). 
- Filter hoạt động như những trạm kiểm soát này, xử lý các tác vụ trước hoặc sau khi logic nghiệp vụ chính (action method) chạy.

-- Tại sao chúng ta cần Filter trong ASP.NET Core Web API? -- 
- Filter cho phép chúng ta tách các mối quan tâm như logging, validation, xử lý ngoại lệ và bảo mật thành các thành phần riêng biệt để không phải lặp lại các kiểm tra này trong mỗi action của controller.
- Bằng cách đóng gói các tác vụ này trong filter, chúng ta giữ cho các action method tập trung vào logic nghiệp vụ, cải thiện khả năng đọc code và giảm thiểu sự trùng lặp.
- Hãy tưởng tượng bạn đang điều hành một thư viện và muốn mọi người khử khuẩn tay trước khi vào. 
- Thay vì lặp lại hướng dẫn cho từng khách truy cập, bạn thiết lập một trạm khử khuẩn (filter) ở lối vào, đảm bảo mọi người đều tự động khử khuẩn. 
- Tương tự, filter giảm thiểu code lặp lại bằng cách tập trung các kiểm tra như validation hoặc logging trên nhiều action, giúp code tập trung vào logic nghiệp vụ và dễ bảo trì hơn.
- Chúng ta đã biết rằng khi client tạo một request, request đó sẽ đến Routing Engine, sau đó điều hướng Request đến Controller Action Method. Action method sẽ xử lý request đến và gửi response trả về cho client.

-- Các loại Filter trong ASP.NET Core Web API --
1. Bộ lọc Ủy quyền (Authorization Filters)
- Authorization filter chạy đầu tiên trong đường ống filter (filter pipeline). 
- Chúng xử lý các mối quan tâm về bảo mật như đảm bảo request được ủy quyền để tiếp tục. 
- Các filter này chủ yếu được sử dụng để xác thực người dùng và kiểm tra xem người dùng có các quyền cần thiết (vai trò, claims, v.v.) để truy cập một tài nguyên hoặc endpoint cụ thể hay không.

  - Cách hoạt động: Authorization Filter xác minh xem người dùng đã được xác thực (authenticated) và ủy quyền (authorized) hay chưa.
    - Ví dụ, nó kiểm tra xem người dùng có token hợp lệ không và vai trò của người dùng có khớp với vai trò yêu cầu để truy cập tài nguyên hay không.
  - Ví dụ: Attribute [Authorize] là một authorization filter tích hợp sẵn, được áp dụng cho các action hoặc controller để đảm bảo chỉ những người dùng đã được xác thực và ủy quyền mới có thể truy cập các endpoint nhất định.
  - Trường hợp sử dụng thực tế: Trong một endpoint API của trang quản trị, bạn có thể áp dụng filter [Authorize(Roles = “Admin”)] để đảm bảo chỉ những người dùng có vai trò "Admin" mới có thể truy cập. 
  - Nếu người dùng không có vai trò "Admin" cố gắng truy cập, filter sẽ chặn họ và trả về phản hồi 403 Forbidden.
2. Bộ lọc Hành động (Action Filters)
- Action filter thực thi trước và sau khi một action method chạy. Chúng thường được sử dụng cho các tác vụ như logging, sửa đổi dữ liệu đầu vào hoặc đầu ra, hoặc đo lường hiệu suất.
  - Cách hoạt động: Action Filter chạy tại hai thời điểm. Trước khi action method được thực thi, chúng cho phép bạn sửa đổi dữ liệu đầu vào hoặc thực hiện validation. Sau khi action method hoàn thành, chúng cho phép bạn sửa đổi kết quả (action result) hoặc ghi log đầu ra.
  - Ví dụ: Một action filter ghi lại mọi request đến và các tham số của nó cho mục đích kiểm toán (auditing).
  - Trường hợp sử dụng thực tế: Trong một ứng dụng thương mại điện tử, bạn có thể ghi lại mọi hoạt động của người dùng bằng action filter. Mỗi khi người dùng thực hiện một hành động như thêm sản phẩm vào giỏ hàng hoặc mua hàng, action filter có thể ghi lại request và các tham số vào một file log hoặc cơ sở dữ liệu để kiểm toán.

3. Bộ lọc Kết quả (Result Filters)
- Result filter thực thi sau khi action method đã chạy nhưng trước khi response được gửi đến client. Chúng hữu ích để sửa đổi hoặc ghi log phản hồi cuối cùng, chẳng hạn như thêm header, sửa đổi nội dung body, hoặc nén các payload phản hồi lớn.
  - Cách hoạt động: Result Filter được thực thi sau khi action method trả về kết quả. Nó cho phép bạn sửa đổi kết quả trước khi nó được gửi đi, chẳng hạn như thêm các header tùy chỉnh, thay đổi nội dung phản hồi, hoặc nén dữ liệu phản hồi.
  - Ví dụ: Một result filter nén dữ liệu JSON lớn trước khi trả về cho client.
  - Trường hợp sử dụng thực tế: Giả sử bạn đang xây dựng một API trả về danh sách sản phẩm. Nếu danh sách rất lớn, bạn có thể áp dụng một result filter để nén phản hồi nhằm tăng hiệu quả truyền tải qua mạng. Ví dụ, bạn có thể áp dụng nén gzip cho phản hồi để giảm kích thước payload.

4. Bộ lọc Ngoại lệ (Exception Filters)
- Exception filter bắt và xử lý bất kỳ ngoại lệ chưa được xử lý (unhandled exceptions) nào được ném ra bởi các action method. Chúng được sử dụng để quản lý lỗi một cách tập trung, trả về thông báo lỗi tùy chỉnh hoặc ghi lại chi tiết lỗi.
  - Cách hoạt động: Exception Filter được kích hoạt khi một ngoại lệ chưa được xử lý xảy ra trong quá trình thực thi action. Bạn có thể bắt các ngoại lệ này và quyết định cách xử lý chúng, chẳng hạn như ghi log lỗi hoặc trả về một thông báo lỗi thân thiện cho client.
  - Ví dụ: Một exception filter bắt lỗi kết nối cơ sở dữ liệu và trả về một thông báo lỗi tùy chỉnh.
  - Trường hợp sử dụng thực tế: Nếu người dùng gửi một ID không hợp lệ trong request body và action method cố gắng truy vấn cơ sở dữ liệu với ID đó, một exception filter có thể bắt lỗi kết quả. Sau đó, nó có thể trả về mã trạng thái 400 Bad Request cùng với một thông báo chi tiết về sự cố (ví dụ: "ID sản phẩm không hợp lệ").

5. Bộ lọc Tài nguyên (Resource Filters)
- Resource filter chạy trước tất cả các filter khác trong pipeline (ngoại trừ Authorization Filter trong một số trường hợp). 
- Chúng thường được sử dụng cho các tác vụ quản lý tài nguyên như caching, hoặc xử lý các request trước khi chúng đến action method.
  - Cách hoạt động: Resource Filter được thực thi trước authorization filter và action filter. Chúng thường được sử dụng cho các tác vụ như caching kết quả hoặc quản lý tài nguyên, ví dụ như giới hạn số lượng request đồng thời đến một tài nguyên.
  - Ví dụ: Một resource filter kiểm tra xem dữ liệu có trong bộ nhớ đệm (cache) hay không và trả về dữ liệu đã cache nếu có, ngăn chặn việc xử lý hoặc gọi cơ sở dữ liệu không cần thiết.
  - Trường hợp sử dụng thực tế: Trong một API dự báo thời tiết, một resource filter có thể cache kết quả dữ liệu thời tiết cho một vị trí cụ thể. Nếu người dùng thực hiện một request tiếp theo cho cùng một vị trí trong một khoảng thời gian nhất định, filter có thể trả về dữ liệu đã cache thay vì truy vấn lại cơ sở dữ liệu, do đó cải thiện hiệu suất và giảm tải cho CSDL.

-- Thứ tự thực thi mặc định của Filter -- 
- Thứ tự thực thi mặc định cho từng loại filter như sau:
  1. Authorization Filters: Chạy đầu tiên để đảm bảo request được xác thực và ủy quyền.
  2. Resource Filters: Chạy tiếp theo để quản lý tài nguyên, chẳng hạn như kiểm tra cache.
  3. Action Filters: Chạy trước và sau khi action method thực thi. Dùng cho logging, sửa đổi đầu vào/đầu ra, hoặc validation.
  4. Result Filters: Chạy sau khi action hoàn thành, ngay trước khi response được gửi đi. Dùng để sửa đổi hoặc ghi log phản hồi cuối cùng.
  5. Exception Filters: Chạy cuối cùng và xử lý bất kỳ ngoại lệ nào chưa được xử lý.

-- Filter hoạt động như thế nào trong ASP.NET Core Web API? --
Filter hoạt động bằng cách cắm logic tùy chỉnh vào các giai đoạn cụ thể của pipeline xử lý request của ASP.NET Core. Hãy xem một ví dụ thực tế sau:
  1. Authorization Filter: 
    - Tưởng tượng người dùng đang cố gắng truy cập một trang quản trị an toàn. 
    - Authorization Filter trước tiên sẽ kiểm tra xem người dùng đã đăng nhập chưa và có vai trò ‘Admin’ cần thiết không. 
    - Nếu người dùng không được ủy quyền, filter sẽ chặn request và trả về phản hồi 403 Forbidden.
  2. Resource Filter: 
    - Tiếp theo, Resource Filter kiểm tra xem dữ liệu (ví dụ: danh sách người dùng) đã được cache hay chưa. 
    - Nếu có, filter sẽ phục vụ dữ liệu từ cache, tiết kiệm thời gian và giảm tải cho cơ sở dữ liệu.
  3. Action Filter: 
    - Action Filter ghi lại chi tiết request, như các tham số được gửi, và xác thực rằng dữ liệu đầu vào là chính xác trước khi chuyển nó cho action method.
  4. Result Filter: 
    - Sau khi action method chạy xong, Result Filter kiểm tra xem phản hồi có cần được sửa đổi không (ví dụ: thêm header tùy chỉnh, nén dữ liệu lớn).
  5. Exception Filter: 
    - Cuối cùng, nếu có bất kỳ ngoại lệ nào xảy ra trong quá trình này (ví dụ: lỗi cơ sở dữ liệu), Exception Filter sẽ bắt chúng và gửi một thông báo lỗi tùy chỉnh.

-- Chúng ta có thể áp dụng Filter ở đâu? -- 
- Trong ASP.NET Core Web API, filter có thể được áp dụng ở các cấp độ khác nhau dựa trên phạm vi và yêu cầu: toàn cục (globally), cấp độ controller (controller level), và cấp độ action (action level).

1. Áp dụng Filter ở cấp độ Toàn cục (Globally)
- Khi bạn áp dụng một filter toàn cục, nó sẽ ảnh hưởng đến tất cả các controller và action method trong toàn bộ ứng dụng. 
- Filter toàn cục thường được sử dụng cho các mối quan tâm áp dụng phổ biến trên toàn ứng dụng, chẳng hạn như xác thực, logging, hoặc xử lý ngoại lệ.
  - Cách hoạt động: Filter toàn cục được đăng ký trong tệp Program.cs (hoặc Startup.cs cho các phiên bản cũ hơn) để đảm bảo chúng chạy cho mọi request HTTP.
  - Khi nào sử dụng: Khi bạn cần thực thi một quy tắc hoặc hành vi trên toàn bộ ứng dụng. Ví dụ, nếu tất cả các request đến Web API của bạn cần được xác thực, bạn sẽ áp dụng một authorization filter toàn cục.
  - Trường hợp sử dụng thực tế: Áp dụng filter logging toàn cục để ghi lại mọi request và thông tin như tham số, thời gian thực thi, và trạng thái phản hồi cho tất cả các action trong ứng dụng.

2. Áp dụng Filter ở cấp độ Controller
- Filter được áp dụng ở cấp độ controller sẽ ảnh hưởng đến tất cả các action trong controller đó. 
- Cấp độ này được sử dụng khi bạn cần hành vi cụ thể cho một nhóm các action liên quan trong một controller.
  - Cách hoạt động: Khi bạn áp dụng một filter ở cấp độ controller, nó sẽ chạy cho mọi action trong controller đó.
  - Khi nào sử dụng: Khi bạn muốn áp dụng filter cho tất cả các action trong một controller, nhưng không phải cho toàn bộ ứng dụng. Ví dụ, bạn có thể muốn ghi log tất cả các action trong một AdminController nhưng không phải cho mọi action trong toàn bộ Web API.
  - Trường hợp sử dụng thực tế: Áp dụng một authorization filter chỉ cho các controller liên quan đến các hoạt động nhạy cảm (ví dụ: AdminController), đảm bảo chỉ người dùng được ủy quyền mới có thể truy cập.

3. Áp dụng Filter ở cấp độ Action
- Filter được áp dụng ở cấp độ action là cụ thể nhất và chỉ được áp dụng cho một action method duy nhất.
- Điều này hữu ích cho việc xử lý chuyên biệt chỉ áp dụng cho một phương thức.
  - Cách hoạt động: Khi được áp dụng cho một action, filter chỉ chạy khi action cụ thể đó được gọi.
  - Khi nào sử dụng: Khi bạn cần hành vi rất cụ thể cho một action cụ thể, chẳng hạn như khi một action cần xử lý ngoại lệ hoặc cơ chế logging khác với các action khác.
  - Trường hợp sử dụng thực tế: Áp dụng một validation filter trên một action cụ thể chấp nhận dữ liệu nhạy cảm hoặc phức tạp, chẳng hạn như một action xử lý thanh toán, nơi bạn có thể muốn thực hiện xác thực bổ sung trên dữ liệu đầu vào.

-- Điều gì xảy ra khi cùng một Filter được áp dụng ở nhiều cấp độ? -- 
- Nếu cùng một filter được áp dụng ở nhiều cấp độ (toàn cục, controller, và action), filter đó sẽ được thực thi nhiều lần, mỗi lần cho một cấp độ mà nó được áp dụng. 
- Ví dụ, một filter logging hoặc authorization được áp dụng ở cả ba cấp độ sẽ chạy ba lần trong một request duy nhất đến action đó. 
- Điều này có thể gây ra các hoạt động dư thừa hoặc lặp đi lặp lại trừ khi bạn thiết kế filter của mình để ngăn chặn việc thực thi trùng lặp.

-- Sự khác biệt giữa Filter và Middleware --
1. Middleware
- Middleware là một thành phần toàn cục được thực thi sớm trong pipeline request. 
- Nó hoạt động trên tất cả các request và response HTTP, có nghĩa là nó có thể sửa đổi hoặc kiểm tra request/response cho bất kỳ action hoặc controller nào trong ứng dụng. 
- Middleware chịu trách nhiệm xử lý các mối quan tâm áp dụng toàn cục, như định tuyến (routing), bảo mật, xử lý ngoại lệ, hoặc sửa đổi response.
  - Cách hoạt động: 
    - Middleware chạy cho mọi request đi vào hệ thống và được xử lý theo thứ tự đăng ký. 
    - Mỗi middleware có quyền truy cập vào request, và nó có thể sửa đổi request, chuyển nó đi tiếp trong pipeline, hoặc dừng request hoàn toàn.
  - Khi nào sử dụng:
    - Xác thực và ủy quyền (Authentication/Authorization) ở cấp độ toàn cục.
    - CORS (Cross-Origin Resource Sharing).
    - Ghi log request.
    - Xử lý ngoại lệ toàn cục.
  - Tương tự trong thực tế: 
    - Hãy tưởng tượng một đèn giao thông trên một con đường. 
    - Trước khi bất kỳ chiếc xe nào (request) đi tiếp, nó phải dừng lại ở đèn tín hiệu (middleware). 
    - Đèn tín hiệu có thể cho xe đi qua, bắt xe chờ, hoặc chuyển hướng nó dựa trên các quy tắc chung. 
    - Quá trình này được áp dụng cho tất cả các xe trên con đường đó.
2. Filters
- Filter là các thành phần cụ thể hơn, gắn với action. 
- Chúng thực thi trước hoặc sau khi một action method trong controller chạy. 
- Filter cho phép lập trình viên áp dụng logic liên quan cụ thể đến việc thực thi của action method, chẳng hạn như ghi log tham số, xác thực dữ liệu model, hoặc xử lý lỗi.
  - Cách hoạt động: 
    - Filter được kích hoạt bởi các action cụ thể và chạy trước hoặc sau khi phương thức controller được thực thi. 
    - Chúng có thể được áp dụng ở cấp độ toàn cục, controller, hoặc action, cho phép bạn kiểm soát chi tiết hơn.
  - Khi nào sử dụng:
    - Validation dành riêng cho action.
    - Ghi log tham số đầu vào hoặc kết quả đầu ra để gỡ lỗi.
    - Xử lý ngoại lệ ở cấp độ action.
    - Kiểm tra ủy quyền cho các action cụ thể.
  - Tương tự trong thực tế: 
    - Hãy coi filter như các trạm kiểm soát cụ thể dọc theo một con đường. Tại mỗi trạm kiểm soát (filter), chiếc xe (request) có thể được kiểm tra những thứ cụ thể, chẳng hạn như:
    - Trạm ủy quyền: Xe có được đăng ký không (authorization filter)?
    - Trạm an toàn: Xe có đáp ứng tiêu chuẩn an toàn không (action filter)?
    - Trạm cuối: Xe đã sẵn sàng rời khỏi con đường chưa (result filter)?
    - Không giống như middleware là toàn cục, filter là các kiểm tra có mục tiêu chỉ áp dụng cho các phần cụ thể của hành trình (action hoặc controller).

Bảng so sánh chính
Tiêu chí	                        Middleware	                                                                                                                            Filters
Phạm vi	Toàn cục.                   Áp dụng cho mọi request.	                                                                                    Gắn với MVC/API. Có thể áp dụng toàn cục, cấp controller, hoặc cấp action.
Ngữ cảnh	                        Hoạt động trên HttpContext. Không biết về action hoặc model binding.	                                        Có quyền truy cập vào ngữ cảnh của action (ActionContext), bao gồm model, route data...
Thứ tự thực thi	                    Chạy sớm trong pipeline, trước khi định tuyến quyết định controller nào sẽ xử lý.	                            Chạy muộn hơn, là một phần của pipeline MVC/API sau khi controller/action đã được chọn.
Trường hợp sử dụng	                Xử lý các mối quan tâm xuyên suốt (cross-cutting concerns) cấp thấp như authentication, static files, CORS.	    Xử lý các mối quan tâm liên quan đến logic của action, như validation, caching, định dạng response.
