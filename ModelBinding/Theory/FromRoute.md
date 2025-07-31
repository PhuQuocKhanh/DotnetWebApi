-- What is FromRoute Attribute in ASP.NET Core Web API? --
- Thuộc tính FromRoute được sử dụng để chỉ định rằng một tham số trong phương thức action sẽ được binding (liên kết dữ liệu) từ route data trong URL. 
- Nó thông báo cho hệ thống Model Binding của ASP.NET Core rằng giá trị của tham số nên được lấy từ các tham số trong định tuyến (route), thay vì từ query string hoặc request body. 
- Khi chúng ta định nghĩa route có sử dụng placeholder (chẳng hạn như {id} trong đường dẫn URL), FromRoute cho phép trích xuất giá trị của placeholder đó và truyền nó vào tham số của phương thức trong controller.

-- How Does the FromRoute Attribute Work in ASP.NET Core Web API? --
- Khi một tham số trong action method được đánh dấu bằng thuộc tính FromRoute, hệ thống Model Binding của ASP.NET Core sẽ cố gắng lấy giá trị tương ứng từ dữ liệu định tuyến (route data – tức phần giá trị có trong URL) và gán nó vào tham số tương ứng trong phương thức.

-- When Should We Use FromRoute Attribute in ASP.NET Core Web API? --
- Chúng ta nên sử dụng thuộc tính FromRoute khi cần trích xuất và sử dụng các đoạn cụ thể trong URL để truyền vào tham số của action method. FromRoute đặc biệt hữu ích trong các trường hợp sau:
  - Khi cần lấy các định danh (ID) của tài nguyên, chẳng hạn như user ID hoặc product ID, được truyền trực tiếp trong đường dẫn URL.
  - Khi triển khai các định tuyến kiểu RESTful, nơi các tham số quan trọng được nhúng trong URL và được dùng để xác định hành động phù hợp.