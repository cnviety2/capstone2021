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
            model.name = dto.name;
            model.headquarters = dto.headquaters;
            if (!dto.website.IsEmpty()) model.website = dto.website;
            if (!dto.avatar.IsEmpty()) model.avatar = dto.avatar;
            if (!dto.description.IsEmpty()) model.description = dto.description;
            model.create_date = DateTime.Now;
            return model;
        }

        public static Company mapFromDbModelToDto(company model)
        {
            Company dto = new Company();
            dto.id = model.id;
            dto.name = model.name;
            dto.headquaters = model.headquarters;
            dto.avatar = model.avatar;
            dto.description = model.description;
            dto.createDate = model.create_date.Value.ToString("dd/MM/yyyy");
            dto.website = model.website;
            dto.recruiterId = model.recruiter_id;
            return dto;
        }
    }
}