using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Utils;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("manager")]
    [Authorize]
    public class ManagerInfoController : ApiController
    {
        private ManagerService managerService;

        public ManagerInfoController()
        {
            managerService = new ManagerServiceImpl();
        }

        //Trả về info của 1 manager sau khi login,check
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
    }
}