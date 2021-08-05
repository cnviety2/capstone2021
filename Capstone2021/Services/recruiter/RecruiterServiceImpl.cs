﻿using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace Capstone2021.Services
{
    /**
     * Class này hiện thực interface RecruiterService
     */
    public class RecruiterServiceImpl : RecruiterService, IDisposable
    {
        //private readonly IUnitOfWork _unitOfWork;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        //private readonly IRecruiterRepository _recruiterrepository;
        public RecruiterServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
            //_unitOfWork = unitOfWork;
        }

        public bool create(Recruiter obj)
        {
            recruiter saveObj = new recruiter();
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable()
                    .Where(c => c.username.Equals(obj.username))
                    .Select(c => new Recruiter()
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

        public bool create(CreateRecruiterDTO obj)
        {
            throw new NotImplementedException();
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
                result = context.recruiters.AsEnumerable()
                    .Where(c => c.id == id)
                    .Select(c => new Recruiter()
                    {
                        avatar = c.avatar,
                        createDate = (DateTime)c.create_date,
                        gmail = c.gmail,
                        id = c.id,
                        phone = c.phone,
                        role = c.role,
                        username = c.username,
                        firstName = c.first_name,
                        lastName = c.last_name
                    }).FirstOrDefault<Recruiter>();
            }
            return result;
        }

        public IList<Recruiter> getAll()
        {
            IList<Recruiter> listResult = new List<Recruiter>();
            using (context)
            {
                listResult = context.recruiters.AsEnumerable()
                    .Select(c => new Recruiter()
                    {
                        avatar = c.avatar,
                        createDate = (DateTime)c.create_date,
                        gmail = c.gmail,
                        id = c.id,
                        password = c.password,
                        phone = c.phone,
                        role = c.role,
                        username = c.username,
                        firstName = c.first_name,
                        lastName = c.last_name,
                        sex = c.sex
                    }).ToList<Recruiter>();
                return listResult;
            }
        }
        public Recruiter login(string username, string password)
        {
            Recruiter result = null;
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable()
                    .Where(s => s.username.Equals(username))
                    .Select(s => new Recruiter()
                    {
                        id = s.id,
                        username = s.username,
                        password = s.password,
                        role = s.role,
                        isBanned = s.is_banned.HasValue ? s.is_banned.Value : false
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
            /*  var recruiter = context.recruiters.Where(c => c.id == id).FirstOrDefault();
              if (recruiter == null)
              {
                  return false;
              }
              else
              {
                  context.recruiters.Remove(recruiter);
              }*/

            return true;
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public bool update(UpdateInformationRecruiterDTO obj, string username)
        {
            bool result = false;
            using (context)
            {
                var recruiter = context.recruiters
                    .SingleOrDefault(c => c.username.Equals(username));
                if (recruiter == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        recruiter.first_name = obj.firstName;
                        recruiter.last_name = obj.lastName;
                        recruiter.gmail = obj.gmail;
                        recruiter.phone = obj.phone;
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in RecruiterServiceImpl");
                        result = false;
                    }

                }
            }
            return result;
        }

        public bool updateImage(string imageUrl, int id)
        {
            var checkRecruiter = context.students.Find(id);
            if (checkRecruiter == null)
            {
                return false;
            }
            using (context)
            {
                try
                {
                    checkRecruiter.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return false;
                }
            }
        }

        public bool updatePassword(string password, string username)
        {
            bool result = false;
            using (context)
            {
                var checkRecruiter = context.recruiters.SingleOrDefault(c => c.username.Equals(username));
                if (checkRecruiter == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        checkRecruiter.password = Crypto.HashPassword(password);
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in RecruiterServiceImpl");
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}