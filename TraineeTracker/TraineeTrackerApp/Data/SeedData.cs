using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TraineeTrackerApp.Models;
using NuGet.Packaging;

namespace TraineeTrackerApp.Data
{
    public class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<Spartan> userManager = serviceProvider.GetService<UserManager<Spartan>>()!;
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);

            context.Database.EnsureCreated();

            if (context.Spartans.Any() || context.Weeks.Any()) return;

            //var trainer = new IdentityRole { Name = "Trainer", NormalizedName = "Trainer" };
            var spartan = new IdentityRole { Name = "Spartan", NormalizedName = "Spartan" };

            //roleStore.CreateAsync(trainer).GetAwaiter().GetResult();
            roleStore.CreateAsync(spartan).GetAwaiter().GetResult();

            var mark = new Spartan { 
                UserName = "Mark@SpartaGlobal.com", 
                Email = "Mark@SpartaGlobal.com", 
                EmailConfirmed = true, 
                FirstName = "Mark", 
                LastName = "Pollard",
                StartDate = new DateTime(2022, 9, 12)
            };
            var syed = new Spartan {
                UserName = "Syed@SpartaGlobal.com",
                Email = "Syed@SpartaGlobal.com", 
                EmailConfirmed = true,
                FirstName = "Syed",
                LastName = "Ahmed",
                StartDate = new DateTime(2022, 9, 12)
            };
            var michael = new Spartan { 
                UserName = "Michael@SpartaGlobal.com", 
                Email = "Micheal@SpartaGlobal.com", 
                EmailConfirmed = true,
                FirstName = "Michael",
                LastName = "Davies",
                StartDate = new DateTime(2022, 9, 12)
            };

            userManager.CreateAsync(mark, "Password1!").GetAwaiter().GetResult();
            userManager.CreateAsync(syed, "Password1!").GetAwaiter().GetResult();
            userManager.CreateAsync(michael, "Password1!").GetAwaiter().GetResult();

            IdentityUserRole<string>[] userRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(mark).GetAwaiter().GetResult(),
                    RoleId = roleStore.GetRoleIdAsync(spartan).GetAwaiter().GetResult()
                },
                new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(syed).GetAwaiter().GetResult(),
                    RoleId = roleStore.GetRoleIdAsync(spartan).GetAwaiter().GetResult()
                },
                new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(michael).GetAwaiter().GetResult(),
                    RoleId = roleStore.GetRoleIdAsync(spartan).GetAwaiter().GetResult()
                }
            };

            context.UserRoles.AddRange(userRoles);
            context.SaveChanges();

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

            mark.Course = courses[0];
            mark.Weeks = new List<Week> { weeks[0], weeks[1], weeks[2] };

            syed.Course = courses[0];
            syed.Weeks = new List<Week> { weeks[3], weeks[4] };

            michael.Course = courses[0];
            syed.Weeks = new List<Week> { weeks[5], weeks[6] };

            context.Spartans.AddRange(mark, syed, michael);
            context.SaveChanges();
        }
    }
}
