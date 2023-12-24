using AttendanceRecords.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AttendanceRecords.Data

{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (context.Group.Any())
            {
                return;
            }

            Status status = new Status 
            { 
                Name = "Опоздание"
            };
            context.Status.Add(status);
            context.SaveChanges();

            Status status1 = new Status
            {
                Name = "Уважительная причина отсутствия"
            };
            context.Status.Add(status1);
            context.SaveChanges();

            Status status2 = new Status
            {
                Name = "Неуважительная или неизвестная причина отсутствия"
            };
            context.Status.Add(status2);
            context.SaveChanges();

            Teacher teacher = new Teacher
            {
                Surname = "Александров",
                Name = "Александр",
                Midname = "Александрович",
                Job = "Заведующий кафедрой",
                Rank = "Доктор педагогических наук, профессор"
            };
            context.Teacher.Add(teacher);
            context.SaveChanges();

            Teacher teacher1 = new Teacher
            {
                Surname = "Васильева",
                Name = "Вера",
                Midname = "Васильевна",
                Job = "Доцент кафедры",
                Rank = "Кандидат технических наук, доцент"
            };
            context.Teacher.Add(teacher1);
            context.SaveChanges();

            Subject subject = new Subject
            {
                Name = "Дисциплина1",
                Description = "1"
            };
            context.Subject.Add(subject);
            context.SaveChanges();

            Subject subject1 = new Subject
            {
                Name = "Дисциплина2",
                Description = "2"
            };
            context.Subject.Add(subject1);
            context.SaveChanges();

            Group group = new Group
            {
                GroupName = "КТ-41-20",
                GroupJob = "Прикладная информатика",
                GroupYear = "2020"
            };
            context.Group.Add(group);
            context.SaveChanges();

            Group group1 = new Group
            {
                GroupName = "КТ-31-20",
                GroupJob = "Программная инженерия",
                GroupYear = "2020"
            };
            context.Group.Add(group1);
            context.SaveChanges();

            Student student = new Student
            {
                Surname = "Александрова",
                Name = "Александра",
                Midname = "Александровна",
                GroupId = group.GroupId,
                RecordBook = "123456789",
                Mail = "alex@mail.ru"
            };
            context.Student.Add(student);
            context.SaveChanges();

            Student student1 = new Student
            {
                Surname = "Иванова",
                Name = "Ксения",
                Midname = "Викторовна",
                GroupId = group1.GroupId,
                RecordBook = "123456789",
                Mail = "ivanova@mail.ru"
            };
            context.Student.Add(student1);
            context.SaveChanges();

            Schedule schedule = new Schedule
            {
                Date = new DateTime(2023, 12, 26, 13, 30, 0),
                SubjectId = subject.SubjectId,
                GroupId = group.GroupId,
                TeacherId = teacher.TeacherId
            };
            context.Schedule.Add(schedule);
            context.SaveChanges();

            Schedule schedule1 = new Schedule
            {
                Date = new DateTime(2023, 12, 27, 12, 30, 0),
                SubjectId = subject1.SubjectId,
                GroupId = group1.GroupId,
                TeacherId = teacher1.TeacherId
            };
            context.Schedule.Add(schedule1);
            context.SaveChanges();

            Skip skip = new Skip
            {
                Date = new DateTime(2023, 12, 26, 13, 30, 0),
                ScheduleId = schedule.ScheduleId,
                StudentId = student.StudentId,
                StatusId = status.StatusId
            };
            context.Skip.Add(skip);
            context.SaveChanges();

            Doljnost doljnost1 = new Doljnost
            {
                DoljnostName = "Сотрудник"
            };
            context.Doljnosts.Add(doljnost1);

            Doljnost doljnost2 = new Doljnost
            {
                DoljnostName = "Студент"
            };
            context.Doljnosts.Add(doljnost2);

            context.SaveChanges();

            string[] roles = new string[] { "Administrator", "Guest" };
            foreach (string role in roles)
            {
                CreateRole(serviceProvider, role);
            }

            CustomUser customUser1 = new() { Surname = "Alekseev", Name = "Aleksei", Midname = "Alekseevich", UserName = "alekseev@mail.ru", Email = "alekseev@mail.ru", DoljnostId = doljnost1.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Administrator", customUser1);

            CustomUser customUser2 = new() { Surname = "Ivanov", Name = "Ivan", Midname = "Ivanovich", UserName = "ivanov@mail.ru", Email = "ivanov@mail.ru", DoljnostId = doljnost2.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Guest", customUser2);
        }

        private static void CreateRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                roleResult.Wait();
            }

        }

        private static void AddUserToRole(IServiceProvider serviceProvider, string userPwd, string roleName, CustomUser customUser)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            Task<CustomUser> checkAppUser = userManager.FindByEmailAsync(customUser.Email); ;

            checkAppUser.Wait();

            if (checkAppUser.Result == null)
            {
                Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(customUser, userPwd);

                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(customUser, roleName);
                    newUserRole.Wait();
                }
            }
        }
    }
}