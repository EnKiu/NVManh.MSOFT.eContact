using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MS.eContact.Web
{
    public class MySqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }

        public override Guid Parse(object value)
        {
            try
            {
                return new Guid((string)value);
            }
            catch (Exception)
            {
                return Guid.Parse(value.ToString());
            }

        }
    }
    public class MySqlGuidWithNullTypeHandler : SqlMapper.TypeHandler<Guid?>
    {
        public override void SetValue(IDbDataParameter parameter, Guid? guid)
        {
            if (guid == null)
                parameter.Value = null;
            else
                parameter.Value = guid.ToString();
        }

        public override Guid? Parse(object value)
        {
            try
            {
                return new Guid((string)value);
            }
            catch (Exception)
            {
                return Guid.Parse(value.ToString());
            }

        }
    }
}
