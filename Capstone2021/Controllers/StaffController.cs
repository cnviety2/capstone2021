using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("staff")]
    [Authorize(Roles = "ROLE_STAFF")]//chỉ có những user có role là staff chứa trong HttpContext là được sử dụng những api này
    public class StaffController : ApiController
    {
        /**
         * Staff ko tạo được chính nó,admin mới tạo đc nên sẽ ko có api create 1 staff,staff cũng
         * ko xóa hay ban đc ai cả,staff chỉ có quyền read và update,ko có create và delete trong bộ crud cơ bản
         */

        private ManagerService managerService;

        public StaffController()
        {
            managerService = new ManagerServiceImpl();
        }

        [HttpGet]
        [Route("self")]
        public IHttpActionResult getSelfInfo()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int id = HttpContextUtils.getUserID(claims);
            Manager currentManager = managerService.get(id);
            ResponseDTO response = new ResponseDTO();
            if (currentManager == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = currentManager;
            return Ok(response);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult getAStaff([FromUri] int id)
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

        [Route("update/fullname")]
        [HttpPut]
        public IHttpActionResult updateFullName([FromBody] UpdateFullNameManagerDTO dto)
        {
            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
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
            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
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

    }
}