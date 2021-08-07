using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace Capstone2021.Services
{
    public class CompanyServiceImpl : CompanyService
    {
        private static Logger logger;
        private DbEntities context;

        public CompanyServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
            context = new DbEntities();
        }

        public void Dispose()
        {
            context.Dispose();
        }


        public bool create(Company obj)
        {
            throw new NotImplementedException();
        }

        public Company get(int id)
        {
            Company result = null;
            using (context)
            {
                result = context.companies.AsEnumerable().Where(s => s.recruiter_id == id).Select(s => CompanyMapper.mapFromDbModelToDto(s)).FirstOrDefault<Company>();
            }
            return result;
        }

        public IList<Company> getAll()
        {
            IList<Company> result = new List<Company>();
            using (context)
            {
                result = context.companies.AsEnumerable().Select(s => CompanyMapper.mapFromDbModelToDto(s)).ToList<Company>();
            }
            return result;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public int createNewCompanyInfo(CreateCompanyDTO dto, int recruiteId)
        {
            using (context)
            {
                var checkDuplicate = context.companies.Where(s => s.recruiter_id == recruiteId).FirstOrDefault<company>();
                if (checkDuplicate != null) return 2;//check recruiter này đã tạo thông tin về company chưa
                try
                {
                    company model = CompanyMapper.mapFromCreateDtoToDbModel(dto);
                    model.recruiter_id = recruiteId;
                    context.companies.Add(model);
                    context.SaveChanges();
                    return 1;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in CompanyServiceImpl");
                    return 3;
                }
            }
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public Company getSelfCompany(int recruiterId)
        {
            using (context)
            {
                var company = context.companies.Where(s => s.recruiter_id == recruiterId).FirstOrDefault<company>();
                if (company != null)
                {
                    Company result = new Company();
                    result.id = company.id;
                    result.name = company.name;
                    result.recruiterId = company.recruiter_id;
                    result.website = company.website;
                    result.headquaters = company.headquarters;
                    result.createDate = company.create_date.Value.ToString("dd/MM/yyyy");
                    result.description = company.description;
                    result.avatar = company.avatar;
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public int updateCompanyInfo(UpdateCompanyDTO dto, int recruiterId)
        {
            using (context)
            {
                var recruiter = context.recruiters.Find(recruiterId);
                if (recruiter == null)
                {
                    return 2;
                }
                else
                {
                    if (recruiter.companies.Count == 0)
                    {
                        return 3;
                    }
                    else
                    {
                        try
                        {
                            var company = context.companies.Where(s => s.recruiter_id == recruiterId).FirstOrDefault<company>();
                            if (dto.name != null && !dto.name.IsEmpty())
                            {
                                company.name = dto.name.Trim();
                            }
                            if (dto.description != null && !dto.description.IsEmpty())
                            {
                                company.description = dto.description.Trim();
                            }
                            if (dto.headquarters != null && !dto.headquarters.IsEmpty())
                            {
                                company.headquarters = dto.headquarters.Trim();
                            }
                            if (dto.website != null && !dto.website.IsEmpty())
                            {
                                company.website = dto.website.Trim();
                            }
                            context.SaveChanges();
                            return 1;
                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.Message + "in CompanyServiceImpl");
                            return 4;
                        }
                    }
                }
            }
        }

        public ReturnCompanyDTO getCompanyByRecruiterId(int recuiterId)
        {
            using (context)
            {
                var company = context.companies.Where(s => s.recruiter_id == recuiterId).FirstOrDefault<company>();
                if (company != null)
                {
                    ReturnCompanyDTO result = new ReturnCompanyDTO();
                    result.avatar = company.avatar;
                    result.createDate = company.create_date.Value.ToString("dd/MM/yyyy");
                    result.description = company.description;
                    result.headquarters = company.headquarters;
                    result.name = company.name;
                    result.recruiterId = company.recruiter_id;
                    result.website = company.website;
                    return result;
                }
            }
            return null;
        }

        public string updateImage(string imageUrl, int recruiterId)
        {
            string url = "";
            var checkRecruiter = context.recruiters.Find(recruiterId);
            if (checkRecruiter == null)
                return url;
            else
            {
                var company = context.companies.Where(s => s.recruiter_id == checkRecruiter.id).FirstOrDefault<company>();
                if (company == null)
                {
                    return url;
                }
                else
                {
                    using (context)
                    {
                        try
                        {
                            company.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                            context.SaveChanges();
                            url = company.avatar;
                            return url;

                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                            return url;
                        }
                    }

                }
            }
        }
    }
}