using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TraineeTrackerApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITraineeService _traineeService;

    public HomeController(ILogger<HomeController> logger, ITraineeService traineeService)
    {
        _logger = logger;
        _traineeService = traineeService;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("Admin"))
        {
            var spartans = await _traineeService.GetSpartansAsync();
            return View("~/Views/Admin/AdminIndex.cshtml", spartans);
        }
        else if (User.IsInRole("Trainee"))
        {
            return RedirectToAction("Index", "Weeks");
        }
        else if (User.IsInRole("Trainer"))
        {
            return RedirectToAction("Index", "Trainees");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}