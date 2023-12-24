using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceRecords.Models
{
    public class Status
    {
        [HiddenInput(DisplayValue = false)]
        public int StatusId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Skip>? Skip { get; set; }
    }
}
