using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Spartan> Spartans { get; set; }
    public DbSet<Week> Weeks { get; set; }
    public DbSet<Course> Courses { get; set; }
}