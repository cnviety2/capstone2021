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

        public bool remove(int id)
        {
            throw new NotImplementedException();
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