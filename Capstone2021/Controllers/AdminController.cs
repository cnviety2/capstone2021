using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Utils;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.WebPages;

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

        //lấy tất cả những staff
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

        [Route("create")]
        [HttpPost]
        public IHttpActionResult create([FromBody] CreateManagerDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            if (StringUtils.isContainSpecialCharacter(dto.username))
            {
                return BadRequest("Username không chứa ký tự đặc biệt");
            }
            ResponseDTO response = new ResponseDTO();
            Manager saveObj = new Manager();
            saveObj.fullName = dto.fullName.Trim();
            saveObj.username = dto.username.Trim();
            saveObj.password = Crypto.HashPassword(dto.password);
            saveObj.role = "ROLE_STAFF";
            int saveState = managerService.createAStaff(saveObj);
            switch (saveState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Đã tồn tại username");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
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
                default:
                    return InternalServerError();
            }
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
                return InternalServerError();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("report/by-quarter")]
        public IHttpActionResult reportByQuarter([FromUri] int quarter)
        {
            if (quarter < 1 || quarter > 4)
            {
                return BadRequest("Quý từ 1 đến 4");
            }
            ResponseDTO response = new ResponseDTO();
            ReportyByQuarterDTO report = managerService.reportByQuarter(quarter);
            response.message = "OK";
            response.data = report;
            return Ok(response);
        }

        [HttpGet]
        [Route("report/by-month")]
        public IHttpActionResult reportByMonth([FromUri] int month)
        {
            if (month < 1 || month > 12)
            {
                return BadRequest("Tháng từ 1 đến 12");
            }
            ResponseDTO response = new ResponseDTO();
            ReportByMonthDTO report = managerService.reportByMonth(month);
            response.message = "OK";
            response.data = report;
            return Ok(response);
        }

        [HttpGet]
        [Route("report/by-year")]
        public IHttpActionResult reportByYear()
        {
            ResponseDTO response = new ResponseDTO();
            ReportByYearDTO report = managerService.reportByYear();
            response.message = "OK";
            response.data = report;
            return Ok(response);
        }

        [HttpPost]
        [Route("generate-report")]
        public async Task<HttpResponseMessage> generateReport([FromBody] RequestForReportDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
            if (dto == null)
            {
                //return BadRequest("Không được null");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            ComboReportDTO report = managerService.generateReport(dto);
            //generate file
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "report.xlsx";
            var workbook = new XLWorkbook();
            try
            {
                using (workbook)
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Report");
                    worksheet.Cell(1, 1).Style.Font.SetFontSize(20);
                    worksheet.Cell(1, 1).Style.Font.SetBold(true);
                    worksheet.Cell(1, 1).Value = "Report theo tháng " + dto.month + ",quý " + dto.quarter + ",năm " + DateTime.Now.Year;
                    worksheet.Cell(2, 7).Style.Font.SetItalic(true);
                    worksheet.Cell(2, 7).Style.Font.SetFontColor(XLColor.Red);
                    worksheet.Cell(2, 7).Value = "*Chú ý:tháng và quý có thể không đồng nhất(report được tạo ra dựa trên lựa chọn tháng và quý của người dùng)";
                    worksheet.Cell(3, 7).Value = DateTime.Now.ToString("dd/MM/yyyy");

                    //write các cell dữ liệu của report theo tháng
                    worksheet.Cell(5, 1).Style.Font.SetFontSize(15);
                    worksheet.Cell(5, 1).Value = "Báo cáo của tháng " + dto.month + "/" + DateTime.Now.Year;
                    worksheet.Cell(5, 11).Value = "Đơn vị";
                    ReportByMonthDTO reportByMonth = report.reportByMonth;
                    worksheet.Cell(6, 2).Value = "Số nhà tuyển dụng tham gia vào hệ thống:";
                    worksheet.Cell(6, 10).Value = reportByMonth.numberOfRecruiters;
                    worksheet.Cell(6, 11).Value = "người dùng";

                    worksheet.Cell(7, 2).Value = "Số lượng việc làm được đăng lên:";
                    worksheet.Cell(7, 10).Value = reportByMonth.numberOfJobs;
                    worksheet.Cell(7, 11).Value = "việc làm";

                    worksheet.Cell(8, 2).Value = "Số lượng sinh viên nhà tuyển dụng có nhu cầu tuyển dụng:";
                    worksheet.Cell(8, 10).Value = reportByMonth.numberOfDesiredStudents;
                    worksheet.Cell(8, 11).Value = "sinh viên";

                    worksheet.Cell(9, 2).Value = "Só lượng sinh viên tham gia tìm việc:";
                    worksheet.Cell(9, 10).Value = reportByMonth.numberOfStudents;
                    worksheet.Cell(9, 11).Value = "sinh viên";

                    //theo quý
                    ReportyByQuarterDTO reportByQuarter = report.reportbyQuarter;
                    worksheet.Cell(11, 1).Style.Font.SetFontSize(15);
                    worksheet.Cell(11, 1).Value = "Báo cáo của quý " + dto.quarter + "/" + DateTime.Now.Year;

                    worksheet.Cell(12, 2).Value = "Số nhà tuyển dụng tham gia vào hệ thống:";
                    worksheet.Cell(12, 10).Value = reportByQuarter.numberOfRecruiters;
                    worksheet.Cell(12, 11).Value = "người dùng";

                    worksheet.Cell(13, 2).Value = "Số lượng việc làm được đăng lên:";
                    worksheet.Cell(13, 10).Value = reportByQuarter.numberOfJobs;
                    worksheet.Cell(13, 11).Value = "việc làm";

                    worksheet.Cell(14, 2).Value = "Số lượng sinh viên nhà tuyển dụng có nhu cầu tuyển dụng:";
                    worksheet.Cell(14, 10).Value = reportByQuarter.numberOfDesiredStudents;
                    worksheet.Cell(14, 11).Value = "sinh viên";

                    //theo năm
                    ReportByYearDTO reportByYear = report.reportByYear;
                    worksheet.Cell(16, 1).Style.Font.SetFontSize(15);
                    worksheet.Cell(16, 1).Value = "Báo cáo của năm " + DateTime.Now.Year;

                    worksheet.Cell(17, 2).Value = "Số nhà tuyển dụng tham gia vào hệ thống:";
                    worksheet.Cell(17, 10).Value = reportByYear.numberOfRecruiters;
                    worksheet.Cell(17, 11).Value = "người dùng";

                    worksheet.Cell(18, 2).Value = "Số lượng việc làm được đăng lên:";
                    worksheet.Cell(18, 10).Value = reportByYear.numberOfJobs;
                    worksheet.Cell(18, 11).Value = "việc làm";

                    worksheet.Cell(19, 2).Value = "Số lượng sinh viên nhà tuyển dụng có nhu cầu tuyển dụng:";
                    worksheet.Cell(19, 10).Value = reportByYear.numberOfDesiredStudents;
                    worksheet.Cell(19, 11).Value = "sinh viên";


                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(stream.ToArray())
                        };
                        result.Content.Headers.ContentDisposition =
                            new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                            {
                                FileName = fileName
                            };
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}