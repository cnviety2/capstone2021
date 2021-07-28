using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Capstone2021.Utils
{
    /// <summary>
    /// Class hỗ trợ việc đọc thông tin từ security context của oauth để lấy thông tin user gửi request
    /// </summary>
    public class HttpContextUtils
    {
        /// <summary>
        /// Trả về username hiện tại đã lưu vào trong token của security context
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string getUsername(IIdentity identity)
        {
            return identity.Name;
        }


        /// <summary>
        /// Trả về id của user hiện tại đã lưu vào token 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int getUserID(ClaimsPrincipal identity)
        {
            return Int32.Parse(identity.Claims.Where(c => c.Type.Equals("id")).Single().Value);
        }

        /// <summary>
        /// Trả về google_id của user hiện tại đã lưu vào token(role student) 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string getGoogleID(ClaimsPrincipal identity)
        {
            return identity.Claims.Where(c => c.Type.Equals("google_id")).Single().Value;
        }
    }
}