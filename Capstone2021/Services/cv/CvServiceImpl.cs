using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.Services
{
    /**
     * Class này hiện thực interface CvService
     */
    public class CvServiceImpl : CvService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        public CvServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public bool create(Cv obj)
        {
            bool result = false;
            /*  int student_id = obj.studentId;
              cv saveObj = new cv();
              using (context)
              {
                  Cv checkCv = context.cvs.AsEnumerable().
                      Where(c => c.student_id.Equals(obj.studentId)).
                      Select(c => new Cv()
                      {
                          studentId = c.student_id,

                      }).FirstOrDefault<Cv>();
                  if (checkCv != null)
                  {
                      result = false;
                  }
                  else
                  {
                      try
                      {
                          saveObj = CvMapper.map(obj);
                          context.cvs.Add(saveObj);
                          context.SaveChanges();

                      }
                      catch (Exception e)
                      {
                          logger.Info("Exception " + e.Message + "in CvServiceImpl");
                          result = false;
                      }
                  }
              }*/
            return result;
        }

        public bool create(CreateCvDTO dto, int studentId)
        {
            using (context)
            {
                using (var contextTransaction = context.Database.BeginTransaction())
                {
                    var student = context.students.Find(studentId);
                    if (student != null)
                    {
                        try
                        {
                            cv model = CvMapper.mapToDatabaseModel(dto);
                            context.cvs.Add(model);
                            model.student_id = studentId;
                            context.SaveChanges();
                            student.profile_status = true;
                            context.SaveChanges();
                            contextTransaction.Commit();
                            return true;
                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.InnerException.Message + "in CvServiceImpl");
                            contextTransaction.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Cv get(int studentId, int cvId)
        {
            Cv result = null;
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student != null)
                {
                    IList<int> listStudentCvId = student.cvs.Select(s => s.id).ToList<int>();
                    if (listStudentCvId.Contains(cvId))
                    {
                        //context.cvs sẽ lấy ra DataSet của table cv ở phía dưới db
                        result = context.cvs.AsEnumerable()
                            .Where(c => c.id == cvId)
                            .Select(c => CvMapper.getFromDbContext(c))
                            .FirstOrDefault<Cv>();
                    }
                }
            }
            return result;
        }

        public Cv get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Cv> getAll()
        {
            throw new NotImplementedException();
        }

        public IList<ReturnListCvDTO> getListCvs(int studentId)
        {
            IList<ReturnListCvDTO> result = new List<ReturnListCvDTO>();
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student != null)
                {
                    result = student.cvs.Select(s => new ReturnListCvDTO()
                    {
                        createDate = s.create_date.Value.ToString("dd/MM/yyyy"),
                        cvName = s.cv_name,
                        id = s.id
                    }).ToList<ReturnListCvDTO>();
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        public int publicACv(int studentId, int cvId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student == null)
                {
                    return 2;
                }
                else
                {
                    IList<int> listCv = student.cvs.Select(s => s.id).ToList<int>();
                    if (!listCv.Contains(cvId))
                    {
                        return 2;
                    }
                    else
                    {
                        try
                        {
                            cv cv = context.cvs.Find(cvId);
                            if (cv.is_public == true)
                            {
                                return 3;
                            }
                            else
                            {
                                cv.is_public = true;
                                context.SaveChanges();
                                return 1;
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.Message + "in CvServiceImpl");
                            return 4;
                        }
                    }
                }
            }
        }

        public int unpublicACv(int studentId, int cvId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student == null)
                {
                    return 2;
                }
                else
                {
                    IList<int> listCv = student.cvs.Select(s => s.id).ToList<int>();
                    if (!listCv.Contains(cvId))
                    {
                        return 2;
                    }
                    else
                    {
                        try
                        {
                            cv cv = context.cvs.Find(cvId);
                            if (cv.is_public == false)
                            {
                                return 3;
                            }
                            else
                            {
                                cv.is_public = false;
                                context.SaveChanges();
                                return 1;
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.Message + "in CvServiceImpl");
                            return 4;
                        }
                    }
                }
            }
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public int removeACv(int studentId, int cvId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student == null)
                {
                    return 2;
                }
                else
                {
                    if (student.cvs.Count == 0)
                    {
                        return 2;
                    }
                    else
                    {
                        IList<int> listCvOfThisStudent = student.cvs.Select(s => s.id).ToList<int>();
                        if (!listCvOfThisStudent.Contains(cvId))
                        {
                            return 3;
                        }
                    }
                    try
                    {
                        using (var contextTransaction = context.Database.BeginTransaction())
                        {
                            var relationship = context.student_apply_job.Where(s => s.cv_id == cvId).FirstOrDefault();
                            if (relationship != null)
                            {
                                return 5;
                            }
                            var cv = context.cvs.Find(cvId);
                            context.cvs.Remove(cv);
                            context.SaveChanges();
                            contextTransaction.Commit();
                            return 1;
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in CvServiceImpl");
                        return 4;
                    }
                }
            }
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public int update(UpdateCvDTO dto, int studentId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student != null)
                {
                    IList<int> listStudentCvId = student.cvs.Select(s => s.id).ToList<int>();
                    if (listStudentCvId.Contains(dto.id))
                    {
                        var cv = context.cvs.Find(dto.id);
                        if (cv == null)
                        {
                            return 2;//ko tim thay
                        }
                        else
                        {
                            try
                            {
                                cv = CvMapper.mapFromDtoToDbModelForUpdating(dto, cv);
                                context.SaveChanges();
                                return 1;//OK
                            }
                            catch (Exception e)
                            {
                                logger.Info("Exception " + e.Message + "in CvServiceImpl");
                                return 3;
                            }
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 2;
                }
            }
        }

        public IList<Cv> searchCvs(SearchCvDTO dto, int page)
        {
            IList<Cv> result = new List<Cv>();
            using (context)
            {
                if (dto.salary.HasValue && !dto.workingForm.HasValue)
                {
                    result = context.cvs.AsEnumerable()
                        .Where(s => s.desired_salary_minimum <= dto.salary.Value && s.is_public == true)
                        .OrderByDescending(s => s.cv_name)
                        .Skip(5 * (page - 1))
                        .Take(5)
                        .Select(s => CvMapper.getFromDbContext(s)).ToList<Cv>();
                }
                else
                {
                    if (!dto.salary.HasValue && dto.workingForm.HasValue)
                    {
                        result = context.cvs.AsEnumerable()
                                    .Where(s => s.working_form == dto.workingForm.Value && s.is_public == true)
                                    .OrderByDescending(s => s.cv_name)
                                    .Skip(5 * (page - 1))
                                    .Take(5)
                                    .Select(s => CvMapper.getFromDbContext(s)).ToList<Cv>();
                    }
                    else
                    {
                        if (dto.salary.HasValue && dto.workingForm.HasValue)
                        {
                            result = context.cvs.AsEnumerable()
                                    .Where(s => s.desired_salary_minimum <= dto.salary.Value && s.working_form == dto.workingForm.Value && s.is_public == true)
                                    .OrderByDescending(s => s.cv_name)
                                    .Skip(5 * (page - 1))
                                    .Take(5)
                                    .Select(s => CvMapper.getFromDbContext(s)).ToList<Cv>();
                        }
                    }
                }
            }
            return result;
        }

        public int getTotalPagesInSearchCv(SearchCvDTO dto)
        {
            int result = 0;
            using (context)
            {
                int count = 0;
                if (dto.salary.HasValue && !dto.workingForm.HasValue)
                {
                    count = context.cvs.Where(s => s.desired_salary_minimum <= dto.salary.Value && s.is_public == true).Count();
                }
                else
                {
                    if (!dto.salary.HasValue && dto.workingForm.HasValue)
                    {
                        count = context.cvs.Where(s => s.working_form == dto.workingForm.Value && s.is_public == true).Count();
                    }
                    else
                    {
                        if (dto.salary.HasValue && dto.workingForm.HasValue)
                        {
                            count = context.cvs.Where(s => s.desired_salary_minimum <= dto.salary.Value && s.working_form == dto.workingForm.Value && s.is_public == true).Count();
                        }
                    }
                }
                result = (int)Math.Ceiling((double)count / 5);
            }
            return result;
        }

        public IList<Cv> getAllPublicCvs()
        {
            IList<Cv> result = new List<Cv>();
            using (context)
            {
                result = context.cvs.AsEnumerable().Where(s => s.is_public == true).Select(s => CvMapper.getFromDbContext(s)).ToList<Cv>();
            }
            return result;
        }

        public string updateImage(String imageUrl, int studentId, int cvId)
        {
            string url = "";
            var student = context.students.Find(studentId);
            if (student == null)
                return url;
            using (context)
            {
                if (student.cvs.Count == 0)
                {
                    return url;
                }
                else
                {
                    IList<int> listCvs = student.cvs.Select(s => s.id).ToList<int>();
                    if (!listCvs.Contains(cvId))
                    {
                        return url;
                    }
                    else
                    {
                        try
                        {
                            var cv = context.cvs.Find(cvId);
                            cv.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                            context.SaveChanges();
                            url = cv.avatar;
                            return url;

                        }
                        catch (Exception e)
                        {
                            logger.Info("Exception " + e.Message + "in CvServiceImpl");
                            return url;
                        }
                    }
                }

            }
        }
    }
}