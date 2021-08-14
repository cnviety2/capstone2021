using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("forgot-password")]
    public class ForgotPasswordController : ApiController
    {
        private RecruiterService recruiterService;

        public ForgotPasswordController()
        {
            recruiterService = new RecruiterServiceImpl();
        }

        [HttpPut]
        [Route("update-password")]
        [AllowAnonymous]
        public IHttpActionResult updateForgottenPassword([FromBody] ForgotPasswordDTO dto)
        {
            if (dto == null || dto.isEmpty())
            {
                return BadRequest("Dữ liệu không được thiếu");
            }
            if (StringUtils.isContainSpecialCharacter(dto.username) == true)
            {
                return BadRequest("Username không được chứa ký tự đặc biệt");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string hashedPasswrod = Crypto.HashPassword(dto.password.Trim());
            int updateState = recruiterService.updateForgottenPassword(dto.code, dto.username, hashedPasswrod);
            switch (updateState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("User không tồn tại");
                case 3:
                    return BadRequest("Mã lấy lại sai");
                case 4:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        [HttpPut]
        [Route("verify")]
        [AllowAnonymous]
        public IHttpActionResult verifyToSendEmailToRetrievePassword([FromBody] UsernameAndEmailDTO dto)
        {
            if (dto == null || dto.isEmpty())
            {
                return BadRequest("Không dược empty");
            }
            if (!StringUtils.isValidEmailFormat(dto.gmail))
                return BadRequest("Email không chính xác. VD: recruiter123@gmail.com");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SendMailToRetrievePasswordDTO result = recruiterService.sendEmailToRetrievePassword(dto.username, dto.gmail);
            switch (result.code)
            {
                case 1:
                    ResponseDTO sendMailState = EmailUtils.sendEmail(result.randomString, dto.gmail);
                    /*if (sendMailState.message.Equals("OK"))
                    {
                        return Ok(sendMailState);
                    }
                    else
                    {
                        return Ok();
                    }*/
                    return Ok(sendMailState);
                case 2:
                    return BadRequest("Email và username không trùng");
                case 3:
                    return BadRequest("User không tồn tại");
                case 4:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

    }
}