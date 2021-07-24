using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Collections.Generic;
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
        [Route("register")]
        [AllowAnonymous]
        public IHttpActionResult create([FromBody] CreateRecruiterDTO recruiter)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }

            recruiter.password = Crypto.HashPassword(recruiter.password.Trim());
            Recruiter model = RecruiterMapper.mapFromDto(recruiter);
            bool saveState = _recruiterService.create(model);
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
        [HttpGet]
        [Route("recruiter")]
        [Authorize]
        public IHttpActionResult getAllRecruiter()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Recruiter> list = _recruiterService.getAll();
            if (list.Count == 0)
            {
                response.message = "No data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }
        [HttpGet]
        [Route("recruiter/{id:int:min(0)}")]
        [Authorize]
        public IHttpActionResult getARecruiter([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Recruiter recruiter = _recruiterService.get(id);
            if (recruiter == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = recruiter;
            return Ok(response);
        }
    }
}