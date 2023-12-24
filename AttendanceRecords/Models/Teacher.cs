using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Teacher
    {
        [HiddenInput(DisplayValue = false)]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите отчество")]
        [Display(Name = "Отчество")]
        public string? Midname { get; set; }

        [Display(Name = "Должность")]
        public string? Job { get; set; }

        [Display(Name = "Ученая степень, звание")]
        public string? Rank { get; set; }

        [Display(Name = "ФИО")]
        public string? FIO
        {
            get
            {
                return Surname + " " + Name + " " + Midname;
            }
        }

        public virtual ICollection<Schedule>? Schedule { get; set; }
    }
}
