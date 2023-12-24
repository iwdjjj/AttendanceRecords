using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Schedule
    {
        [HiddenInput(DisplayValue = false)] 
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Укажите дисциплину")]
        [Display(Name = "Дисциплина")]
        public int? SubjectId { get; set; }
        [Display(Name = "Дисциплина")]
        public virtual Subject? Subject { get; set; }
        [Display(Name = "Дисциплина")]
        public string? SubjectName { get; set; }

        [Required(ErrorMessage = "Укажите группу")]
        [Display(Name = "Группа")]
        public int? GroupId { get; set; }
        [Display(Name = "Группа")]
        public virtual Group? Group { get; set; }

        [Required(ErrorMessage = "Укажите преподавателя")]
        [Display(Name = "Преподаватель")]
        public int? TeacherId { get; set; }
        [Display(Name = "Преподаватель")]
        public virtual Teacher? Teacher { get; set; }

        public virtual ICollection<Skip>? Skip { get; set; }
    }
}
