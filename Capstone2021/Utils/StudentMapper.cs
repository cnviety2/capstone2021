using Capstone2021.DTO;
using System;

namespace Capstone2021.Utils
{
    public class StudentMapper
    {
        public static student mapToDbModel(Student dto)
        {
            student result = new student();
            result.avatar = dto.avatar;
            result.create_date = DateTime.Now;
            result.gmail = dto.gmail;
            result.google_id = dto.googleId;
            result.is_banned = false;
            result.profile_status = false;
            return result;
        }

        public static Student mapToDto(student model)
        {
            Student result = new Student();
            result.gmail = model.gmail;
            result.googleId = model.google_id;
            result.isBanned = model.is_banned.Value;
            result.profileStatus = model.profile_status;
            result.avatar = model.avatar;
            return result;
        }
    }
}