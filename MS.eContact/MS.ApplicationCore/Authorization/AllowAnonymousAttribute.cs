using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Authorization
{
    /// <summary>
    /// Attribute tùy chỉnh cho phép gán vào các phương thức muốn bỏ qua việc xác thực
    /// Khi attribute này được gán thì nó phương thức đó cho phép truy cập ẩn danh.
    /// Thay vì sử dụng thuộc tính [AllowAnonymous] mặc định
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowAnonymousAttribute : Attribute
    { }
}
