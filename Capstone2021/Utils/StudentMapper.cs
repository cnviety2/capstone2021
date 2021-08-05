using Capstone2021.DTO;
using System;

namespace Capstone2021.Utils
{
    public class StudentMapper
    {
        public static student mapToDbModel(Student dto)
        {
            student result = new student();
            result.avatar = dto.avatar.Trim();
            result.create_date = DateTime.Now;
            result.gmail = dto.gmail.Trim();
            result.google_id = dto.googleId.Trim();
            result.is_banned = false;
            result.profile_status = false;
            return result;
        }

        public static Student mapToDto(student model)
        {
            Student result = new Student();
            result.id = model.id;
            result.gmail = model.gmail.Trim();
            result.googleId = model.google_id.Trim();
            result.isBanned = model.is_banned.Value;
            result.profileStatus = model.profile_status;
            result.avatar = model.avatar.Trim();
            return result;
        }
    }
}