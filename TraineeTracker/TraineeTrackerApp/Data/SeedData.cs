using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Data
{
    public class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            //ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            //UserManager<Spartan> userManager = serviceProvider.GetService<UserManager<Spartan>>()!;
            //RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);



            //context.Database.EnsureCreated();



            //if (context.Spartans.Any() || context.Weeks.Any()) return;



            //var trainer = new IdentityRole { Name = "Trainer", NormalizedName = "Trainer" };
            //var trainee = new IdentityRole { Name = "Trainee", NormalizedName = "Trainee" };



            //roleStore.CreateAsync(trainer).GetAwaiter().GetResult();
            //roleStore.CreateAsync(trainee).GetAwaiter().GetResult();



            //var phil = new Spartan { UserName = "Phil@SpartaGlobal.com", Email = "Phil@SpartaGlobal.com", EmailConfirmed = true };
            //var peter = new Spartan { UserName = "Peter@SpartaGlobal.com", Email = "Peter@SpartaGlobal.com", EmailConfirmed = true };
            //var nish = new Spartan { UserName = "Nish@SpartaGlobal.com", Email = "Nish@SpartaGlobal.com", EmailConfirmed = true };



            //userManager.CreateAsync(phil, "Password1!").GetAwaiter().GetResult();
            //userManager.CreateAsync(peter, "Password1!").GetAwaiter().GetResult();
            //userManager.CreateAsync(nish, "Password1!").GetAwaiter().GetResult();




            //IdentityUserRole<string>[] userRoles = new IdentityUserRole<string>[]
            //{
            //    new IdentityUserRole<string>
            //    {
            //        UserId = userManager.GetUserIdAsync(phil).GetAwaiter().GetResult(),
            //        RoleId = roleStore.GetRoleIdAsync(trainee).GetAwaiter().GetResult()
            //    },
            //    new IdentityUserRole<string>
            //    {
            //        UserId = userManager.GetUserIdAsync(peter).GetAwaiter().GetResult(),
            //        RoleId = roleStore.GetRoleIdAsync(trainee).GetAwaiter().GetResult()
            //    },
            //    new IdentityUserRole<string>
            //    {
            //        UserId = userManager.GetUserIdAsync(nish).GetAwaiter().GetResult(),
            //        RoleId = roleStore.GetRoleIdAsync(trainer).GetAwaiter().GetResult()
            //    }
            //};



            //context.UserRoles.AddRange(userRoles);
            //context.SaveChanges();



            //ToDo[] toDos = new ToDo[]
            // {
            //    new ToDo
            //    {
            //        Title = "Teach C#",
            //        Description = "Teach Engineering-2022 how to use Entity Framework",
            //        Complete = true,
            //        DateCreated = new DateTime(2022, 03, 01),
            //        Spartan = peter
            //    },
            //    new ToDo
            //    {
            //        Title = "Learn two swim",
            //        Description = "Dive off from the Cliffs of Dover and swim until I get to France",
            //        Complete = false,
            //        Spartan = peter
            //    },
            //    new ToDo
            //    {
            //        Title = "Reunite Oasis",
            //        Description = "Need to arrange chat with Noel and Liam to sort out a reunion tour",
            //        Complete = false,
            //        Spartan = phil
            //    }
            // };



            //context.ToDos.AddRange(toDos);
            //context.SaveChanges();
        }
    }
}
