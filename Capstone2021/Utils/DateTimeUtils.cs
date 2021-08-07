using System;
using System.Globalization;

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

        /// <summary>
        /// Kiểm tra có trên 18 tuổi chưa,true nếu trên 18
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool is18Plus(string value)
        {
            DateTime bday = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime today = DateTime.Today;
            int age = today.Year - bday.Year;
            if (bday > today.AddYears(-age))
            {
                age--;
            }
            if (age < 18)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Parse sang DateTime,value theo format yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime parse(string value)
        {
            return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}