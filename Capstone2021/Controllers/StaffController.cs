using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;

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
                response.message = "Không có dữ liệu";
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
            if (dto.oldPassword == null || dto.oldPassword.IsEmpty())
            {
                ModelState.AddModelError("dto.oldPassword", "Password cũ không được thiếu");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            int updateState = managerService.updatePassword(dto.password, currentUser, dto.oldPassword);
            switch (updateState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Sai password cũ");
                case 3:
                    return InternalServerError();
                case 4:
                    return BadRequest("Tài khoản đã bị ban");
                default:
                    return InternalServerError();
            }
        }

        //check
        [HttpPut]
        [Route("update/banner")]
        public IHttpActionResult updateBanner()
        {

            NameValueCollection formValue = HttpContext.Current.Request.Form;
            UpdateBannerDTO dto = new UpdateBannerDTO();
            dto.url = formValue.Get("url");
            try
            {
                dto.id = Int32.Parse(formValue.Get("id"));
            }
            catch (Exception e)
            {
                return BadRequest("Id là số");
            }
            if (dto.id < 1 || dto.id > 4)
            {
                return BadRequest("Id chỉ từ 1 -> 4");
            }
            if (dto == null || dto.isEmpty())
            {
                return BadRequest("Không được empty");
            }
            else
            {
                if (dto.url != null || !dto.url.IsEmpty())
                {
                    if (!StringUtils.isValidHttpUrl(dto.url))
                        return BadRequest("Không đúng định dạng 1 url,VD:http//asd.com");
                }
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int staffId = HttpContextUtils.getUserID(claims);
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (!file.ContentType.Equals("image/jpeg") && !file.ContentType.Equals("image/png"))
                {
                    return BadRequest("Only jpeg or png");
                }
                using (Stream st = file.InputStream)
                {
                    string name = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1];
                    string myBucketName = "capstone2021-fpt";//your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    String imgUrl = null;
                    if (a == true)
                    {
                        imgUrl = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + name;
                    }
                    else
                    {
                        imgUrl = "";
                        return BadRequest("Lỗi,không thể upload ảnh lên server AWS");
                    }
                    dto.imgUrl = imgUrl;
                }
            }
            int updateState = managerService.updateBanner(dto, staffId);
            switch (updateState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("User bị ban");
                case 3:
                    return InternalServerError();
                case 4:
                    return BadRequest("Không tìm thấy banner");
                default:
                    return InternalServerError();
            }
        }

        //check
        [HttpGet]
        [Route("banners")]
        public IHttpActionResult getAllBanners()
        {
            IList<Banner> result = managerService.getAllBanners();
            ResponseDTO response = new ResponseDTO();
            response.data = result;
            response.message = "OK";
            return Ok(response);
        }

    }
}