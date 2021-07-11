using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("recruiter")]
    [Authorize(Roles = "ROLE_RECRUITER")]//chỉ có những user có role là recruiter chứa trong HttpContext là được sử dụng những api này
    public class RecruiterController : ApiController
    {
        private readonly RecruiterService _recruiterService;
        public RecruiterController()
        {
            _recruiterService = new RecruiterServiceImpl();
        }
        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public IHttpActionResult create([FromBody] CreateRecruiterDTO recruiter)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }

            recruiter.password = Crypto.HashPassword(recruiter.password.Trim());
            bool saveState = _recruiterService.create(recruiter);
            if (saveState)
            {
                response.message = "OK";
            }
            else
            {
                response.message = "Error occured";
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult updateInformation([FromBody] UpdateInformationRecruiterDTO recruiter)
        {
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }

            ResponseDTO response = new ResponseDTO();
            bool saveState = _recruiterService.update(recruiter);
            if (saveState)
            {
                response.message = "OK";
            }
            else
            {
                response.message = "Error occured";
            }
            return Ok(response);
        }
        [Route("update/password")]
        [HttpPut]
        public IHttpActionResult updatePassword([FromBody] UpdatePasswordRecruiterDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            string currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            bool updateState = _recruiterService.updatePassword(dto.password, currentUser);
            if (updateState)
            {
                response.message = "OK";
            }
            else
            {
                return InternalServerError();
            }
            return Ok(response);
        }
    }
}