using AttendanceRecords.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceRecords.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Group> Group { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Skip> Skip { get; set; }        
        public DbSet<Status> Status { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Doljnost> Doljnosts { get; set; }

        public DbSet<Skip_CountOtchet> Skip_CountOtchet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable(tb => tb.HasTrigger("AddDelCount"));
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable(tb => tb.HasTrigger("UpdCount"));
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Schedule>().ToTable(tb => tb.HasTrigger("Changes"));
            base.OnModelCreating(modelBuilder);
        }
    }
}