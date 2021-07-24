using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.Services.Student
{
    public class StudentServiceImpl : StudentService
    {
        private static Logger logger;
        private DbEntities context;

        public StudentServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
            context = new DbEntities();
        }
        public bool create(DTO.Student obj)
        {
            var std = context.students.Where(s => s.gmail.Equals(obj.gmail)).Select(s => new DTO.Student()
            {
                gmail = s.gmail,
                googleId = s.google_id
            }).FirstOrDefault<DTO.Student>();
            if (std != null)
            {
                if (std.googleId.Equals(obj.googleId)) return true;
                else return false;
            }
            else
            {
                try
                {
                    student saveObj = StudentMapper.mapToDbModel(obj);
                    context.students.Add(saveObj);
                    context.SaveChanges();
                    obj = StudentMapper.mapToDto(saveObj);
                    return true; ;
                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return false;
                }

            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public DTO.Student get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<DTO.Student> getAll()
        {
            throw new NotImplementedException();
        }

        public DTO.Student login(DTO.Student obj)
        {
            using (context)
            {
                DTO.Student std = context.students.AsEnumerable().Where(s => s.gmail.Equals(obj.gmail)).Select(s => new DTO.Student()
                {
                    id = s.id,
                    gmail = s.gmail,
                    isBanned = s.is_banned.Value,
                    avatar = s.avatar,
                    googleId = s.google_id,
                    profileStatus = s.profile_status
                }
                ).FirstOrDefault<DTO.Student>();
                if (std != null)
                {
                    if (std.googleId.Equals(obj.googleId)) return std;
                    else return null;
                }
            }
            return null;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }
    }
}