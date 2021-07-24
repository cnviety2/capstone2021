using Capstone2021.DTO;
using Capstone2021.Repositories.CompanyRepository;

namespace Capstone2021.Services
{
    interface CompanyService : CompanyRepository
    {
        /// <summary>
        /// Method tạo mới 1 company,recruiter mới được xài
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        bool create(CreateCompanyDTO model, int recruiterId);
    }
}
