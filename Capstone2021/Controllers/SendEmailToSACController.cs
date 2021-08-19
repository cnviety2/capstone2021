using Capstone2021.DTO;
using Capstone2021.Utils;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("public")]
    public class SendEmailToSACController : ApiController
    {
        [HttpPost]
        [Route("send-email")]
        [AllowAnonymous]
        public IHttpActionResult sendEmailToSAC([FromBody] SendEmailToSACDTO dto)
        {
            ResponseDTO response = EmailUtils.sendEmailToSAC(dto);
            return Ok(response);
        }
    }
}