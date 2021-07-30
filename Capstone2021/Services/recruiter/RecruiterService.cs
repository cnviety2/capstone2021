using Capstone2021.DTO;
using Capstone2021.Repositories.RecruiterRepository;

namespace Capstone2021.Services
{
    interface RecruiterService : IRecruiterRepository
    {
        ///<summary>
        ///Method login để check login,trả về null nếu không tồn tại user
        ///</summary>
        Recruiter login(string username, string password);
        ///<summary>
        ///Method update information của recruiter,return true nếu thành công
        ///</summary>
        bool update(UpdateInformationRecruiterDTO obj, string username);
        ///<summary>
        ///Method update password của recruiter,return true nếu thành công
        ///</summary>
        bool updatePassword(string password, string username);
        ///<summary>
        ///Method create account của recruiter,return true nếu thành công
        ///</summary>
        bool create(CreateRecruiterDTO obj);
        bool updateImage(string imageUrl, int id);
    }
}