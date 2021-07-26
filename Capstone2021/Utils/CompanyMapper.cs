using Capstone2021.DTO;
using System;
using System.Web.WebPages;

namespace Capstone2021.Utils
{
    public class CompanyMapper
    {
        public static company mapFromCreateDtoToDbModel(CreateCompanyDTO dto)
        {
            company model = new company();
            model.name = dto.name.Trim();
            model.headquarters = dto.headquaters.Trim();
            if (!dto.website.IsEmpty()) model.website = dto.website.Trim();
            if (!dto.avatar.IsEmpty()) model.avatar = dto.avatar.Trim();
            if (!dto.description.IsEmpty()) model.description = dto.description.Trim();
            model.create_date = DateTime.Now;
            return model;
        }

        public static Company mapFromDbModelToDto(company model)
        {
            Company dto = new Company();
            dto.id = model.id;
            dto.name = model.name.Trim();
            dto.headquaters = model.headquarters.Trim();
            dto.avatar = model.avatar.Trim();
            dto.description = model.description.Trim();
            dto.createDate = model.create_date.Value.ToString("dd/MM/yyyy");
            dto.website = model.website.Trim();
            dto.recruiterId = model.recruiter_id;
            return dto;
        }
    }
}