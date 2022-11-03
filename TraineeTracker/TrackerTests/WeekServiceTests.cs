using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TrackerTests;

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
            .UseInMemoryDatabase(databaseName: "Test_DB")
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

    #region GetWeeksAsync()
    [Test]
    public void GetWeeksAsync_ReturnsTypeOfWeekList()
    {
        // Act
        var result = _sut.GetWeeksAsync().Result;

        // Assert
        Assert.That(result, Is.TypeOf<List<Week>>());
    }

    [Test]
    public void GetWeeksAsync_ReturnsCorrectLength()
    {
        // Arrange
        int listCount = 3;

        // Act
        var result = _sut.GetWeeksAsync().Result;

        // Assert
        Assert.That(result.Count(), Is.EqualTo(listCount));
    }
    #endregion

    #region GetWeekByIdAsync(int)
    [Test]
    public void GetWeekByIdAsync_ReturnsTypeOfWeek()
    {
        // Act
        var result = _sut.GetWeekByIdAsync(1).Result;

        // Assert
        Assert.That(result, Is.TypeOf<Week>());
    }

    [Test]
    public void GetWeekByIdAsync_ReturnsCorrectWeek()
    {
        // Act
        var result = _sut.GetWeekByIdAsync(1).Result;

        // Assert
        Assert.That(result.Start, Is.EqualTo("Eating carrots"));
        Assert.That(result.Stop, Is.EqualTo("Eating crisps"));
        Assert.That(result.Continue, Is.EqualTo("Drinking Water"));
    }

    [Test]
    public void GetWeekByIdAsync_ReturnsNull()
    {
        // Act
        var result = _sut.GetWeekByIdAsync(4).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
    #endregion

    #region GetSpartansAsync()
    [Test]
    public void GetSpartansAsync_ReturnsTypeOfSpartanList()
    {
        // Act
        var result = _sut.GetSpartansAsync().Result;

        // Assert
        Assert.That(result, Is.TypeOf<List<Spartan>>());
    }

    [Test]
    public void GetSpartansAsync_ReturnsCorrectLength()
    {
        // Arrange
        int listCount = 3;

        // Act
        var result = _sut.GetSpartansAsync().Result;

        // Assert
        Assert.That(result.Count(), Is.EqualTo(listCount));
    }
    #endregion

    #region GetSpartansByIdAsync(string)
    [Ignore("Service layer needs updating")]
    [Test]
    public void GetSpartansByIdAsync_ReturnsTypeOfWeek()
    {
        // Act
        var result = _sut.GetSpartansByIdAsync(1).Result;

        // Assert
        Assert.That(result, Is.TypeOf<Week>());
    }

    [Ignore("Service layer needs updating")]
    [Test]
    public void GetSpartansByIdAsync_ReturnsCorrectWeek()
    {
        // Act
        var result = _sut.GetWeekByIdAsync(1).Result;

        // Assert
        Assert.That(result.Start, Is.EqualTo("Eating carrots"));
        Assert.That(result.Stop, Is.EqualTo("Eating crisps"));
        Assert.That(result.Continue, Is.EqualTo("Drinking Water"));
    }

    [Ignore("Service layer needs updating")]
    [Test]
    public void GetSpartansByIdAsync_ReturnsNull()
    {
        // Act
        var result = _sut.GetSpartansByIdAsync(4).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
    #endregion

    #region AddWeek(Week)
    [Test]
    public async Task AddWeek_GivenValidWeek_NumberOfWeekIncreases()
    {
        // Arrange
        var initialCount = _context.Weeks.Count();

        // Act
        var spartanObj = new Spartan
        {
            FirstName = "Gary",
            LastName = "Pints",
            StartDate = DateTime.Today
        };
        var weekObj = new Week
        {
            Start = "Eating carrots",
            Stop = "Eating crisps",
            Continue = "Drinking Water",
            Spartan = spartanObj
        };
        await _sut.AddWeek(weekObj);
        var updatedCount = _context.Weeks.Count();

        // Assert
        Assert.That(updatedCount, Is.EqualTo(initialCount + 1));

        // CleanUp
        var weekToRemove = _context.Weeks.Find(weekObj.Id);
        _context.Weeks.Remove(weekToRemove);
        _context.Spartans.Remove(spartanObj);
        _context.SaveChanges();
    }

    [Test]
    public async Task AddWeek_GivenValidWeek_WeekIsAdded()
    {
        // Arrange

        // Act
        var spartanObj = new Spartan
        {
            FirstName = "Gary",
            LastName = "Pints",
            StartDate = DateTime.Today
        };
        var weekObj = new Week
        {
            Start = "Eating carrots",
            Stop = "Eating crisps",
            Continue = "Drinking Water",
            Spartan = spartanObj
        };
        _sut.AddWeek(weekObj).Wait();
        var result = _context.Weeks.Find(weekObj.Id);

        // Assert
        Assert.That(result.Start, Is.EqualTo(weekObj.Start));
        Assert.That(result.Stop, Is.EqualTo(weekObj.Stop));
        Assert.That(result.Continue, Is.EqualTo(weekObj.Continue));

        // CleanUp
        var weekToRemove = _context.Weeks.Find(weekObj.Id);
        _context.Weeks.Remove(weekToRemove);
        _context.Spartans.Remove(spartanObj);
        _context.SaveChanges();
    }

    // Throws exception despite adding to database.
    [Ignore("Slightly broken")]
    [Test]
    public void AddWeek_GivenInvalidWeek_ThrowsException()
    {
        // Arrange

        // Act
        var spartanObj = new Spartan
        {
            FirstName = "Gary",
            LastName = "Pints",
            StartDate = DateTime.Today
        };
        var weekObj = new Week
        {
            Start = "Eating carrots",
            Spartan = spartanObj
        };

        // Assert
        Assert.That(async () => await _sut.AddWeek(weekObj), Throws.Exception.TypeOf<DbUpdateException>());

        // CleanUp
        var weekToRemove = _context.Weeks.Find(weekObj.Id);
        _context.Weeks.Remove(weekToRemove);
        _context.Spartans.Remove(spartanObj);
        _context.SaveChanges();
    }
    #endregion

    #region RemoveWeekAsync(Week)
    [Test]
    public async Task RemoveWeek_GivenValidWeek_NumberOfWeekDecreases()
    {
        // Arrange
        var spartanObj = new Spartan
        {
            FirstName = "Gary",
            LastName = "Pints",
            StartDate = DateTime.Today
        };
        var weekObj = new Week
        {
            Start = "Eating carrots",
            Stop = "Eating crisps",
            Continue = "Drinking Water",
            Spartan = spartanObj
        };
        _context.Weeks.Add(weekObj);
        await _context.SaveChangesAsync();
        var initialCount = _context.Weeks.Count();

        // Act
        var weekToRemove = _context.Weeks.Where(w => w.Id == weekObj.Id).FirstOrDefault();
        await _sut.RemoveWeekAsync(weekToRemove);
        var updatedCount = _context.Weeks.Count();

        // Assert
        Assert.That(updatedCount, Is.EqualTo(initialCount - 1));

        // CleanUp
        var weekObjCreated = _context.Weeks.Where(w => w.Id == weekObj.Id).FirstOrDefault();
        var spartanObjCreated = _context.Spartans.Where(s => s.Id == spartanObj.Id).FirstOrDefault();

        if (weekObjCreated != null)
        {
            _context.Weeks.Remove(weekObjCreated);
        }

        if (spartanObjCreated != null)
        {
            _context.Spartans.Remove(spartanObjCreated);
        }

        _context.SaveChanges();
    }

    [Test]
    public async Task RemoveWeek_GivenValidWeek_WeekIsRemoved()
    {
        // Arrange
        var spartanObj1 = new Spartan
        {
            FirstName = "Steven",
            LastName = "Pints",
            StartDate = DateTime.Today
        };
        var weekObj = new Week
        {
            Start = "Eating carrots",
            Stop = "Eating crisps",
            Continue = "Drinking Water",
            Spartan = spartanObj1
        };
        _context.Weeks.Add(weekObj);
        _context.SaveChanges();

        // Act
        var weekToRemove = _context.Weeks.Find(weekObj.Id);
        _sut.RemoveWeekAsync(weekToRemove).Wait();
        var result = _context.Weeks.Find(weekObj.Id);

        // Assert
        Assert.That(result, Is.EqualTo(null));

        // CleanUp
        if (_context.Spartans.Where(s => s.Id == spartanObj1.Id).FirstOrDefault() != null)
        {
            _context.Spartans.Remove(spartanObj1);
            _context.SaveChanges();
        }
    }

    [Ignore("Breaks other tests")]
    [Test]
    public void RemoveWeek_GivenInvalidWeek_ThrowsException()
    {
        // Arrange
        var weekObj = new Week
        {
            Start = "Eating carrots"
        };

        // Act


        // Assert
        Assert.That(async () => await _sut.RemoveWeekAsync(weekObj), Throws.Exception.TypeOf<DbUpdateConcurrencyException>());
    }
    #endregion
}