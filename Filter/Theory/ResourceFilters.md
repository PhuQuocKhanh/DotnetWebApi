-- Resource Filter trong ASP.NET Core Web API là gì? --
- Trong ASP.NET Core Web API, Resource Filter là một loại bộ lọc đặc biệt cho phép chúng ta chạy logic tùy chỉnh ở giai đoạn rất sớm (ngay sau Authorization filter) và rất muộn (sau Result filter) trong pipeline xử lý request. 
- Các mục đích chính của Resource Filter là:
  1. Quản lý Tài nguyên (Resource Management): 
    - Chúng có thể chuẩn bị, quản lý hoặc dọn dẹp các tài nguyên (như kết nối cơ sở dữ liệu, tệp tin, hoặc cache) cần thiết cho một request.
  2. Ngắt mạch Request (Short-circuiting Requests): 
    - Chúng có thể kiểm tra xem một response (như dữ liệu từ cache) có thể được trả về ngay lập tức hay không, giúp bỏ qua phần còn lại của pipeline để tăng hiệu suất.
  3. Tiền xử lý/Hậu xử lý (Pre/Post-Processing): 
    - Chúng có thể thực hiện các hoạt động ở cả đầu và cuối của một request, làm cho chúng trở nên hữu ích cho các tác vụ như caching, giới hạn tần suất truy cập (rate limiting), hoặc kiểm tra hợp lệ ban đầu.
- Một Resource Filter giống như người gác cổng ở trung tâm thương mại, người có cơ hội đầu tiên để xem xét một request HTTP đang đến và cũng là người cuối cùng trước khi response rời đi.

-- Resource Filter hoạt động như thế nào trong ASP.NET Core Web API? -- 
- Resource Filter là các thành phần đặc biệt cho phép chúng ta thực thi mã tùy chỉnh tại hai thời điểm quan trọng trong quá trình xử lý một request HTTP:
  - Trước khi hầu hết quá trình xử lý bắt đầu (ngay sau bước ủy quyền, trước model binding, action filter, và action method).
  - Sau khi tất cả quá trình xử lý đã hoàn tất, ngay trước khi response được gửi lại cho client.
  - Chúng thực hiện điều này bằng cách triển khai interface IResourceFilter (cho mã đồng bộ) hoặc IAsyncResourceFilter (cho mã bất đồng bộ).
  - Interface IResourceFilter định nghĩa hai phương thức chính, và IAsyncResourceFilter định nghĩa một phương thức mà ASP.NET Core sẽ gọi trong vòng đời của request:

1. OnResourceExecuting/OnResourceExecutionAsync (Trước khi thực thi Action)
  - Phương thức này được gọi ngay sau khi các Authorization Filter đã được thông qua, nhưng trước khi action method và các bộ lọc khác (như action filter hoặc model binding) chạy. 
  - Nó cho bạn cơ hội đầu tiên để kiểm tra, sửa đổi hoặc ngắt mạch request. 
  - Bạn nhận được một đối tượng ResourceExecutingContext, cho phép bạn:
    - Truy cập HttpContext và dữ liệu request.
    - Gán một IActionResult (như một response JSON hoặc mã trạng thái) cho context.Result để ngắt mạch pipeline.
    - Ngắt mạch (Short-circuiting) có nghĩa là:
      - Nếu bạn gán một giá trị cho context.Result trong phương thức này, framework sẽ không chạy action method hoặc bất kỳ bộ lọc nào khác sau đó. Thay vào đó, nó sẽ trực tiếp gửi response này về cho client. Điều này hữu ích khi bạn muốn:
    - Trả về một response từ cache ngay lập tức mà không cần chạy action.
   - Từ chối một request sớm do xác thực thất bại.
2. OnResourceExecuted (Sau khi thực thi Action)
  - Phương thức này được gọi sau khi action method đã chạy và tạo ra một kết quả, trước khi response được gửi đến client. 
  - Nó nhận một đối tượng ResourceExecutedContext, cho phép chúng ta:
    - Kiểm tra hoặc sửa đổi response.
    - Thực hiện các tác vụ dọn dẹp như giải phóng tài nguyên, đóng kết nối cơ sở dữ liệu hoặc ghi log.

-- Luồng xử lý Request với Resource Filter -- 
Luồng xử lý với Resource Filter như sau:
  - Request đến máy chủ.
  - Authorization Filter chạy đầu tiên để đảm bảo người dùng/request được ủy quyền.
  - Resource Filter’s OnResourceExecuting chạy tiếp theo.
    - Bạn có thể kiểm tra điều kiện hoặc trả về dữ liệu cache ngay tại đây.
    - Nếu bạn gán giá trị cho context.Result ở đây, pipeline sẽ ngắt mạch và bỏ qua tất cả các bộ lọc và action method tiếp theo.
    - Nếu không bị ngắt mạch:
      - Model Binding diễn ra (ánh xạ dữ liệu HTTP vào các tham số của action).
  - Action Filter chạy (trước và sau action).
  - Action Method được thực thi.
  - Result Filter chạy (trước khi kết quả được hoàn thiện/gửi đi).
  - Resource Filter’s OnResourceExecuted chạy để hoàn tất hoặc dọn dẹp.
  - Exception Filter chạy nếu có bất kỳ ngoại lệ nào chưa được xử lý xảy ra trong quá trình thực thi action, result hoặc resource filter.
  - Response được gửi đến client.

-- Khi nào nên sử dụng IResourceFilter trong ASP.NET Core Web API? -- 
- Chúng ta nên sử dụng interface IResourceFilter khi logic của resource filter là đồng bộ và không yêu cầu bất kỳ hoạt động bất đồng bộ nào. 
- IResourceFilter lý tưởng cho các kịch bản mà các hành động cần thực hiện, như ngắt mạch request, cache response, xác thực header của request, hoặc ghi log truy cập tài nguyên, có thể hoàn thành nhanh chóng và không liên quan đến các tác vụ tốn thời gian hoặc I/O-bound như gọi cơ sở dữ liệu, yêu cầu API từ xa, hoặc các hoạt động trên hệ thống tệp. 
- Sử dụng IResourceFilter trong những trường hợp này giúp cho việc triển khai bộ lọc của chúng ta hiệu quả và đơn giản, vì framework không phải xử lý việc quản lý tác vụ hay chuyển đổi ngữ cảnh bất đồng bộ.

-- Tại sao chúng ta cần Resource Filter trong ASP.NET Core? -- 
- Resource Filter cung cấp cho bạn một cơ hội mạnh mẽ để chặn, sửa đổi hoặc ngắt mạch các request HTTP từ sớm, ngay cả trước khi model binding và thực thi action. 
- Chúng lý tưởng khi bạn muốn cải thiện hiệu suất, thực thi các chính sách hoặc quản lý tài nguyên một cách toàn cục.

1. Caching Response – Cung cấp dữ liệu nhanh mà không cần xử lý thêm
  - Hãy tưởng tượng một API tin tức trực tuyến cung cấp các tiêu đề mới nhất. 
  - Việc tạo ra các tiêu đề này đòi hỏi phải truy vấn nhiều cơ sở dữ liệu và chạy các thuật toán phức tạp. 
  - Nếu các tiêu đề cho một khu vực cụ thể đã được cache, resource filter có thể trả về ngay lập tức các tiêu đề đã cache mà không cần gọi đến action của controller hoặc thực thi các bộ lọc khác.
  - Điều này tránh được các lượt truy cập cơ sở dữ liệu không cần thiết, giảm tải cho máy chủ và cải thiện đáng kể thời gian phản hồi.
  - Lợi ích: Bằng cách ngắt mạch request và phục vụ dữ liệu cache sớm, resource filter tối ưu hóa hiệu suất và khả năng mở rộng.
2. Quản lý tài nguyên – Sử dụng và dọn dẹp tài nguyên dùng chung một cách hiệu quả
  - Hãy xem xét một API tài chính mở một giao dịch cơ sở dữ liệu (database transaction) trước khi xử lý request của người dùng để đảm bảo tính nhất quán. 
  - Một resource filter có thể mở một giao dịch cơ sở dữ liệu ở đầu quá trình xử lý request. 
  - Sau khi action thực thi, resource filter sẽ commit hoặc rollback giao dịch tùy thuộc vào việc thành công hay thất bại. 
  - Điều này đảm bảo toàn bộ request chạy trong một phạm vi giao dịch duy nhất, cung cấp tính nguyên tử (atomicity) và quản lý tài nguyên nhất quán.
  - Lợi ích: Resource filter giúp phân bổ tài nguyên một lần cho mỗi request và dọn dẹp chúng một cách đáng tin cậy sau đó, tránh rò rỉ hoặc trạng thái không nhất quán.
3. Ngắt mạch Request – Xử lý các trường hợp đơn giản ngay lập tức
  - Một API yêu cầu xác thực API key trước khi xử lý request. 
  - Resource filter kiểm tra API key ngay từ đầu. 
  - Nếu key không hợp lệ hoặc bị thiếu, bộ lọc sẽ ngay lập tức trả về một response lỗi, ngăn không cho request đi xa hơn. 
  - Điều này tiết kiệm thời gian CPU và băng thông bằng cách tránh công việc không cần thiết ở các bước sau.
  - Lợi ích: Việc từ chối sớm các request không hợp lệ này giúp tăng cường bảo mật và hiệu quả tài nguyên.
4. Giới hạn tần suất truy cập – Ngăn chặn quá tải
  - Một API được công khai áp dụng giới hạn tần suất để tránh lạm dụng. Resource filter kiểm tra xem client đã vượt quá hạn ngạch request cho phép của họ chưa. 
  - Nếu đã vượt quá, nó sẽ trả về response 429 Too Many Requests ngay lập tức. 
  - Nếu không, nó cho phép request tiếp tục.
  - Lợi ích: Bảo vệ backend API của bạn khỏi quá tải, đảm bảo việc sử dụng công bằng.
5. Tiền xử lý Header hoặc Token xác thực – Từ chối sớm
  - API của bạn xác thực hoặc giải mã các header tùy chỉnh trước khi model binding hoặc thực thi action. 
  - Một resource filter kiểm tra và xử lý các header này ngay từ đầu. Nó có thể từ chối các request có header không đúng định dạng từ sớm hoặc sửa đổi chúng để xử lý ở các bước sau.
  - Lợi ích: Giữ cho việc xác thực và tiền xử lý được sạch sẽ và tập trung.