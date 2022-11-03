using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TraineeTrackerApp.Models;
using NuGet.Packaging;
using TraineeTrackerApp.Utilities;

namespace TraineeTrackerApp.Data
{
    public class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<Spartan> userManager = serviceProvider.GetService<UserManager<Spartan>>()!;
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>()!;

            context.Database.EnsureCreated();

            if (!roleManager.RoleExistsAsync(TheRoles.Role_Trainee).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(TheRoles.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(TheRoles.Role_Trainer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(TheRoles.Role_Trainee)).GetAwaiter().GetResult();
            }

            if (context.Courses.Any()) return;

            Course[] courses = new Course[]
            {
                new Course { Title = "C# Developer" },
                new Course { Title = "C# SDET" },
                new Course { Title = "Java Developer" },
                new Course { Title = "Java SDET" },
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();

            if (context.Spartans.Any() || context.Weeks.Any()) return;

            var mark = new Spartan
            {
                UserName = "mark@spartaglobal.com",
                Email = "mark@spartaglobal.com",
                EmailConfirmed = true,
                FirstName = "Mark",
                LastName = "Pollard",
                StartDate = new DateTime(2022, 9, 12),
                Course = new List<Course> { courses[0] }
            };
            var syed = new Spartan
            {
                UserName = "syed@spartaglobal.com",
                Email = "syed@spartaglobal.com",
                EmailConfirmed = true,
                FirstName = "Syed",
                LastName = "Ahmed",
                StartDate = new DateTime(2022, 9, 12),
                Course = new List<Course> { courses[0] }
            };
            var michael = new Spartan
            {
                UserName = "michael@spartaglobal.com",
                Email = "micheal@spartaglobal.com",
                EmailConfirmed = true,
                FirstName = "Michael",
                LastName = "Davies",
                StartDate = new DateTime(2022, 9, 12),
                Course = new List<Course> { courses[0] }
            };

            userManager.CreateAsync(mark, "Password1!").GetAwaiter().GetResult();
            userManager.CreateAsync(syed, "Password1!").GetAwaiter().GetResult();
            userManager.CreateAsync(michael, "Password1!").GetAwaiter().GetResult();

            userManager.AddToRoleAsync(mark, TheRoles.Role_Trainee).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(syed, TheRoles.Role_Trainee).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(michael, TheRoles.Role_Trainee).GetAwaiter().GetResult();

            var admin = new Spartan
            {
                UserName = "admin@spartaglobal.com",
                Email = "admin@spartaglobal.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Admin"
            };

            userManager.CreateAsync(admin, "Admin1!").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(admin, TheRoles.Role_Admin).GetAwaiter().GetResult();

            Week[] weeks = new Week[]
            {
                new Week
                {
                    Start = "Revising for tests",
                    Stop = "Being lazy",
                    Continue = "Practicing Agile",
                    WeekStart = new DateTime(2022,9,12),
                    TechnicalSkill = Proficiency.PartiallySkilled,
                    ConsultantSkill = Proficiency.LowSkilled,
                    Spartan = mark,
                    SpartanId = mark.Id
                },
                new Week
                {
                    Start = "Doing Homework",
                    Stop = "Being lazy",
                    Continue = "Practicing Unit tests",
                    WeekStart = new DateTime(2022,9,13),
                    TechnicalSkill = Proficiency.PartiallySkilled,
                    ConsultantSkill = Proficiency.LowSkilled,
                    Spartan = mark,
                    SpartanId = mark.Id
                },
                new Week
                {
                    Start = "Doing Homework",
                    Stop = "Being lazy",
                    Continue = "Being a sick guy",
                    WeekStart = new DateTime(2022,9,14),
                    TechnicalSkill = Proficiency.PartiallySkilled,
                    ConsultantSkill = Proficiency.LowSkilled,
                    Spartan = mark,
                    SpartanId = mark.Id
                },
                new Week
                {
                    Start = "Doing Homework",
                    Stop = "Getting distracted by my cat",
                    Continue = "Practicing API's",
                    WeekStart = new DateTime(2022,9,12),
                    TechnicalSkill = Proficiency.Skilled,
                    ConsultantSkill = Proficiency.PartiallySkilled,
                    Spartan = syed,
                    SpartanId = syed.Id
                },
                new Week
                {
                    Start = "Playing Bobble League",
                    Stop = "Getting distracted by my cat",
                    Continue = "Making awesome ReadMe's",
                    WeekStart = new DateTime(2022,9,13),
                    TechnicalSkill = Proficiency.Skilled,
                    ConsultantSkill = Proficiency.Skilled,
                    Spartan = syed,
                    SpartanId = syed.Id
                },
                new Week
                {
                    Start = "Making more memes",
                    Stop = "Playing Fifa",
                    Continue = "Practicing API's",
                    WeekStart = new DateTime(2022,9,12),
                    TechnicalSkill = Proficiency.Skilled,
                    ConsultantSkill = Proficiency.PartiallySkilled,
                    Spartan = michael,
                    SpartanId = michael.Id
                },
                new Week
                {
                    Start = "Learning stuff",
                    Stop = "Having Nazis in my background",
                    Continue = "Practicing MVC",
                    WeekStart = new DateTime(2022,9,13),
                    TechnicalSkill = Proficiency.Skilled,
                    ConsultantSkill = Proficiency.Skilled,
                    Spartan = michael,
                    SpartanId = michael.Id
                },
            };

            context.Weeks.AddRange(weeks);
            context.SaveChanges();

        }
    }
}
