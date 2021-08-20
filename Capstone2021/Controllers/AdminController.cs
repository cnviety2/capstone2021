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

        //trả về tất cả students ,có paging,check
        [Route("students")]
        [HttpGet]
        public IHttpActionResult getStudentWithPaging([FromUri] int page)
        {
            IList<Student> list = managerService.getListStudentsWithPaging(page);
            ResponseDTO response = new ResponseDTO();
            if (list.Count == 0)
            {
                response.message = "Không có kết quả";
                return Ok(response);
            }
            else
            {
                response.message = "OK";
                response.data = list;
                return Ok(response);
            }
        }

        //trả về recruiter,có paging,check
        [Route("recruiters")]
        [HttpGet]
        public IHttpActionResult getRecruiterWithPaging([FromUri] int page)
        {
            IList<ReturnRecruiterForAdminDTO> list = managerService.getListRecruiterWithPaging(page);
            ResponseDTO response = new ResponseDTO();
            if (list.Count == 0)
            {
                response.message = "Không có kết quả";
                return Ok(response);
            }
            else
            {
                response.message = "OK";
                response.data = list;
                return Ok(response);
            }
        }

        //trả về tất cả cate,check
        [Route("categories")]
        [HttpGet]
        public IHttpActionResult getAllCate()
        {
            IList<Category> list = managerService.getAllCategory();
            ResponseDTO response = new ResponseDTO();
            if (list.Count == 0)
            {
                response.message = "Không có kết quả";
                return Ok(response);
            }
            else
            {
                response.message = "OK";
                response.data = list;
                return Ok(response);
            }
        }

        [Route("categories/create")]
        [HttpPost]
        public IHttpActionResult createACate([FromBody] Category dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu bị null");
            }
            if (dto.value == null || dto.value.IsEmpty())
            {
                return BadRequest("Không được trống");
            }
            bool createState = managerService.createACategory(dto.value.Trim());
            if (createState == true)
            {
                ResponseDTO response = new ResponseDTO();
                response.message = "OK";
                return Ok(response);
            }
            else
            {
                return BadRequest("Lỗi xảy ra,hãy thử lại");
            }
        }

        [Route("categories/update")]
        [HttpPut]
        public IHttpActionResult updateACate([FromBody] Category dto)
        {
            if (dto.value == null || dto.value.IsEmpty())
            {
                return BadRequest("Không được trống");
            }
            int updateState = managerService.updateACategory(dto.id, dto.value.Trim());
            switch (updateState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Không tìm thấy");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        //trả về tất cả option lựa chọn ngày hiệu lực và giá tiền
        [Route("active-days-price")]
        [HttpGet]
        public IHttpActionResult getAllActiveDaysAndPrice()
        {
            IList<ActiveDaysAndPrice> list = managerService.getAllActiveDaysAndPrice();
            ResponseDTO response = new ResponseDTO();
            if (list.Count == 0)
            {
                response.message = "Không có kết quả";
                return Ok(response);
            }
            else
            {
                response.message = "OK";
                response.data = list;
                return Ok(response);
            }
        }

        //tạo mới 1 option ngày hiệu lực - giá tiền,check
        [Route("active-days-price/create")]
        [HttpPost]
        public IHttpActionResult createAnActiveDaysAndPrice([FromBody] ActiveDaysAndPrice dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu bị null");
            }
            if (dto.activeDays < 1 || dto.price < 1)
            {
                return BadRequest("Số ngày phải nhiều hơn 0 và giá trị của tiền phải lớn hơn 0");
            }
            int createState = managerService.createAnActiveDaysAndPrice(dto.activeDays, dto.price);
            switch (createState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Giá trị này đã tồn tại (số ngày hiệu lực hoặc giá tiền)");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        //update lại 1 giá trị ngày hiệu lực - giá tiền,check
        [Route("active-days-price/update")]
        [HttpPut]
        public IHttpActionResult updateAnActiveDaysAndPrice([FromBody] UpdateActiveDaysAndPriceDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu bị null");
            }
            if (dto.activeDays < 1 || dto.price < 1)
            {
                return BadRequest("Số ngày phải nhiều hơn 0 và giá trị của tiền phải lớn hơn 0");
            }
            int updateState = managerService.updateAnActiveDaysAndPrice(dto);
            switch (updateState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 4:
                    return BadRequest("Giá trị này đã tồn tại (số ngày hiệu lực hoặc giá tiền)");
                case 3:
                    return InternalServerError();
                case 2:
                    return BadRequest("Không tìm thấy");
                default:
                    return InternalServerError();
            }
        }

        //xóa 1 giá trị ngày hiệu lực - giá tiền,ít hơn 2 ko cho xóa,check
        [Route("active-days-price/{id}/delete")]
        [HttpDelete]
        public IHttpActionResult deleteAnActiveDaysAndPrice([FromUri] int id)
        {
            int deleteState = managerService.deleteAnActiceDaysAndPrice(id);
            switch (deleteState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Không tìm thấy");
                case 3:
                    return BadRequest("Không thể xóa vì chỉ còn lại 2 cặp giá trị lựa chọn");
                case 4:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        [Route("total-pages")]
        [HttpGet]
        public IHttpActionResult getTotalPage([FromUri] string choice)
        {
            if (!choice.Equals("recruiter") && !choice.Equals("student"))
            {
                return BadRequest("Choice: student hoặc recruiter");
            }
            int pages = managerService.getTotalPages(choice);
            ResponseDTO response = new ResponseDTO();
            response.message = "OK";
            response.data = pages;
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

        [HttpPut]
        [Route("unban/staff/{id:int:min(0)}")]
        public IHttpActionResult unbanAStaff([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            bool state = managerService.unbanAStaff(id);
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
        [Route("dashboard-data")]
        public IHttpActionResult getDashboardData()
        {
            ResponseDTO response = new ResponseDTO();
            DashboardDataDTO data = managerService.getDashboardData();
            response.data = data;
            response.message = "OK";
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
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Report tháng");
                    worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(5, 14)).Merge();
                    worksheet.Column(1).Width = 50;
                    worksheet.Row(5).Height = 15;
                    var imagePath =  AppDomain.CurrentDomain.BaseDirectory + "Controllers\\sac.png";
                    worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell(1, 1)).Scale(0.4);
                    worksheet.Cell(1, 1).Style.Font.SetFontSize(28);
                    worksheet.Cell(1, 1).Style.Font.SetBold(true);
                    worksheet.Cell(1, 1).Value = "                    Trung tâm Hỗ trợ học sinh, sinh viên Tp. Hồ Chí Minh";
                    worksheet.Cell(1,1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    worksheet.Cell(6, 4).Style.Font.SetItalic(true);
                    worksheet.Cell(6, 4).Style.Font.SetFontColor(XLColor.Red);
                    worksheet.Cell(6, 4).Value = "*Chú ý:tháng và quý có thể không đồng nhất(report được tạo ra dựa trên lựa chọn tháng và quý của người dùng)";
                    worksheet.Cell(7, 4).Value = "Ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                    //write các cell dữ liệu của report theo tháng
                    ReportByMonthDTO reportByThisMonth = report.listReportByMonth.ToArray()[0];
                    ReportByMonthDTO reportByPreviousMonth = report.listReportByMonth.ToArray()[1];
                    worksheet.Cell(9, 2).Style.Font.SetFontSize(26);
                    worksheet.Cell(9, 2).Style.Font.SetBold(true);
                    worksheet.Cell(9, 2).Value = "Báo cáo của tháng " + dto.month + "/" + DateTime.Now.Year;
                    var range = worksheet.Range(worksheet.Cell(11,1),worksheet.Cell(15,9));
                    range.Style.Font.FontSize = 16;
                    IXLTable table = range.CreateTable();
                    table.ShowAutoFilter = false;
                    table.ShowHeaderRow = false;
                    worksheet.Range(worksheet.Cell(11, 1), worksheet.Cell(11, 9)).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(worksheet.Cell("G12"), worksheet.Cell("H15")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 8).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 7).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Value = "đơn vị";
                    worksheet.Cell(11, 1).Value = "Tháng";
                    if (dto.month == 1)
                    {
                        worksheet.Column(7).Width = 15;
                        worksheet.Cell(11, 7).SetValue<string>(Convert.ToString(reportByPreviousMonth.month));
                    }
                    else
                    {
                        worksheet.Cell(11, 7).Value = reportByPreviousMonth.month;
                    }
                    worksheet.Column(9).Width = 15;
                    worksheet.Cell(11, 8).Value = reportByThisMonth.month;
                    worksheet.Cell(12, 1).Value = "Nhà tuyển dụng tham gia vào hệ thống";
                    worksheet.Cell(12, 7).Value = reportByPreviousMonth.numberOfRecruiters;
                    worksheet.Cell(12, 8).Value = reportByThisMonth.numberOfRecruiters;
                    worksheet.Cell(12, 9).Value = "người dùng";

                    worksheet.Cell(13, 1).Value = "Việc làm được đăng lên";
                    worksheet.Cell(13, 7).Value = reportByPreviousMonth.numberOfJobs;
                    worksheet.Cell(13, 8).Value = reportByThisMonth.numberOfJobs;
                    worksheet.Cell(13, 9).Value = "việc làm";

                    worksheet.Cell(14, 1).Value = "Sinh viên nhà tuyển dụng có nhu cầu tuyển dụng";
                    worksheet.Cell(14, 7).Value = reportByPreviousMonth.numberOfDesiredStudents;
                    worksheet.Cell(14, 8).Value = reportByThisMonth.numberOfDesiredStudents;
                    worksheet.Cell(14, 9).Value = "sinh viên";

                    worksheet.Cell(15, 1).Value = "Sinh viên tham gia tìm việc";
                    worksheet.Cell(15, 7).Value = reportByPreviousMonth.numberOfStudents;
                    worksheet.Cell(15, 8).Value = reportByThisMonth.numberOfStudents;
                    worksheet.Cell(15, 9).Value = "sinh viên";

                    worksheet.Cell(11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 1).Style.Font.Bold = true;
                    worksheet.Cell(11, 7).Style.Font.Bold = true;
                    worksheet.Cell(11, 8).Style.Font.Bold = true;
                    worksheet.Cell(11, 9).Style.Font.Bold = true;
                    //hết tháng

                    worksheet = workbook.Worksheets.Add("Report quý");
                    worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(5, 14)).Merge();
                    worksheet.Column(1).Width = 50;
                    worksheet.Column(9).Width = 15;
                    worksheet.Row(5).Height = 15;
                    worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell(1, 1)).Scale(0.4);
                    worksheet.Cell(1, 1).Style.Font.SetFontSize(28);
                    worksheet.Cell(1, 1).Style.Font.SetBold(true);
                    worksheet.Cell(1, 1).Value = "                    Trung tâm Hỗ trợ học sinh, sinh viên Tp. Hồ Chí Minh";
                    worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    worksheet.Cell(6, 4).Style.Font.SetItalic(true);
                    worksheet.Cell(6, 4).Style.Font.SetFontColor(XLColor.Red);
                    worksheet.Cell(6, 4).Value = "*Chú ý:tháng và quý có thể không đồng nhất(report được tạo ra dựa trên lựa chọn tháng và quý của người dùng)";
                    worksheet.Cell(7, 4).Value = "Ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                    ReportyByQuarterDTO reportByQuarterThisYear = report.listReportByQuarter.ToArray()[0];
                    ReportyByQuarterDTO reportByQuarterPreviousYear = report.listReportByQuarter.ToArray()[1];
                    worksheet.Cell(9, 2).Style.Font.SetFontSize(26);
                    worksheet.Cell(9, 2).Style.Font.SetBold(true);
                    worksheet.Cell(9, 2).Value = "Báo cáo theo quý " + dto.quarter;
                    range = worksheet.Range(worksheet.Cell(11, 1), worksheet.Cell(14, 9));
                    range.Style.Font.FontSize = 16;
                    table = range.CreateTable();
                    table.ShowAutoFilter = false;
                    table.ShowHeaderRow = false;
                    range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(worksheet.Cell("G12"), worksheet.Cell("H15")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(worksheet.Cell(11, 1), worksheet.Cell(11, 9)).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    worksheet.Cell(11, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 8).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 7).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Value = "đơn vị";
                    worksheet.Cell(11, 1).Value = "Năm";
                    worksheet.Cell(11, 7).Value = reportByQuarterPreviousYear.year;
                    worksheet.Cell(11, 8).Value = reportByQuarterThisYear.year;

                    worksheet.Cell(12, 1).Value = "Nhà tuyển dụng tham gia vào hệ thống";
                    worksheet.Cell(12, 7).Value = reportByQuarterPreviousYear.numberOfRecruiters;
                    worksheet.Cell(12, 8).Value = reportByQuarterThisYear.numberOfRecruiters;
                    worksheet.Cell(12, 9).Value = "người dùng";

                    worksheet.Cell(13, 1).Value = "Lượng việc làm được đăng lên";
                    worksheet.Cell(13, 7).Value = reportByQuarterPreviousYear.numberOfJobs;
                    worksheet.Cell(13, 8).Value = reportByQuarterThisYear.numberOfJobs;
                    worksheet.Cell(13, 9).Value = "việc làm";

                    worksheet.Cell(14, 1).Value = "Lượng sinh viên nhà tuyển dụng có nhu cầu tuyển dụng";
                    worksheet.Cell(14, 7).Value = reportByQuarterPreviousYear.numberOfDesiredStudents;
                    worksheet.Cell(14, 8).Value = reportByQuarterThisYear.numberOfDesiredStudents;
                    worksheet.Cell(14, 9).Value = "sinh viên";

                    worksheet.Cell(11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 1).Style.Font.Bold = true;
                    worksheet.Cell(11, 7).Style.Font.Bold = true;
                    worksheet.Cell(11, 8).Style.Font.Bold = true;
                    worksheet.Cell(11, 9).Style.Font.Bold = true;
                    //hết quý

                    worksheet = workbook.Worksheets.Add("Report năm");
                    worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(5, 14)).Merge();
                    worksheet.Column(1).Width = 50;
                    worksheet.Column(9).Width = 15;
                    worksheet.Row(5).Height = 15;
                    worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell(1, 1)).Scale(0.4);
                    worksheet.Cell(1, 1).Style.Font.SetFontSize(28);
                    worksheet.Cell(1, 1).Style.Font.SetBold(true);
                    worksheet.Cell(1, 1).Value = "                    Trung tâm Hỗ trợ học sinh, sinh viên Tp. Hồ Chí Minh";
                    worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    worksheet.Cell(6, 4).Style.Font.SetItalic(true);
                    worksheet.Cell(6, 4).Style.Font.SetFontColor(XLColor.Red);
                    worksheet.Cell(6, 4).Value = "*Chú ý:tháng và quý có thể không đồng nhất(report được tạo ra dựa trên lựa chọn tháng và quý của người dùng)";
                    worksheet.Cell(7, 4).Value = "Ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                    ReportByYearDTO reportByThisYear = report.listreportByYear.ToArray()[0];
                    ReportByYearDTO reportByPreviousYear = report.listreportByYear.ToArray()[1];
                    worksheet.Cell(9, 2).Style.Font.SetFontSize(26);
                    worksheet.Cell(9, 2).Style.Font.SetBold(true);
                    worksheet.Cell(9, 2).Value = "Báo cáo theo năm";
                    range = worksheet.Range(worksheet.Cell(11, 1), worksheet.Cell(14, 9));
                    range.Style.Font.FontSize = 16;
                    table = range.CreateTable();
                    table.ShowAutoFilter = false;
                    table.ShowHeaderRow = false;
                    range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(worksheet.Cell("G12"), worksheet.Cell("H15")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(worksheet.Cell(11, 1), worksheet.Cell(11, 9)).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    worksheet.Cell(11, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 8).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 7).Style.Font.FontSize = 16;
                    worksheet.Cell(11, 9).Value = "đơn vị";
                    worksheet.Cell(11, 1).Value = "Năm";
                    worksheet.Cell(11, 7).Value = reportByPreviousYear.year;
                    worksheet.Cell(11, 8).Value = reportByThisYear.year;

                    worksheet.Cell(12, 1).Value = "Nhà tuyển dụng tham gia vào hệ thống";
                    worksheet.Cell(12, 7).Value = reportByPreviousYear.numberOfRecruiters;
                    worksheet.Cell(12, 8).Value = reportByThisYear.numberOfRecruiters;
                    worksheet.Cell(12, 9).Value = "người dùng";

                    worksheet.Cell(13, 1).Value = "Lượng việc làm được đăng lên";
                    worksheet.Cell(13, 7).Value = reportByPreviousYear.numberOfJobs;
                    worksheet.Cell(13, 8).Value = reportByThisYear.numberOfJobs;
                    worksheet.Cell(13, 9).Value = "việc làm";

                    worksheet.Cell(14, 1).Value = "Lượng sinh viên nhà tuyển dụng có nhu cầu tuyển dụng";
                    worksheet.Cell(14, 7).Value = reportByPreviousYear.numberOfDesiredStudents;
                    worksheet.Cell(14, 8).Value = reportByThisYear.numberOfDesiredStudents;
                    worksheet.Cell(14, 9).Value = "sinh viên";

                    worksheet.Cell(11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(11, 1).Style.Font.Bold = true;
                    worksheet.Cell(11, 7).Style.Font.Bold = true;
                    worksheet.Cell(11, 8).Style.Font.Bold = true;
                    worksheet.Cell(11, 9).Style.Font.Bold = true;
                    //hết năm

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