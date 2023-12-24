using Microsoft.EntityFrameworkCore;

namespace AttendanceRecords.Models
{
    [Keyless]
    public class Skip_CountOtchet
    {
        public int? id { get; set; }
        public string? nm { get; set; }
        public int? kol { get; set; }
    }
}
