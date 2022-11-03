using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.MSEnums
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
    public enum MSRole
    {
        Administrator = 1,
        Mod = 10,
        Member = 100,
        NewUser = 999
    }



    /// <summary>
    /// Loại kế hoạch thu/chi
    /// </summary>
    public enum ExpenditurePlanType
    {
        /// <summary>
        /// Kế hoạch thu cho sự kiện
        /// </summary>
        INCREMENT_EVENT = 100,

        /// <summary>
        /// Kế hoạch thu quỹ hàng năm
        /// </summary>
        INCREMENT_ANNUAL = 101,

        /// <summary>
        /// Thu Khác
        /// </summary>
        INCREMENT_OTHER = 102,

        /// <summary>
        /// Kế hoạch chi cho sự kiện
        /// </summary>
        REDURE_EVENT = 200,

        /// <summary>
        /// Chi khác
        /// </summary>
        REDUCE_OTHER = 201

    }


    /// <summary>
    /// Loại khoản thu/ chi
    /// </summary>
    public enum ExpenditureType
    {
        /// <summary>
        /// Thu theo kế hoạch
        /// </summary>
        INCREMENT_PLAN = 1,

        /// <summary>
        /// Thu từ nguồn ủng hộ của các mạnh thường quân
        /// </summary>
        INCREMENT_SUPER_RICH = 2,

        /// <summary>
        /// Mạnh thường quân
        /// </summary>
        INCREMENT_OTHER = 3,

        /// <summary>
        /// Chi theo kế hoạch
        /// </summary>
        REDURE_PLAN = 20,

        /// <summary>
        /// Chi cho đám cưới
        /// </summary>
        REDURE_WEDDING = 21,

        /// <summary>
        /// Chi cho đám hiếu
        /// </summary>
        REDURE_FUNERAL = 22,

        /// <summary>
        /// Chi ốm đau, nghỉ bệnh
        /// </summary>
        REDUCE_MEDICAL = 23,

        /// <summary>
        /// Chi khác
        /// </summary>
        REDUCE_OTHER = 24

    }
}
