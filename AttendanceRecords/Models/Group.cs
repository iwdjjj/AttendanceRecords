using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Group
    {
        [HiddenInput(DisplayValue = false)]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Укажите группу")]
        [Display(Name = "Группа")]
        public string? GroupName { get; set; }

        [Required(ErrorMessage = "Укажите направление подготовки")]
        [Display(Name = "Направление подготовки")]
        public string? GroupJob { get; set; }

        [Required(ErrorMessage = "Укажите год поступления")]
        [Display(Name = "Год поступления")]
        public string? GroupYear { get; set; }

        [Display(Name = "Количество студентов")]
        public int? Student_Count { get; set; }

        public virtual ICollection<Student>? Student { get; set; }
        public virtual ICollection<Schedule>? Schedule { get; set; }
    }
}
