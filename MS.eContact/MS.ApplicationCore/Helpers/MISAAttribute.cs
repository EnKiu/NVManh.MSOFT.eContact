using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableName : Attribute
    {
        public string Name { get; set; }
        public DataTableName(string name)
        {
            Name = name;
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnName : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute
    {
        public string ErroValidate;
        public Required(string error = "")
        {
            this.ErroValidate = error;
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LabelText : Attribute
    {
        public string Text;
        public LabelText(string text)
        {
            this.Text = text;
        }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class EmailValid : Attribute
    {

    }


    /// <summary>
    /// Attribute để đánh dấu Propety bổ sung, bắt buộc phải đặt cho các prop hoặc field mà không có trong thiết kế của table tương ứng
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NotMapQuery : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NotDuplicate : Attribute
    {
        public string? Error;
        public NotDuplicate(string? error = null)
        {
            Error = error;
        }
    }
}
