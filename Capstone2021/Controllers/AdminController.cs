using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "ROLE_ADMIN")]//chỉ có những user có role là admin chứa trong HttpContext là được sử dụng những api này
    public class AdminController : ApiController
    {
        private ManagerService managerService;

        public AdminController()
        {
            managerService = new ManagerServiceImpl();
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult getAnAdmin([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Manager manager = managerService.get(id);
            if (manager == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = manager;
            return Ok(response);
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult getAll()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Manager> list = managerService.getAll();
            if (list.Count == 0)
            {
                response.message = "No data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);

        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult create([FromBody] CreateManagerDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }

            ResponseDTO response = new ResponseDTO();
            Manager saveObj = new Manager();
            saveObj.fullName = dto.fullName.Trim();
            saveObj.username = dto.username.Trim();
            saveObj.password = Crypto.HashPassword(dto.password);
            saveObj.role = dto.role.Trim();
            bool saveState = managerService.create(saveObj);
            if (saveState)
            {
                response.message = "OK";
            }
            else
            {
                return InternalServerError();
            }
            return Created<ResponseDTO>("database", response);
        }

        [Route("update/fullname")]
        [HttpPut]
        public IHttpActionResult updateFullName([FromBody] UpdateFullNameManagerDTO dto)
        {

            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            bool updateState = managerService.updateFullName(dto.fullName, currentUser);
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

        [Route("update/password")]
        [HttpPut]
        public IHttpActionResult updatePassword([FromBody] UpdatePasswordManagerDTO dto)
        {

            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            bool updateState = managerService.updatePassword(dto.password, currentUser);
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

        [HttpDelete]
        [Route("ban/staff/{id:int:min(0)}")]
        public IHttpActionResult banAStaff([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            bool state = managerService.banAStaff(id);
            if (state)
            {
                response.message = "OK";
            }
            else
            {
                response.message = "Error occured";
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("ban/recruiter/{username}")]
        public IHttpActionResult banARecruiter([FromUri] string username)
        {
            ResponseDTO response = new ResponseDTO();
            bool state = managerService.banARecruiter(username);
            if (state)
            {
                response.message = "OK";
            }
            else
            {
                response.message = "Error occured";
            }
            return Ok(response);
        }

    }
}