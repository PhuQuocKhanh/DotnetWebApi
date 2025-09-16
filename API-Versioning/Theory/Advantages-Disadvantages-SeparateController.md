- Ưu điểm khi tách Controller cho từng phiên bản API:
  - Cách ly hoàn toàn giữa các phiên bản: Mỗi phiên bản API hoạt động độc lập, thay đổi ở phiên bản này không ảnh hưởng đến phiên bản khác. Ví dụ: khi thêm tính năng ở v2, mã nguồn ở v1 vẫn giữ nguyên, không bị tác động.
  - Mã nguồn rõ ràng, dễ quản lý: Việc tách Controller giúp phân tách code theo từng phiên bản, lập trình viên có thể nhanh chóng tìm thấy logic của phiên bản cụ thể bằng cách xem đúng Controller tương ứng.
  - Dễ dàng kiểm thử độc lập: Mỗi phiên bản có thể viết unit test riêng, giảm độ phức tạp và tránh phụ thuộc lẫn nhau. Ví dụ: test cho v1 tách biệt hoàn toàn so với test cho v2, dễ dàng khoanh vùng và xử lý lỗi.

-- Nhược điểm khi tách Controller cho từng phiên bản API: -- 
  - Dễ dẫn đến trùng lặp logic: Một số logic chung phải viết lại ở nhiều Controller khác nhau. Ví dụ: cùng một đoạn validation dữ liệu người dùng, nếu cần ở cả v1 và v2 thì phải copy code ở cả hai Controller.
  - Tốn công quản lý khi nhiều phiên bản: Số lượng Controller sẽ tăng theo số phiên bản. Ví dụ: có 5 phiên bản API thì sẽ có ít nhất 5 Controller cần maintain, update, viết tài liệu và test.
  - Cấu trúc project trở nên cồng kềnh: Khi số lượng Controller nhiều, project dễ phình to và khó điều hướng. Ví dụ: việc tìm kiếm Controller hoặc quản lý dependency giữa các lớp sẽ phức tạp hơn về lâu dài.

-- Khi nào nên dùng Controller riêng cho từng phiên bản: -- 
  - Phù hợp với các dự án có sự thay đổi lớn giữa các phiên bản, cần tách biệt rõ ràng logic, và ưu tiên cách ly phiên bản.
  - Nhược điểm: Tăng sự trùng lặp code và chi phí bảo trì, nhưng bù lại giúp tách biệt và rõ ràng hơn.

-- Ưu điểm của Một Controller với các phương thức versioned: -- 
- Tái sử dụng logic chung dễ dàng giữa các phiên bản, giảm trùng lặp.
  - Ví dụ: Logic validate chung chỉ cần viết một lần và áp dụng cho nhiều phiên bản.
- Ít controller hơn để quản lý, làm cấu trúc dự án gọn gàng và dễ điều hướng hơn.
  - Ví dụ: Một controller xử lý nhiều phiên bản sẽ giảm số file và class phải duy trì.
- Refactor logic chung dễ hơn vì được tập trung ở một chỗ.
  - Ví dụ: Thay đổi một logic chung chỉ cần sửa ở một nơi, không phải ở nhiều controller.
- Tập trung toàn bộ business logic liên quan trong cùng controller, giúp dễ hình dung luồng xử lý tổng thể.
  - Ví dụ: Dev có thể xem toàn bộ các version của một endpoint trong cùng một controller để hiểu quá trình tiến hóa của API.

-- Nhược điểm của Một Controller với các phương thức versioned: -- 
- Controller dễ trở nên phức tạp và khó quản lý khi số lượng phiên bản tăng.
  - Ví dụ: Có nhiều phương thức cùng endpoint nhưng khác version sẽ làm controller rối rắm.
- Nếu không cẩn thận, thay đổi của một phiên bản có thể ảnh hưởng đến phiên bản khác.
  - Ví dụ: Chỉnh sửa logic chung có thể gây ra side-effect trên nhiều version nếu không cô lập đúng cách.

-- Khi nào nên dùng Một Controller với các phương thức versioned:
  - Phù hợp với các dự án có ít khác biệt giữa các phiên bản, ưu tiên tái sử dụng code và đơn giản hóa cấu trúc.
  - Nhược điểm: Controller phức tạp hơn và có nguy cơ xung đột logic giữa các phiên bản, nhưng bù lại giúp giảm trùng lặp code và cấu trúc project nhẹ nhàng hơn.