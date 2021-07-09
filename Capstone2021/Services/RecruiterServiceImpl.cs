using Capstone2021.DTO;
using Capstone2021.Interfaces;
using Capstone2021.Repositories.RecruiterRepository;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace Capstone2021.Services
{
    public class RecruiterServiceImpl : RecruiterService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        private readonly IRecruiterRepository _recruiterrepository;
        public RecruiterServiceImpl(IUnitOfWork unitOfWork)
        {
            context = new DbEntities();
            _unitOfWork = unitOfWork;
        }

        public bool create(Recruiter obj)
        {
            recruiter saveObj = new recruiter();
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable().Where(c => c.username.Equals(obj.username)).Select(c => new Recruiter()
                {
                    id = c.id,
                    username = c.username
                }).FirstOrDefault<Recruiter>();
                if (checkRecruiter != null)
                {
                    return false;
                }
                else
                {
                    try
                    {
<<<<<<< HEAD
                        recruiter saveObj = new recruiter();
=======
>>>>>>> c5d3466a62a8b1f7ce5929072bb874f2c89d1662
                        saveObj = RecruiterMapper.map(obj);
                        context.recruiters.Add(saveObj);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in RecruiterServiceImpl");
                        return false;
                    }
                }
            }
            return true;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Recruiter get(int id)
        {
            Recruiter result = null;
            using (context)
            {
                //context.recruiters sẽ lấy ra DataSet của table recruiters ở phía dưới db
                result = context.recruiters.AsEnumerable().Where(c => c.id == id).Select(c => new Recruiter()
                {
                    avatar = c.avatar,
                    companyName = c.company_name,
                    createDate = (DateTime)c.create_date,
                    description = c.description,
                    gmail = c.gmail,
                    headquarter = c.headquarters,
                    id = c.id,
                    password = c.password,
                    phone = c.phone,
                    role = c.role,
                    username = c.username,
                    website = c.website
                }).FirstOrDefault<Recruiter>();
            }
            return result;
        }

        public IList<Recruiter> getAll()
        {
            IList<Recruiter> listResult = new List<Recruiter>();
            using (context)
            {
                listResult = context.recruiters.AsEnumerable().Select(c => new Recruiter()
                {
                    avatar = c.avatar,
                    companyName = c.company_name,
                    createDate = (DateTime)c.create_date,
                    description = c.description,
                    gmail = c.gmail,
                    headquarter = c.headquarters,
                    id = c.id,
                    password = c.password,
                    phone = c.phone,
                    role = c.role,
                    username = c.username,
                    website = c.website
                }).ToList<Recruiter>();
                return listResult;
            }
        }
        public Recruiter login(string username, string password)
        {
            Recruiter result = null;
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable().Where(s => s.username.Equals(username)).Select(s => new Recruiter()
                {
                    id = s.id,
                    username = s.username,
                    role = s.role
                }).FirstOrDefault<Recruiter>();
                if (checkRecruiter == null)
                {
                    return null;
                }
                if (Crypto.VerifyHashedPassword(checkRecruiter.password, password))
                {
                    result = checkRecruiter;
                }
            }
            return result;
        }

        public bool remove(int id)
        {
            var recruiter = context.recruiters.Where(c => c.id == id).FirstOrDefault();
            if (recruiter == null)
            {
                return false;
            }
            else
            {
                context.recruiters.Remove(recruiter);
            }
            return true;
        }

        public bool update(Recruiter obj)
        {
            Recruiter recruiter = _unitOfWork.RecruiterRepository.GetByID(obj.id);
            if(recruiter == null)
            {
                return false;
            }
            recruiter.companyName = obj.companyName;
            recruiter.gmail = obj.gmail;
            recruiter.headquarter = obj.headquarter;
            recruiter.phone = obj.phone;
            recruiter.website = obj.website;
            recruiter.description = obj.description;
            _unitOfWork.RecruiterRepository.Update(recruiter);
            return true;
        }


    }
}