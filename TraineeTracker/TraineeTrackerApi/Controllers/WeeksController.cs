using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;
using TraineeTrackerApi.Data;
using TraineeTrackerApi.Data.Utils;
using TraineeTrackerApi.Data.DTO;

namespace TraineeTrackerApi.Controllers
{
    [Route("api/[controller]")]
    public class WeeksController : Controller
    {
        private readonly IWeekService _service;
        private UserManager<Spartan> _userManager;

        public WeeksController(IWeekService service, UserManager<Spartan> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        // Roles checking to requests.

        [HttpGet]
        public async Task<ActionResult> GetWeeksForSpartan()
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());
            var user = await _userManager.FindByIdAsync(accessToken);
            var role = await _userManager.GetRolesAsync(user);

            List<Week> weeks = new List<Week>();

            if (role.Contains("Trainer"))
            {
                weeks = _service.GetWeeksAsync().Result
                    .Where(w => w.SpartanId == accessToken)
                    .OrderBy(w => w.SpartanId)
                    .ThenBy(w => w.WeekStart)
                    .ToList();
            }
            else
            {
                weeks = _service.GetWeeksAsync().Result
                    .Where(w => w.SpartanId == accessToken)
                    .OrderBy(w => w.SpartanId)
                    .ThenBy(w => w.WeekStart)
                    .ToList();
            }

            List<WeekDTO> dtoList = new List<WeekDTO>();
            weeks.ForEach(w => dtoList.Add(DTOUtils.WeekToDto(w)));

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = dtoList
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWeekByIdForSpartan(int? id)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());
            var user = await _userManager.FindByIdAsync(accessToken);
            var role = await _userManager.GetRolesAsync(user);

            bool isTrainer = role.Contains("Trainer");

            Week week = _service.GetWeeksAsync().Result
                .Where(w => w.Id == id)
                .FirstOrDefault();

            if (week == null) return NotFound(ResponseBuilder.GetResponse_Error_NotFound());
            if (week.SpartanId != accessToken && !isTrainer) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = DTOUtils.WeekToDto(week)
            });
        }
    }
}
