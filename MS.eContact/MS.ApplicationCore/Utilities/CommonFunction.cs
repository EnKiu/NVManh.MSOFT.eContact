﻿using Microsoft.AspNetCore.Http;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Utilities
{
    public class CommonFunction: ICommonFunction
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static string LanguageCode = "vi-VN";
        private static string? OrganizationId = null;
        public CommonFunction(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string GetCurrentOrganozationId()
        {
            return OrganizationId;
        }

        public static void SetCurrentOrganozationId(string orgId)
        {
            OrganizationId = orgId;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User?.Claims?.First(x => x.Type == "id").Value;
            return userId;
        }

        public static string? GetResourceString(string resourceKey)
        {
            var culture = CultureInfo.CreateSpecificCulture(CommonFunction.LanguageCode);
            //ResourceManager rm = new
            //    ResourceManager("MISA.FM.Core.Resource", assembly:Resource.Resource);

            ResourceManager rm = new
                ResourceManager(typeof(MS.ApplicationCore.Resource.Resource));

            var resourceString = rm.GetString(resourceKey, culture);
            return resourceString;
            //btnHello.Text = rm.GetString("hello", culture);
            //radEnglish.Text = rm.GetString("english", culture);
            //radVietnamese.Text = rm.GetString("vietnamese", culture);
            //helloWorldString = rm.GetString("helloworld", culture);
        }

        public static string? GetResourceStringByEnum(Enum? misaEnum)
        {
            if (misaEnum == null)
                return null;
            var enumName = misaEnum.GetType().Name;
            var enumItemName = misaEnum.ToString();
            var resourceKey = $"Enum_{enumName}_{enumItemName}";
            return CommonFunction.GetResourceString(resourceKey);
        }

        /// <summary>
        /// Hàm chuyển các ký tự unicode thành ký tự không dấu, viết liền và viết thường (mục đích để compare gần đúng 2 chuỗi ký tự)
        /// </summary>
        /// <param name="text">Chuỗi ký tự</param>
        /// <returns>Chuỗi ký tự đã loại bỏ dấu và lowercase - phục vụ check map tương đối nội dung của text</returns>
        /// CreatedBy: NVMANH (23/04/2020)
        public static string RemoveDiacritics(string text)
        {
            var newText = string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
            newText = newText.Replace("#", "sharp");
            newText = RemoveSpecialCharacters(newText.Trim().Replace(" ", "_").ToLower());
            newText = newText.Trim();
            return newText;
        }

        /// <summary>
        /// Loại bỏ các ký tự đặc biệt có trong tên tệp
        /// </summary>
        /// <param name="str">Tên tệp</param>
        /// <returns>Tên mới được loại bỏ các ký tự đặc biệt</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == '.') || (c == '_') || (c == '-'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
