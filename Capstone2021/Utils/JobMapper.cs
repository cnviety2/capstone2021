using Capstone2021.DTO;
using System;
using System.Web.WebPages;

namespace Capstone2021.Utils
{
    public class JobMapper
    {
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
            result.name = model.name;
            result.offer = model.offer;
            result.quantity = model.quantity;
            result.recruiterId = model.recruiter_id;
            result.requirement = model.requirement;
            result.salaryMax = model.salary_max;
            result.salaryMin = model.salary_min;
            result.sex = model.sex != null ? model.sex.Value : 3;//cả 2 giới tính đều được
            result.status = model.status;
            result.type = model.type;
            result.workingForm = model.working_form;
            result.workingPlace = model.working_place;
            result.createDate = model.create_date.ToString("dd/MM/yyyy");
            result.description = model.description;
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
            model.name = dto.name;
            model.working_form = dto.workingForm;
            model.location = dto.location;
            model.working_place = dto.workingPlace;
            model.description = dto.description;
            model.requirement = dto.requirement;
            model.type = dto.type;
            model.offer = dto.offer;
            model.sex = dto.sex;
            model.quantity = dto.quantity;
            model.salary_min = dto.salaryMin;
            model.salary_max = dto.salaryMax;
            model.recruiter_id = dto.recruiterId;
            model.create_date = DateTime.Now;
            model.status = 1;
            return model;
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
                model.name = dto.name;
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
                model.working_place = dto.workingPlace;
            }
            if (dto.description != null || !dto.description.IsEmpty())
            {
                model.description = dto.description;
            }
            if (dto.requirement != null || !dto.requirement.IsEmpty())
            {
                model.requirement = dto.requirement;
            }
            if (dto.type.HasValue && dto.type != model.type)
            {
                model.type = dto.type.Value;
            }
            if (dto.offer != null || !dto.offer.IsEmpty())
            {
                model.offer = dto.offer;
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

            return model;
        }
    }
}