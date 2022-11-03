using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraineeTrackerApp.Models;
using Moq;
using TraineeTrackerApp.Services;
using TraineeTrackerApp.Controllers;
using System.Security.Claims;

namespace TrackerTests;

internal class WeeksControllerTests
{
    // Note add tests to check logged in user privileges

    #region Index()
    //public async Task<IActionResult> Index()
    //{
    //    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
    //    //var applicationDbContext = _service.Weeks.Include(w => w.Spartan);
    //    //return View(await applicationDbContext.ToListAsync());
    //    var weeks = await _service.GetWeeksAsync();
    //    var filteredWeeks = weeks.Where(w => w.SpartanId == currentUser.Id)
    //        .OrderBy(w => w.WeekStart.Date).ToList();
    //    return View(filteredWeeks);
    //}

    [Test]
    public async Task Index_ReturnsTypeOfListWeeks()
    {
        // Arrange
        var serviceMock = new Mock<IWeekService>();
        serviceMock.Setup(mock => mock.GetWeeksAsync()).ReturnsAsync(new List<Week>());

        var store = new Mock<IUserStore<Spartan>>();
        var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
        userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

        var sut = new WeeksController(serviceMock.Object, userMgrMock.Object);
        sut.ControllerContext = new ControllerContext
         {
             HttpContext = new DefaultHttpContext()
         };

        // Act
        var result = await sut.Index();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
    }

    [Test]
    public void Index_GivenUserHasWeeks_ReturnsListOfUsersWeeks()
    {
        // Arrange
        Spartan user = new Spartan
        {
            Id = Guid.NewGuid().ToString(),
        };

        List<Week> weeks = new List<Week>();
        weeks.Add(new Week{ SpartanId = user.Id });
        weeks.Add(new Week{ SpartanId = user.Id });
        weeks.Add(new Week{ SpartanId = user.Id });
        weeks.Add(new Week{ SpartanId = Guid.NewGuid().ToString() });
        weeks.Add(new Week{ SpartanId = Guid.NewGuid().ToString() });

        var serviceMock = new Mock<IWeekService>();
        serviceMock.Setup(mock => mock.GetWeeksAsync()).ReturnsAsync(weeks);

        var store = new Mock<IUserStore<Spartan>>();
        var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
        userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        var sut = new WeeksController(serviceMock.Object, userMgrMock.Object);
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = sut.Index().Result;
        var viewResult = (ViewResult)result;
        var viewDataList = (List<Week>)viewResult.ViewData.Model;

        // Assert
        Assert.That(viewDataList.Count(), Is.EqualTo(3));
        Assert.That(viewDataList[0], Is.EqualTo(weeks[0]));
        Assert.That(viewDataList[1], Is.EqualTo(weeks[1]));
        Assert.That(viewDataList[2], Is.EqualTo(weeks[2]));
    }

    [Test]
    public void Index_GivenUserHasNoWeeks_ReturnsEmptyListOfWeeks()
    {
        // Arrange
        Spartan user = new Spartan
        {
            Id = Guid.NewGuid().ToString(),
        };

        List<Week> weeks = new List<Week>();
        weeks.Add(new Week { SpartanId = Guid.NewGuid().ToString() });
        weeks.Add(new Week { SpartanId = Guid.NewGuid().ToString() });

        var serviceMock = new Mock<IWeekService>();
        serviceMock.Setup(mock => mock.GetWeeksAsync()).ReturnsAsync(weeks);

        var store = new Mock<IUserStore<Spartan>>();
        var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
        userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        var sut = new WeeksController(serviceMock.Object, userMgrMock.Object);
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = sut.Index().Result;
        var viewResult = (ViewResult)result;
        var viewDataList = (List<Week>)viewResult.ViewData.Model;

        // Assert
        Assert.That(viewDataList.Count(), Is.EqualTo(0));
    }
    #endregion

    #region Details(int)
    //// GET: Weeks/Details/5
    //public async Task<IActionResult> Details(int? id)
    //{
    //    if (id == null || _service.GetWeeksAsync().Result == new List<Week>())
    //    {
    //        return NotFound();
    //    }

    //    //var week = await _service.Weeks
    //    //    .Include(w => w.Spartan)
    //    //    .FirstOrDefaultAsync(m => m.Id == id);
    //    var week = await _service.GetWeekByIdAsync(id);
    //    if (week == null)
    //    {
    //        return NotFound();
    //    }

    //    var currentUser = await _userManager.GetUserAsync(HttpContext.User);

    //    if (week.SpartanId != currentUser.Id)
    //    {
    //        return Unauthorized();
    //    }

    //    return View(week);
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Details_ReturnsTypeOfWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Details_GivenValidWeekId_ReturnsWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Details_GivenInvalidWeekId_Returns404NotFound()
    {
        Assert.That(true);
    }
    #endregion

    #region Create()
    //// GET: Weeks/Create
    //public IActionResult Create()
    //{
    ////   ViewData["SpartanId"] = new SelectList(_service.GetSpartansAsync().Result, "Id", "Id");
    //    return View();
    //}
    #endregion

    #region Create(Week)
    //// POST: Weeks/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill")] Week week)
    //{
    //    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
    //    week.SpartanId = currentUser.Id;
    //    week.Spartan = currentUser;

    //    //var newWeek = new Week
    //    //{
    //    //    Start = week.Start,
    //    //    Stop = week.Stop,
    //    //    Continue = week.Continue,
    //    //    Spartan = currentUser
    //    //};

    //    if (week.SpartanId != null)
    //    {
    //        await _service.AddWeek(week);
    //        return RedirectToAction(nameof(Index));
    //    }

    ////    ViewData["SpartanId"] = new SelectList(_service.GetSpartansAsync().Result, "Id", "Id", week.SpartanId);
    //    return View(week);
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Create_ReturnsTypeOfWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Create_GivenWeek_ReturnsWeek()
    {
        Assert.That(true);
    }
    #endregion

    #region Edit(int)
    //// GET: Weeks/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null || _service.GetWeeksAsync().Result == new List<Week>())
    //    {
    //        return NotFound();
    //    }

    //    var week = await _service.GetWeekByIdAsync(id);
    //    if (week == null)
    //    {
    //        return NotFound();
    //    }

    //    var currentUser = await _userManager.GetUserAsync(HttpContext.User);

    //    if (week.SpartanId != currentUser.Id)
    //    {
    //        return Unauthorized();
    //    }

    ////    ViewData["SpartanId"] = new SelectList(_service.GetSpartansAsync().Result, "Id", "Id", week.SpartanId);
    //    return View(week);
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Edit_ReturnsTypeOfWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Edit_GivenValidId_ReturnsWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Edit_GivenInvalidId_Returns404()
    {
        Assert.That(true);
    }
    #endregion

    #region Edit(int, Week)
    //// POST: Weeks/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill")] Week week)
    //{
    //    if (id != week.Id)
    //    {
    //        return NotFound();
    //    }

    //    try
    //    {
    //        var weekToUpdate = _service.GetWeekByIdAsync(id).Result;
    //        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
    //        if (weekToUpdate.SpartanId != currentUser.Id) return Unauthorized(); ;
    //        weekToUpdate.WeekStart = week.WeekStart;
    //        weekToUpdate.Start = week.Start ?? weekToUpdate.Start;
    //        weekToUpdate.Stop = week.Stop ?? weekToUpdate.Stop;
    //        weekToUpdate.Continue = week.Continue ?? weekToUpdate.Continue;
    //        weekToUpdate.GitHubLink = week.GitHubLink ?? weekToUpdate.GitHubLink;
    //        weekToUpdate.TechnicalSkill = week.TechnicalSkill;
    //        weekToUpdate.ConsultantSkill = week.ConsultantSkill;

    //        await _service.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!WeekExists(week.Id))
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            throw;
    //        }
    //    }
    //    return RedirectToAction(nameof(Index));

    //    ViewData["SpartanId"] = new SelectList(_service.GetSpartansAsync().Result, "Id", "Id", week.SpartanId);
    //    return View(week);
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Edit_Given_ReturnsTypeOfWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Edit_GivenValidIdAndValidWeek_ReturnsWeek()
    {
        Assert.That(true);
    }
    #endregion

    #region Delete(int)
    //// GET: Weeks/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    //{
    //    if (id == null || _service.GetWeeksAsync().Result == new List<Week>())
    //    {
    //        return NotFound();
    //    }

    //    var week = await _service.GetWeekByIdAsync(id);
    //    if (week == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(week);
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_ReturnsTypeOfWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_GivenValidId_ReturnsWeek()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_GivenInvalidId_Return404()
    {
        Assert.That(true);
    }
    #endregion

    #region DeleteConfirmed(int)
    //// POST: Weeks/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    if (_service.GetWeeksAsync().Result == new List<Week>())
    //    {
    //        return Problem("Entity set 'ApplicationDbContext.Weeks' is empty.");
    //    }

    //    var week = await _service.GetWeekByIdAsync(id);
    //    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
    //    if (week.SpartanId != currentUser.Id)
    //    {
    //        return Unauthorized();
    //    }
    //    if (week != null)
    //    {
    //        await _service.RemoveWeekAsync(week);
    //    }

    //    return RedirectToAction(nameof(Index));
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_ReturnsTypeOfViewResultIndex()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_GivenValidId_ReturnsIndexView()
    {
        Assert.That(true);
    }

    [Ignore("NotImplemented")]
    [Test]
    public void Delete_GivenInvalidId_Returns404()
    {
        Assert.That(true);
    }
    #endregion

    #region WeekExists(int)
    //private bool WeekExists(int id)
    //{
    //    return _service.GetWeekByIdAsync(id).Result != null;
    //}

    [Ignore("NotImplemented")]
    [Test]
    public void WeekExists_ReturnsTypeOfBool()
    {
        Assert.That(true);
    }
    #endregion
}