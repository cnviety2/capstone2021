using Capstone2021.DTO;
using Capstone2021.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AdminController : ApiController
    {
        private ManagerService managerService = null;

        public AdminController()
        {
            managerService = new ManagerServiceImpl();
        }

        [Route("{id}")]
        public IHttpActionResult getAnAdmin([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Manager manager = managerService.get(id);
            if (manager == null)
            {
                response.message = "Not found";
            }
            else
            {
                response.message = "OK";
            }
            response.data = manager;
            return Ok(response);
        }


    }
}