using Capstone2021.DTO;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.WebPages;

namespace Capstone2021.Service
{
    /**
     * Class này hiện thực interface ManagerService
     */
    public class ManagerServiceImpl : ManagerService, IDisposable
    {
        private static Logger logger;
        private DbEntities context;

        public ManagerServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public void Dispose()
        {
            context.Dispose();
        }
        public bool create(Manager obj)
        {
            String username = obj.username;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(obj.username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username
                }).FirstOrDefault<Manager>();
                if (checkManager != null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        manager saveObj = new manager();
                        saveObj.create_date = DateTime.Now;
                        saveObj.username = obj.username;
                        saveObj.password = obj.password;
                        saveObj.role = obj.role;
                        saveObj.full_name = obj.fullName;
                        saveObj.is_banned = false;
                        context.managers.Add(saveObj);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                        return false;
                    }
                }
            }
            return true;
        }


        public int createAStaff(Manager obj)
        {
            String username = obj.username;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(obj.username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username
                }).FirstOrDefault<Manager>();
                if (checkManager != null)
                {
                    return 2;
                }
                else
                {
                    try
                    {
                        manager saveObj = new manager();
                        saveObj.create_date = DateTime.Now;
                        saveObj.username = obj.username;
                        saveObj.password = obj.password;
                        saveObj.role = obj.role;
                        saveObj.full_name = obj.fullName;
                        saveObj.is_banned = false;
                        context.managers.Add(saveObj);
                        context.SaveChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                        return 3;
                    }
                }
            }
        }

        public Manager get(int id)
        {
            Manager result = null;
            using (context)
            {
                //context.managers sẽ lấy ra DataSet của table manager ở phía dưới db
                result = context.managers.AsEnumerable().Where(s => s.id == id).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = "***",
                    role = s.role,
                    createDate = s.create_date != null ? s.create_date.Value.ToString("dd/MM/yyyy") : "null",
                    fullName = s.full_name
                }).FirstOrDefault<Manager>();
            }
            return result;
        }

        public IList<Manager> getAll()
        {
            IList<Manager> listResult = new List<Manager>();
            using (context)
            {
                listResult = context.managers.AsEnumerable().Where(s => s.role.Equals("ROLE_STAFF")).Select(s =>
                    new Manager()
                    {
                        id = s.id,
                        createDate = s.create_date != null ? s.create_date.Value.ToString("dd/MM/yyyy") : "null",
                        fullName = s.full_name,
                        username = s.username,
                        role = s.role,
                        password = "***",
                        isBanned = s.is_banned.Value
                    }
                ).ToList<Manager>();
            }
            return listResult;
        }

        public Manager login(string username, string password)
        {
            Manager result = null;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = s.password,
                    role = s.role,
                    isBanned = s.is_banned.HasValue ? s.is_banned.Value : false
                }).FirstOrDefault<Manager>();
                if (checkManager == null)
                {
                    return null;
                }
                if (Crypto.VerifyHashedPassword(checkManager.password, password))
                {
                    result = checkManager;
                }
            }
            return result;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public int updatePassword(string password, string username, string oldPassword)
        {
            using (context)
            {
                var checkManager = context.managers.SingleOrDefault(b => b.username.Equals(username));
                if (checkManager == null)
                {
                    return 3;
                }
                if (checkManager.is_banned == true)
                {
                    return 4;
                }
                else
                {

                    if (!Crypto.VerifyHashedPassword(checkManager.password, oldPassword))
                    {
                        return 2;
                    }
                    try
                    {
                        checkManager.password = Crypto.HashPassword(password);
                        context.SaveChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                        return 3;
                    }
                }
            }
        }

        public bool updateFullName(string fullName, string username)
        {
            bool result = false;
            using (context)
            {
                var checkManager = context.managers.SingleOrDefault(b => b.username.Equals(username));
                if (checkManager == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        checkManager.full_name = fullName;
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                    }
                }

            }
            return result;
        }

        public bool banAStaff(int id)
        {
            using (context)
            {
                var checkManager = context.managers.Find(id);
                if (checkManager == null)
                    return false;
                if (checkManager.role.Equals("ROLE_ADMIN"))
                    return false;
                if (checkManager.is_banned == true)
                    return true;
                try
                {
                    checkManager.is_banned = true;
                    context.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                }
            }
            return false;
        }


        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        //check
        public ReportByMonthDTO reportByMonth(int month)
        {
            ReportByMonthDTO report = new ReportByMonthDTO();
            using (context)
            {
                int numberOfRecruiters = context.recruiters.Where(s => s.create_date.Value.Month == month && s.create_date.Value.Year == DateTime.Now.Year).ToList().Count;
                var jobs = context.jobs.Where(s => s.create_date.Month == month && s.create_date.Year == DateTime.Now.Year);
                int numberOfJobs = jobs.Count();
                IList<int> listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                int numberOfDesiredStudents = 0;
                foreach (int quantity in listQuantity)
                {
                    numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                }
                int numberOfStudents = context.students.Where(s => s.create_date.Month == month && s.create_date.Year == DateTime.Now.Year).ToList().Count;
                report.numberOfDesiredStudents = numberOfDesiredStudents;
                report.numberOfJobs = numberOfJobs;
                report.numberOfRecruiters = numberOfRecruiters;
                report.numberOfStudents = numberOfStudents;
                report.month = month + "/" + DateTime.Now.Year.ToString();
            }
            return report;
        }

        //check
        public ReportByYearDTO reportByYear()
        {
            ReportByYearDTO report = new ReportByYearDTO();
            using (context)
            {
                int numberOfRecruiters = context.recruiters.Where(s => s.create_date.Value.Year == DateTime.Now.Year).ToList().Count;
                var jobs = context.jobs.Where(s => s.create_date.Year == DateTime.Now.Year);
                int numberOfJobs = jobs.Count();
                IList<int> listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                int numberOfDesiredStudents = 0;
                foreach (int quantity in listQuantity)
                {
                    numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                }
                int numberOfStudents = context.students.Where(s => s.create_date.Year == DateTime.Now.Year).ToList().Count;
                report.numberOfDesiredStudents = numberOfDesiredStudents;
                report.numberOfJobs = numberOfJobs;
                report.numberOfRecruiters = numberOfRecruiters;
                report.year = DateTime.Now.Year;
            }
            return report;
        }


        public ReportyByQuarterDTO reportByQuarter(int quarter)
        {
            int numberOfRecruiters = 0;
            int numberOfJobs = 0;
            int numberOfDesiredStudents = 0;
            IList<int> listQuantity = null;
            ReportyByQuarterDTO report = new ReportyByQuarterDTO();
            using (context)
            {
                switch (quarter)
                {
                    case 1:
                        numberOfRecruiters = context.recruiters
                            .AsEnumerable()
                            .Where(s => s.create_date.Value.Month >= 1 && s.create_date.Value.Month <= 3 && s.create_date.Value.Year == DateTime.Now.Year)
                            .Count();
                        var jobs = context.jobs.Where(s => s.create_date.Month >= 1 && s.create_date.Month <= 3 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 2:
                        numberOfRecruiters = context.recruiters
                            .AsEnumerable()
                            .Where(s => s.create_date.Value.Month >= 4 && s.create_date.Value.Month <= 6 && s.create_date.Value.Year == DateTime.Now.Year)
                            .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 4 && s.create_date.Month <= 6 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 3:
                        numberOfRecruiters = context.recruiters
                           .AsEnumerable()
                           .Where(s => s.create_date.Value.Month >= 7 && s.create_date.Value.Month <= 9 && s.create_date.Value.Year == DateTime.Now.Year)
                           .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 7 && s.create_date.Month <= 9 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 4:
                        numberOfRecruiters = context.recruiters
                           .AsEnumerable()
                           .Where(s => s.create_date.Value.Month >= 10 && s.create_date.Value.Month <= 12 && s.create_date.Value.Year == DateTime.Now.Year)
                           .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 10 && s.create_date.Month <= 12 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                }
            }
            report.numberOfDesiredStudents = numberOfDesiredStudents;
            report.numberOfJobs = numberOfJobs;
            report.numberOfRecruiters = numberOfRecruiters;
            report.year = DateTime.Now.Year;
            report.quarter = quarter;
            return report;
        }

        public ComboReportDTO generateReport(RequestForReportDTO dto)
        {
            ReportByMonthDTO byMonth = new ReportByMonthDTO();
            ReportyByQuarterDTO byQuarter = new ReportyByQuarterDTO();
            ReportByYearDTO byYear = new ReportByYearDTO();
            ComboReportDTO result = new ComboReportDTO();
            using (context)
            {
                int numberOfRecruiters = 0;
                int numberOfJobs = 0;
                int numberOfDesiredStudents = 0;
                IList<int> listQuantity = null;
                //quarter
                switch (dto.quarter)
                {
                    case 1:
                        numberOfRecruiters = context.recruiters
                            .AsEnumerable()
                            .Where(s => s.create_date.Value.Month >= 1 && s.create_date.Value.Month <= 3 && s.create_date.Value.Year == DateTime.Now.Year)
                            .Count();
                        var jobs = context.jobs.Where(s => s.create_date.Month >= 1 && s.create_date.Month <= 3 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 2:
                        numberOfRecruiters = context.recruiters
                            .AsEnumerable()
                            .Where(s => s.create_date.Value.Month >= 4 && s.create_date.Value.Month <= 6 && s.create_date.Value.Year == DateTime.Now.Year)
                            .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 4 && s.create_date.Month <= 6 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 3:
                        numberOfRecruiters = context.recruiters
                           .AsEnumerable()
                           .Where(s => s.create_date.Value.Month >= 7 && s.create_date.Value.Month <= 9 && s.create_date.Value.Year == DateTime.Now.Year)
                           .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 7 && s.create_date.Month <= 9 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                    case 4:
                        numberOfRecruiters = context.recruiters
                           .AsEnumerable()
                           .Where(s => s.create_date.Value.Month >= 10 && s.create_date.Value.Month <= 12 && s.create_date.Value.Year == DateTime.Now.Year)
                           .Count();
                        jobs = context.jobs.Where(s => s.create_date.Month >= 10 && s.create_date.Month <= 12 && s.create_date.Year == DateTime.Now.Year);
                        numberOfJobs = jobs.Count();
                        listQuantity = jobs.Select(s => s.quantity).ToList<int>();
                        foreach (int quantity in listQuantity)
                        {
                            numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                        }
                        break;
                }
                byQuarter.numberOfDesiredStudents = numberOfDesiredStudents;
                byQuarter.numberOfJobs = numberOfJobs;
                byQuarter.numberOfRecruiters = numberOfRecruiters;
                byQuarter.year = DateTime.Now.Year;
                byQuarter.quarter = dto.quarter;

                //year
                numberOfRecruiters = context.recruiters.Where(s => s.create_date.Value.Year == DateTime.Now.Year).ToList().Count;
                var jobs2 = context.jobs.Where(s => s.create_date.Year == DateTime.Now.Year);
                numberOfJobs = jobs2.Count();
                listQuantity = jobs2.Select(s => s.quantity).ToList<int>();
                numberOfDesiredStudents = 0;
                foreach (int quantity in listQuantity)
                {
                    numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                }
                int numberOfStudents = context.students.Where(s => s.create_date.Year == DateTime.Now.Year).ToList().Count;
                byYear.numberOfDesiredStudents = numberOfDesiredStudents;
                byYear.numberOfJobs = numberOfJobs;
                byYear.numberOfRecruiters = numberOfRecruiters;
                byYear.year = DateTime.Now.Year;

                //month
                numberOfRecruiters = context.recruiters.Where(s => s.create_date.Value.Month == dto.month && s.create_date.Value.Year == DateTime.Now.Year).ToList().Count;
                var jobs3 = context.jobs.Where(s => s.create_date.Month == dto.month && s.create_date.Year == DateTime.Now.Year);
                numberOfJobs = jobs3.Count();
                listQuantity = jobs3.Select(s => s.quantity).ToList<int>();
                numberOfDesiredStudents = 0;
                foreach (int quantity in listQuantity)
                {
                    numberOfDesiredStudents = numberOfDesiredStudents + quantity;
                }
                numberOfStudents = context.students.Where(s => s.create_date.Month == dto.month && s.create_date.Year == DateTime.Now.Year).ToList().Count;
                byMonth.numberOfDesiredStudents = numberOfDesiredStudents;
                byMonth.numberOfJobs = numberOfJobs;
                byMonth.numberOfRecruiters = numberOfRecruiters;
                byMonth.numberOfStudents = numberOfStudents;
                byMonth.month = dto.month + "/" + DateTime.Now.Year.ToString();
                //done
            }
            result.reportByMonth = byMonth;
            result.reportbyQuarter = byQuarter;
            result.reportByYear = byYear;
            return result;
        }

        public int updateBanner(UpdateBannerDTO dto, int staffId)
        {
            using (context)
            {
                var staff = context.managers.Find(staffId);
                if (staff == null || staff.is_banned == true)
                {
                    return 2;
                }
                else
                {
                    try
                    {
                        var banner = context.banners.Find(dto.id);
                        if (banner == null)
                        {
                            return 4;
                        }
                        else
                        {
                            if (dto.url != null && !dto.url.IsEmpty())
                            {
                                banner.url = dto.url;
                            }
                            context.SaveChanges();
                            return 1;
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                        return 3;
                    }
                }
            }
        }

        public string updateBannerImgUrl(int id, string imgUrl, int staffId)
        {
            string result = "";
            using (context)
            {
                var staff = context.managers.Find(staffId);
                if (staff == null || staff.is_banned == true)
                {
                    result = "error";
                    return result;
                }
                try
                {
                    var banner = context.banners.Find(id);
                    result = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imgUrl;
                    banner.image_url = result;
                    context.SaveChanges();
                    return result;
                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return "error";
                }
            }
        }

        public IList<Banner> getAllBanners()
        {
            IList<Banner> result = new List<Banner>();
            using (context)
            {
                result = context.banners.Select(s => new Banner() { id = s.id, url = s.url, imgUrl = s.image_url }).ToList<Banner>();
            }
            return result;
        }

        public bool unbanAStaff(int id)
        {
            using (context)
            {
                var checkManager = context.managers.Find(id);
                if (checkManager == null)
                    return false;
                if (checkManager.role.Equals("ROLE_ADMIN"))
                    return false;
                if (checkManager.is_banned == false)
                    return true;
                try
                {
                    checkManager.is_banned = false;
                    context.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                    return false;
                }
            }
        }
    }
}