using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.MSEnums;
using MS.ApplicationCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class DictionaryEnumService : IDictionaryEnumService
    {
        List<EnumDictionary> dics = new List<EnumDictionary>();
        public IEnumerable<EnumDictionary> GetExpenditurePlanType()
        {
            return GetListEnumDictionary(typeof(ExpenditurePlanType).GetEnumValues());
        }

        public IEnumerable<EnumDictionary> GetGenders()
        {
            return GetListEnumDictionary(typeof(Gender).GetEnumValues());
        }

        private IEnumerable<EnumDictionary> GetListEnumDictionary(Array enumProp)
        {
            foreach (var item in enumProp)
            {
                var itemString = CommonFunction.GetResourceStringByEnum(item as Enum);
                dics.Add(new EnumDictionary() { Text = itemString, Value = (int)item });
            }
            return dics;
        }
    }
}
