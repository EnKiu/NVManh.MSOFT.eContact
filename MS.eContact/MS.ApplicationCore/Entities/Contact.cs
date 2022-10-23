using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.MSEnums;
using MS.ApplicationCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Contact: BaseEntity
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Career { get; set; }
        public string MaritalStatusText;
        private int _maritalStatus;

        public int MaritalStatus
        {
            get { return _maritalStatus; }
            set
            {
                _maritalStatus = value;
               
                switch ((MaritalStatus)value)
                {
                    case MSEnums.MaritalStatus.Engaged:
                        MaritalStatusText = "Đã đính hôn";
                        break;
                    case MSEnums.MaritalStatus.Married:
                        MaritalStatusText = "Đã kết hôn";
                        break;
                    case MSEnums.MaritalStatus.Separated:
                        MaritalStatusText = "Đã ly hôn";
                        break;
                    case MSEnums.MaritalStatus.Divorced:
                        MaritalStatusText = "Ly thân";
                        break;
                    case MSEnums.MaritalStatus.Widow:
                        MaritalStatusText = "Góa chồng";
                        break;
                    case MSEnums.MaritalStatus.Widower:
                        MaritalStatusText = "Góa vợ";
                        break;
                    default:
                        MaritalStatusText = "Độc thân";
                        break;
                }
            }
        }

        public string Workplace { get; set; }
        public string Facebook { get; set; }
        public string Zalo { get; set; }
        public string OtherInfo { get; set; }
        public Int64 Sort { get; set; }
        public int RankStar { get; set; }
        public string AvatarLink { get; set; }
        public string? AvatarFullPath
        {
            get
            {
                var random = new Random();
                if (AvatarLink != null)
                    return String.Format("{0}/{1}?v={2}", CommonConst.ServerFileUrl, AvatarLink, random.Next(1, 999));
                return null;
            }
        }
    }
}
