using Capstone2021.DTO;
using System;
using System.Collections.Generic;
using System.Web.WebPages;

namespace Capstone2021.Utils
{
    public class JobUtils
    {

        /// <summary>
        /// Lấy ra chuỗi đại diện cho job để thực hiện suggest tới student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static string getJobSuggestStringFromDTO(CreateJobDTO dto)
        {
            string result = "";
            if (dto.salaryMin > 6500000)//kiểm tra lớn hơn 6tr5-thu nhập bình quân 1 tháng của TPHCM
            {
                result = result + "1";
            }
            else result = result + "0";
            result = result + "-";
            for (int i = 0; i < dto.categories.Length; i++)
            {
                result = result + dto.categories[i].ToString();
                result = result + ";";
            }
            result = result.Remove(result.Length - 1, 1);
            result = result + "-";
            result = result + dto.location.ToString();
            return result;//chuỗi trả về có dạng 1-1;2;3;4-12 
        }

        /// <summary>
        /// Kiểm tra xem student này đã apply job này chưa,true nếu đã aplly rồi
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static bool hasThisStudentApplied(int studentId, job model)
        {

            if (model.student_apply_job == null || model.student_apply_job.Count == 0)
            {
                return false;
            }
            foreach (student_apply_job element in model.student_apply_job)
            {
                if (element.student_id == studentId)
                    return true;
            }
            return false;
        }

        public static string getJobSuggestStringFromDTO(job model)
        {
            string result = "";
            if (model.salary_min > 6500000)//kiểm tra lớn hơn 6tr5-thu nhập bình quân 1 tháng của TPHCM
            {
                result = result + "1";
            }
            else result = result + "0";
            result = result + "-";
            ICollection<job_has_category> listCategories = model.job_has_category;
            foreach (job_has_category relationship in listCategories)
            {
                result = result + relationship.category_id.ToString();
                result = result + ";";
            }
            result = result.Remove(result.Length - 1, 1);
            result = result + "-";
            result = result + model.location.ToString();
            return result;//chuỗi trả về có dạng 1-1;2;3;4-12 
        }

        /// <summary>
        /// Method để map từ model của Db sang dto Job,dùng trong service của job khi get 1 job từ Db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Job mapFromDbContext(job model)
        {
            Job result = new Job();
            result.id = model.id;
            result.location = model.location;
            result.managerId = model.manager_id != null ? model.manager_id.Value : -1;//chưa đc duyệt
            result.name = model.name.Trim();
            result.offer = model.offer.Trim();
            result.quantity = model.quantity;
            result.recruiterId = model.recruiter_id;
            result.requirement = model.requirement.Trim();
            result.salaryMax = model.salary_max;
            result.salaryMin = model.salary_min;
            result.sex = model.sex != null ? model.sex.Value : 3;//cả 2 giới tính đều được
            result.status = model.status;
            result.type = model.type;
            result.workingForm = model.working_form;
            result.workingPlace = model.working_place.Trim();
            result.createDate = model.create_date.ToString("dd/MM/yyyy");
            result.description = model.description.Trim();
            result.relationship = model.job_has_category;
            result.relationShipWithStudent = model.student_apply_job;
            result.stringForSuggestion = model.string_for_suggestion;
            result.createDate2 = model.create_date;
            result.activeDays = model.active_days;
            return result;
        }

        /// <summary>
        /// Method để map từ dto Job sang model của entity framework để lưu xuống databse
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static job mapToDatabaseModel(CreateJobDTO dto)
        {
            job model = new job();
            model.name = dto.name.Trim();
            model.working_form = dto.workingForm;
            model.location = dto.location;
            model.working_place = dto.workingPlace.Trim();
            model.description = dto.description.Trim();
            model.requirement = dto.requirement.Trim();
            model.type = dto.type;
            model.offer = dto.offer.Trim();
            model.sex = dto.sex;
            model.quantity = dto.quantity;
            model.salary_min = dto.salaryMin;
            model.salary_max = dto.salaryMax;
            model.recruiter_id = dto.recruiterId;
            model.create_date = DateTime.Now;
            //model.status = 1;
            model.status = 2;
            model.manager_id = 1006;
            model.active_days = dto.activeDays;
            return model;
        }

        public static AppliedJobDTO mapFromDbModelToAppliedDTO(job model)
        {
            AppliedJobDTO result = new AppliedJobDTO();
            result.id = model.id;
            result.name = model.name;
            return result;
        }

        /// <summary>
        /// Hàm map từ UpadteJobDTO sang model trong db để update,sẽ kiểm tra trong dto field nào khác null thì mới map sang model
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static job mapFromDtoToDbModelForUpdating(UpdateJobDTO dto, job model)
        {
            if (dto.name != null || !dto.name.IsEmpty())
            {
                model.name = dto.name.Trim();
            }
            if (dto.workingForm.HasValue && dto.workingForm != model.working_form && dto.workingForm != 0)
            {
                model.working_form = dto.workingForm.Value;
            }
            if (dto.location.HasValue && dto.location != model.location && dto.location != 0)
            {
                model.location = dto.location.Value;
            }
            if (dto.workingPlace != null || !dto.workingPlace.IsEmpty())
            {
                model.working_place = dto.workingPlace.Trim();
            }
            if (dto.description != null || !dto.description.IsEmpty())
            {
                model.description = dto.description.Trim();
            }
            if (dto.requirement != null || !dto.requirement.IsEmpty())
            {
                model.requirement = dto.requirement.Trim();
            }
            if (dto.type.HasValue && dto.type != model.type)
            {
                model.type = dto.type.Value;
            }
            if (dto.offer != null || !dto.offer.IsEmpty())
            {
                model.offer = dto.offer.Trim();
            }
            if (dto.sex.HasValue && dto.sex != model.sex && dto.sex != 0)
            {
                model.sex = dto.sex;
            }
            if (dto.quantity.HasValue && dto.quantity != model.quantity && dto.quantity != 0)
            {
                model.quantity = dto.quantity.Value;
            }
            if (dto.salaryMin.HasValue && dto.salaryMin != model.salary_min && dto.salaryMin != 0)
            {
                model.salary_min = dto.salaryMin.Value;
            }
            if (dto.salaryMax.HasValue && dto.salaryMax != model.salary_max && dto.salaryMax != 0)
            {
                model.salary_max = dto.salaryMax.Value;
            }
            if (dto.categories != null)
            {
                int[] categoryArray = dto.categories;
                for (int i = 0; i < categoryArray.Length; i++)
                {
                    job_has_category relationship = new job_has_category();
                    relationship.category_id = categoryArray[i];
                    relationship.job_id = dto.id;
                    model.job_has_category.Add(relationship);
                }
            }

            return model;
        }
    }
}