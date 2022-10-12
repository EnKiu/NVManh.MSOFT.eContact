using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Enums
{
    /// <summary>
    /// Enum giới tính
    /// </summary>
    /// Created by: NVMANH (12/03/2017)
    public enum Gender
    {
        Male = 0,
        Female = 1,
        Other = 2
    }

    /// <summary>
    /// Enum tình trạng hôn nhân
    /// </summary>
    public enum MaritalStatus : int
    {
        //Độc thân
        Singler = 0,
        //Đã đính hôn
        Engaged = 1,
        //Đã kết hôn
        Married = 2,
        //Đã ly hôn
        Separated = 3,
        //Đã ly thân
        Divorced = 4,
        //Góa chồng
        Widow = 5,
        //Góa vợ
        Widower = 6
    }

    /// <summary>
    /// Trạng thái của đối tượng
    /// </summary>
    public enum EntityState
    {
        ADD = 1,
        UPDATE = 2,
        DELETE = 3,
    }
    public enum Role
    {
        Administrator = 1,
        Management = 5,
        Employee = 10,
        Teacher = 15,
        Advisor = 20,
        HRIntern = 21,
        Fresher = 25,
        Intern = 30,
        Newbie = 35
    }
}
