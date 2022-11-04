using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraineeTrackerApi.Data.DTO;
using TraineeTrackerApi.Data.Utils;
using TraineeTrackerApi.Data;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TraineeTrackerApi.Controllers;

[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly ITraineeService _service;
    private UserManager<Spartan> _userManager;

    public UsersController(ITraineeService service, UserManager<Spartan> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult> GetTrainees()
    {
        var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
        if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

        var user = await _userManager.FindByIdAsync(accessToken);
        if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

        var role = await _userManager.GetRolesAsync(user);
        if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());
        if (role.Contains("Trainee")) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

        var trainees = await _service.GetSpartansAsync();

        for (int i = 0; i < trainees.Count; i++)
        {
            trainees[i].Weeks = await _service.GetWeeksBySpartanIdAsync(trainees[i].Id);
        }

        List<SpartanDTO> spartanDTOs = new List<SpartanDTO>();
        foreach (var item in trainees)
        {
            var dto = DTOUtils.SpartanToDTO(item);
            dto.Roles = await _userManager.GetRolesAsync(item);

            if (role.Contains("Trainer") && !dto.Roles.Contains("Admin") && !dto.Roles.Contains("Trainer"))
            {
                spartanDTOs.Add(dto);
            }
            else if (role.Contains("Admin") && !dto.Roles.Contains("Admin"))
            {
                spartanDTOs.Add(dto);
            }
        }

        return Ok(new
        {
            link = ResponseBuilder.GetResponseLink(Request),
            data = spartanDTOs
        });
    }
}
