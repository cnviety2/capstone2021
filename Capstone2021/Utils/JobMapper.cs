using Capstone2021.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Utils
{
    public class JobMapper
    {
        /// <summary>
        /// Method để map từ model của Db sang dto Job,dùng trong service của job khi get 1 job từ Db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Job getFromDbContext(job model)
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
            return result;
        }
    }
}