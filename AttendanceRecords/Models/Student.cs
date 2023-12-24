using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Student
    {
        [HiddenInput(DisplayValue = false)]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите отчество")]
        [Display(Name = "Отчество")]
        public string? Midname { get; set; }

        [Required(ErrorMessage = "Укажите групу")]
        [Display(Name = "Группа")]
        public int? GroupId { get; set; }
        [Display(Name = "Группа")]
        public virtual Group? Group { get; set; }

        [Display(Name = "Номер зачетной книжки")]
        public string? RecordBook { get; set; }

        [Display(Name = "Электронная почта")]
        public string? Mail { get; set; }

        [Display(Name = "ФИО")]
        public string? FIO
        {
            get
            {
                return Surname + " " + Name + " " + Midname;
            }
        }

        public virtual ICollection<Skip>? Skip { get; set; }
    }
}
