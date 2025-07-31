-- Automapper Null Substitution in ASP.NET Core Web API --
- Trong ngữ cảnh sử dụng AutoMapper trong một dự án ASP.NET Core Web API, null substitution (thay thế giá trị null) là một tính năng cho phép bạn định nghĩa một giá trị mặc định cho thuộc tính đích nếu thuộc tính tương ứng từ đối tượng nguồn có giá trị null. 
- Nói cách khác, nó cho phép bạn chỉ định một giá trị thay thế sẽ được sử dụng khi giá trị từ nguồn là null trong quá trình ánh xạ.
- Tính năng này đặc biệt hữu ích trong các tình huống mà bạn muốn đảm bảo phía client (người dùng API) luôn nhận được một giá trị có ý nghĩa thay vì một giá trị null, điều này có thể gây ra lỗi hoặc khiến client hiểu sai dữ liệu.
- Điều này có nghĩa là NullSubstitute cho phép chúng ta cung cấp một giá trị thay thế cho một thuộc tính ở đối tượng đích nếu giá trị tương ứng ở đối tượng nguồn là null. Để làm điều này, chúng ta sử dụng phương thức NullSubstitute() trong cấu hình AutoMapper.

