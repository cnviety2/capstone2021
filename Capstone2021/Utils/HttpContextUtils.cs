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
    }
}