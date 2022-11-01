using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TrackerTests
{

    public class Tests
    {
        private WeekService _sut;
        private ApplicationDbContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                //The key here is .UseInMemoryDatabase to make a
                //use a memory database with our northwind context 
                //schema
                .UseInMemoryDatabase(databaseName: "Example_DB")
                .Options;

            //this calls the second constructor of NorthwindContext
            _context = new ApplicationDbContext(options);

            //this also calls the second constructor
            //of our service layer
            _sut = new WeekService(_context);

            //seed the db
            //dont use SUT to seed IM_DB
            _context.Weeks.Add(new Week
            {
                Start = "Eating carrots",
                Stop = "Eating crisps",
                Continue = "Drinking Water",
                Spartan = new Spartan
                {
                    FirstName = "Gary",
                    LastName = "Pints",
                    StartDate = DateTime.Today
                }

            });

            _context.Weeks.Add(new Week
            {
                Start = "Going gym",
                Stop = "Procrastinating",
                Continue = "Doing code katas",
                Spartan = new Spartan
                {
                    FirstName = "Jeff",
                    LastName = "Salsiccia",
                    StartDate = DateTime.Today
                }

            });

            _context.Weeks.Add(new Week
            {
                Start = "MOQ testing all projects",
                Stop = "Yelling at my wife and son",
                Continue = "Practising the harpsichord",
                Spartan = new Spartan
                {
                    FirstName = "Armando",
                    LastName = "Bollard",
                    StartDate = DateTime.Today
                }

            });

            _context.SaveChanges();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}