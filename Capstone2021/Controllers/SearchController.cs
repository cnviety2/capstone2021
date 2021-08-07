using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.WebPages;

namespace Capstone2021.Controllers
{
    [RoutePrefix("search")]
    [AllowAnonymous]
    public class SearchController : ApiController
    {
        private JobService jobService;

        public SearchController()
        {
            jobService = new JobServiceImpl();
        }

        //check
        [HttpPost]
        [Route("")]
        public IHttpActionResult search([FromBody] SearchJobDTO searchDTO)
        {
            if (searchDTO == null)
            {
                return BadRequest();
            }
            bool flag = false;
            if (searchDTO.isEmpty())
            {
                ModelState.AddModelError("dto", "Data tìm kiếm không được trống");
                flag = true;
            }
            if (searchDTO.workingForm.HasValue && (searchDTO.workingForm < 1 || searchDTO.workingForm > 3))
            {
                ModelState.AddModelError("dto.workingForm", "`Part-time:1,Full-time:2,Both:3");
                flag = true;
            }
            if (searchDTO.location.HasValue && (searchDTO.location < 1 || searchDTO.location > 24))
            {
                ModelState.AddModelError("dto.location", "Quận 1 -> 12 : 1 -> 12,13 : bình tân,14 : bình thạnh,15 : gò vấp,16 : phú nhuận,17 : tân bình,18 : " +
            "tân phú,19 : thủ đức,20 : bình chánh,21 : cần giờ,22 : củ chi,23 : hóc môn,24 : nhà bè");
                flag = true;
            }
            if (flag)
            {
                return BadRequest(ModelState);
            }
            if (searchDTO.keyword != null && !searchDTO.keyword.IsEmpty())
            {
                searchDTO.keyword = searchDTO.keyword.Trim();
                if (!searchDTO.keyword.IsNormalized(NormalizationForm.FormD))
                {
                    searchDTO.keyword = StringUtils.convertToUnSign3(searchDTO.keyword);
                }
            }
            IList<Job> listResult = jobService.search(searchDTO);
            ResponseDTO response = new ResponseDTO();
            if (listResult.Count == 0)
            {
                response.message = "Không có data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = listResult;
            return Ok(response);

        }
    }
}