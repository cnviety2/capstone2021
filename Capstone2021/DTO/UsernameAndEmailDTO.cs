using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    public class UsernameAndEmailDTO
    {
        [Required(ErrorMessage = "Username không được thiếu")]
        [StringLength(20, ErrorMessage = "Độ dài username lớn hơn 3 ký tự và không quá 20 ký tự", MinimumLength = 3)]
        public string username { get; set; }

        [Required(ErrorMessage = "Email không được thiếu")]
        [RegularExpression("^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+$", ErrorMessage = "Email không chính xác. VD: recruiter123@gmail.com")]
        public string gmail { get; set; }

        public bool isEmpty()
        {
            if (username.IsEmpty() || gmail.IsEmpty())
                return true;

            return false;
        }
    }
}