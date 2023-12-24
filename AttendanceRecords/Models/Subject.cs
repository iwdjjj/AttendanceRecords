using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Subject
    {
        [HiddenInput(DisplayValue = false)]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Укажите название")]
        [Display(Name = "Название дисциплины")]
        public string? Name { get; set; }

        [Display(Name = "Описание дисциплины")]
        public string? Description { get; set; }

        public virtual ICollection<Schedule>? Schedule { get; set; }
    }
}
