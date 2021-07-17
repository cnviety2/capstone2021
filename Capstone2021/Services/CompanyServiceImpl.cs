using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                result = context.companies.AsEnumerable().Where(s => s.id == id).Select(s => CompanyMapper.mapFromDbModelToDto(s)).FirstOrDefault<Company>();
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

        public bool create(CreateCompanyDTO dto, int recruiteId)
        {
            bool result = false;
            using (context)
            {
                var checkDuplicate = context.companies.Where(s => s.recruiter_id == recruiteId).Select(s => new Company()
                {
                    id = s.id
                }).FirstOrDefault<Company>();
                if (checkDuplicate != null) return result;//check recruiter này đã tạo thông tin về company chưa
                try
                {
                    company model = CompanyMapper.mapFromCreateDtoToDbModel(dto);
                    model.recruiter_id = recruiteId;
                    context.companies.Add(model);
                    context.SaveChanges();
                    result = true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in CompanyServiceImpl");
                    return result;
                }
            }
            return result;
        }
    }
}