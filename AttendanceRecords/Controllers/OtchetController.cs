using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using AttendanceRecords.Data;
using AttendanceRecords.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Group = AttendanceRecords.Models.Group;
using System.ComponentModel;

namespace AttendanceRecords.Controllers
{
    public class OtchetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtchetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OtchetController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile fileExcel)
        {
            using (XLWorkbook workbook = new XLWorkbook(fileExcel.OpenReadStream()))
            {
                List<Student_ImpExp> Student_ImpExps = new List<Student_ImpExp>();
                List<Schedule_ImpExp> Schedule_ImpExps = new List<Schedule_ImpExp>();

                foreach (IXLWorksheet worksheet in workbook.Worksheets)
                {
                    if (worksheet.Name == "Student")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Student students = new Student();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            students.Surname = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Surname").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            students.Name = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Name").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            students.Midname = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Midname").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            students.GroupId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "GroupId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            students.RecordBook = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "RecordBook").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            students.Mail = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Mail").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            _context.Student.Add(students);

                            _context.SaveChanges();

                            Student_ImpExps.Add(new Student_ImpExp { StudentSubd = students.StudentId, StudentExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "StudentId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "Schedule")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Schedule schedules = new Schedule();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            schedules.Date = DateTime.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Date").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            schedules.SubjectId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "SubjectId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            schedules.GroupId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "GroupId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            schedules.TeacherId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "TeacherId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());

                            _context.Schedule.Add(schedules);

                            _context.SaveChanges();

                            Schedule_ImpExps.Add(new Schedule_ImpExp { ScheduleSubd = schedules.ScheduleId, ScheduleExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ScheduleId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "Skip")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Skip skips = new Skip();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            skips.Date = DateTime.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Date").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            skips.ScheduleId = Schedule_ImpExps.FirstOrDefault(c => c.ScheduleExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ScheduleId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).ScheduleSubd;
                            skips.StudentId = Student_ImpExps.FirstOrDefault(c => c.StudentExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "StudentId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).StudentSubd;
                            skips.StatusId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "StatusId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());

                            _context.Skip.Add(skips);

                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OtchetController/Details/5
        public ActionResult Export(int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Otchet");
                worksheet.Cell(1, 1).Value = "Студент";
                worksheet.Cell(1, 2).Value = "Посещаемость";

                worksheet.Row(1).Style.Font.Bold = true;

                var otch = _context.Set<Skip_CountOtchet>().FromSqlInterpolated($"EXEC Otchet").ToList();
                int i = 2;
                foreach (Skip_CountOtchet item in otch)
                {
                    worksheet.Cell(i, 1).Value = item.nm;
                    worksheet.Cell(i, 2).Value = item.kol;
                    i++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Otchet_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }

        public ActionResult Export2()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet1 = workbook.Worksheets.Add("Student");
                worksheet1.Cell(1, 1).Value = "StudentId";
                worksheet1.Cell(1, 2).Value = "Surname";
                worksheet1.Cell(1, 3).Value = "Name";
                worksheet1.Cell(1, 4).Value = "Midname";
                worksheet1.Cell(1, 5).Value = "GroupId";
                worksheet1.Cell(1, 6).Value = "RecordBook";
                worksheet1.Cell(1, 7).Value = "Mail";

                int i1 = 2;

                worksheet1.Row(1).Style.Font.Bold = true;

                foreach (Student item in _context.Student)
                {
                    worksheet1.Cell(i1, 1).Value = item.StudentId;
                    worksheet1.Cell(i1, 2).Value = item.Surname;
                    worksheet1.Cell(i1, 3).Value = item.Name;
                    worksheet1.Cell(i1, 4).Value = item.Midname;
                    worksheet1.Cell(i1, 5).Value = item.GroupId;
                    worksheet1.Cell(i1, 6).Value = item.RecordBook;
                    worksheet1.Cell(i1, 7).Value = item.Mail;

                    i1++;
                }

                var worksheet2 = workbook.Worksheets.Add("Schedule");
                worksheet2.Cell(1, 1).Value = "ScheduleId";
                worksheet2.Cell(1, 2).Value = "Date";
                worksheet2.Cell(1, 3).Value = "SubjectId";
                worksheet2.Cell(1, 4).Value = "GroupId";
                worksheet2.Cell(1, 5).Value = "TeacherId";

                int i2 = 2;

                worksheet2.Row(1).Style.Font.Bold = true;

                foreach (Schedule item in _context.Schedule)
                {
                    worksheet2.Cell(i2, 1).Value = item.ScheduleId;
                    worksheet2.Cell(i2, 2).Value = item.Date;
                    worksheet2.Cell(i2, 3).Value = item.SubjectId;
                    worksheet2.Cell(i2, 4).Value = item.GroupId;
                    worksheet2.Cell(i2, 5).Value = item.TeacherId;

                    i2++;
                }

                var worksheet = workbook.Worksheets.Add("Skip");
                worksheet.Cell(1, 1).Value = "SkipId";
                worksheet.Cell(1, 2).Value = "Date";
                worksheet.Cell(1, 3).Value = "ScheduleId";
                worksheet.Cell(1, 4).Value = "StudentId";
                worksheet.Cell(1, 5).Value = "StatusId";

                worksheet.Row(1).Style.Font.Bold = true;

                int i = 2;
                foreach (Skip item in _context.Skip)
                {
                    worksheet.Cell(i, 1).Value = item.SkipId;
                    worksheet.Cell(i, 2).Value = item.Date;
                    worksheet.Cell(i, 3).Value = item.ScheduleId;
                    worksheet.Cell(i, 4).Value = item.StudentId;
                    worksheet.Cell(i, 5).Value = item.StatusId;

                    i++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Export_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
