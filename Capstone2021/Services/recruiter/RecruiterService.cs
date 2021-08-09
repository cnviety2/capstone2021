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

        /// <summary>
        /// Trả về recruiter data dựa trên recruiterId
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        ReturnRecruiterDTO getById(int recruiterId);

        ///<summary>
        ///Method register account của recruiter,return 1: OK,2:đã có username này,3:lôĩ
        ///</summary>
        int register(CreateRecruiterDTO dto);

        /// <summary>
        /// Return về url của image nếu thành công
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string updateImage(string imageUrl, int id);

        /// <summary>
        /// Xóa 1 job ở phía recruiter,return 1 : ok , 2 : ko tìm thấy recruiter, 3 : job ko tồn tại , 4 : lỗi
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        int removeAJob(int recruiterId, int jobId);
    }
}