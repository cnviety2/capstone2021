using Capstone2021.DTO;

namespace Capstone2021.Utils
{
    public class RecruiterMapper
    {
        ///<summary>Map từ Recruiter sang recruiter để lưu xuống db,ko có phần jobs</summary>
        public static recruiter map(Recruiter recruiter)
        {
            recruiter result = new recruiter();
            result.avatar = recruiter.avatar;
            result.create_date = recruiter.createDate;
            result.gmail = recruiter.gmail;
            result.id = recruiter.id;
            result.password = recruiter.password;
            result.phone = recruiter.phone;
            result.role = recruiter.role;
            result.username = recruiter.username;

            return result;
        }
    }
}