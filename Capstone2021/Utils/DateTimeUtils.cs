using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Utils
{
    public class DateTimeUtils
    {
        /// <summary>
        /// Kiểm tra createDate cho tới bây giờ đã hơn 30 ngày chưa,true nếu đúng
        /// </summary>
        /// <param name="createDate"></param>
        /// <returns></returns>
        public static bool isOver30Days(DateTime createDate)
        {
            if (createDate.AddDays(30).CompareTo(DateTime.Now) < 0) return true;
            else return false;
        }
    }
}