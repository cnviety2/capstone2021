using Capstone2021.DTO;
using Capstone2021.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
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
                response.message = "Not found";
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

        [Route("remove/{id:int:min(0)}")]
        [HttpDelete]
        public IHttpActionResult remove([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            IIdentity identity = HttpContext.Current.User.Identity;/*HttpContext sẽ lưu lại những data đã xác thực của người dùng,những data này đã set ở class SimpleAuthorizationServerProvider
                                                                   bây giờ sẽ lấy data đó ra để xác định user đã gửi request có thể thực hiện hành động remove này ko,vì user1 thì ko thể remove chính nó*/
            String username = identity.Name;
            Manager checkManager = managerService.get(id);
            if (checkManager == null)
            {
                response.message = "Doesn't exist";
                return Ok(response);
            }
            //Trường hợp username gửi request xóa chính nó trong db
            if (username.Equals(checkManager.username))
            {
                response.message = "Can't remove itself";
                return Ok(response);
            }
            //Trường hợp username này muốn xóa 1 username khác có role là admin
            if (checkManager.role.Equals("ROLE_ADMIN"))
            {
                response.message = "An admin can't remove an admin";
                return Ok(response);
            }
            //thực hiện xóa trong db,chưa làm
            response.message = "OK";
            return Ok(response);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult create([FromBody] CreateManagerDTO manager)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Manager saveObj = new Manager();
            saveObj.fullName = manager.fullName.Trim();
            saveObj.username = manager.username.Trim();
            saveObj.password = Crypto.HashPassword(manager.password.Trim());
            saveObj.role = manager.role.Trim();
            bool saveState = managerService.create(saveObj);
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
    }
}