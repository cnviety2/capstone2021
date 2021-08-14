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
        /// 5 : nếu có student đã apply thì ko cho xóa
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        int removeAJob(int recruiterId, int jobId);

        /// <summary>
        /// Kiểm tra xem username và email có trùng hay không,nếu đúng sẽ thực hiện send email tới và cập
        /// nhật forgot_password_string trong recruiter và return 1,nếu không trùng sẽ return 2,user không tồn tại là 3
        /// lỗi là 4
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        SendMailToRetrievePasswordDTO sendEmailToRetrievePassword(string username, string gmail);

        /// <summary>
        /// Tìm ra user,so sánh code bên trong recruiter để cập nhật password mới,retrun 1 nếu thành công,
        /// 2 nếu user ko tồn tại,3 nếu ko khớp code, 4 nếu lỗi
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        int updateForgottenPassword(string code, string username, string newPassword);
    }
}