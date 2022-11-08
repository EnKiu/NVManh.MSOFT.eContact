using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IDictionaryEnumService
    {
        IEnumerable<EnumDictionary> GetGenders();
        IEnumerable<EnumDictionary> GetExpenditurePlanType();
    }
}
