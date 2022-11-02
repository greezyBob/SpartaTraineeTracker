//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TraineeTrackerApp.Data;
//using TraineeTrackerApp.Models;
//using TraineeTrackerApp.Services;

//namespace TrackerTests;

//internal class TraineeServiceTests
//{
//    private TraineeService _sut;
//    private ApplicationDbContext _context;

//    [OneTimeSetUp]
//    public void OneTimeSetup()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            //The key here is .UseInMemoryDatabase to make a
//            //use a memory database with our northwind context 
//            //schema
//            .UseInMemoryDatabase(databaseName: "Test_TraineeService_DB")
//            .Options;

//        //this calls the second constructor of NorthwindContext
//        _context = new ApplicationDbContext(options);

//        //this also calls the second constructor
//        //of our service layer
//        _sut = new TraineeService(_context);

//        //seed the db
//        //dont use SUT to seed IM_DB
//        _context.Spartans.Add(new Spartan
//        {
//            FirstName = "Gary",
//            LastName = "Pints",
//            StartDate = DateTime.Today
//        });

//        _context.Spartans.Add(new Spartan
//        {
//            FirstName = "Jeff",
//            LastName = "Salsiccia",
//            StartDate = DateTime.Today
//        });

//        _context.Spartans.Add(new Spartan
//        {
//            FirstName = "Armando",
//            LastName = "Bollard",
//            StartDate = DateTime.Today
//        });

//        _context.SaveChanges();
//    }

//    #region GetSpartansAsync()
//    [Test]
//    public void GetSpartansAsync_ReturnsTypeOfWeekList()
//    {
//        // Act
//        var result = _sut.GetSpartansAsync().Result;

//        // Assert
//        Assert.That(result, Is.TypeOf<List<Spartan>>());
//    }

//    [Test]
//    public void GetSpartansAsync_ReturnsCorrectLength()
//    {
//        // Arrange
//        int listCount = 3;

//        // Act
//        var result = _sut.GetSpartansAsync().Result;

//        // Assert
//        Assert.That(result.Count(), Is.EqualTo(listCount));
//    }
//    #endregion

//    #region GetSpartanByIdAsync(int)
//    [Test]
//    public void GetSpartanByIdAsync_ReturnsTypeOfWeek()
//    {
//        // Act
//        var result = _sut.GetSpartanByIdAsync("hello").Result;

//        // Assert
//        Assert.That(result, Is.TypeOf<Spartan>());
//    }

//    //[Test]
//    //public void GetSpartanByIdAsync_ReturnsCorrectWeek()
//    //{
//    //    // Act
//    //    var result = _sut.GetWeekByIdAsync(1).Result;

//    //    // Assert
//    //    Assert.That(result.Start, Is.EqualTo("Eating carrots"));
//    //    Assert.That(result.Stop, Is.EqualTo("Eating crisps"));
//    //    Assert.That(result.Continue, Is.EqualTo("Drinking Water"));
//    //}

//    //[Test]
//    //public void GetSpartanByIdAsync_ReturnsNull()
//    //{
//    //    // Act
//    //    var result = _sut.GetWeekByIdAsync(4).Result;

//    //    // Assert
//    //    Assert.That(result, Is.EqualTo(null));
//    //}
//    #endregion

//}
