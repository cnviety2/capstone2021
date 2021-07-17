using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    public class CompanyController : ApiController
    {
        private CompanyService companyService;

        public CompanyController()
        {
            companyService = new CompanyServiceImpl();
        }

        [HttpGet]
        [Route("company")]
        [Authorize]
        public IHttpActionResult getAllCompany()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Company> list = companyService.getAll();
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
        [Route("company/{id:int:min(0)}")]
        [Authorize]
        public IHttpActionResult getACompany([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Company manager = companyService.get(id);
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

        [HttpPost]
        [Route("recruiter/company/create")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createACompany([FromBody] CreateCompanyDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int id = HttpContextUtils.getUserID(claims);
            bool createState = companyService.create(dto, id);
            if (createState)
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