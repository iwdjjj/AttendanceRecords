using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Skip
    {        
        [HiddenInput(DisplayValue = false)]
        public int SkipId { get; set; }
        
        [Required(ErrorMessage = "Укажите дату")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Занятие")]
        public int? ScheduleId { get; set; }
        [Display(Name = "Занятие")]
        public virtual Schedule? Schedule { get; set; }

        [Required(ErrorMessage = "Укажите студента")]
        [Display(Name = "Студент")]
        public int? StudentId { get; set; }
        [Display(Name = "Студент")]
        public virtual Student? Student { get; set; }

        [Required(ErrorMessage = "Укажите статус")]
        [Display(Name = "Статус")]
        public int? StatusId { get; set; }
        [Display(Name = "Статус")]
        public virtual Status? Status { get; set; }
    }
}
