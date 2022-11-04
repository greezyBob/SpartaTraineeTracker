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

        [HttpGet]
        public async Task<ActionResult> GetWeeksForSpartan()
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            List<Week> weeks = new List<Week>();

            if (role.Contains("Trainer"))
            {
                weeks = _service.GetWeeksAsync().Result
                    .OrderBy(w => w.SpartanId)
                    .ThenBy(w => w.WeekStart)
                    .ToList();
            }
            else if (role.Contains("Trainee"))
            {
                weeks = _service.GetWeeksAsync().Result
                    .Where(w => w.SpartanId == accessToken)
                    .OrderBy(w => w.SpartanId)
                    .ThenBy(w => w.WeekStart)
                    .ToList();
            }
            else
            {
                return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());
            }

            List<WeekDTO> dtoList = new List<WeekDTO>();
            weeks.ForEach(w => dtoList.Add(DTOUtils.WeekToDto(w)));

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = dtoList
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateWeekForSpartan(WeekDTO week)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            if (role.Contains("Trainer") || role.Contains("Admin"))
                return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess()); // Change response to include details to go to different enpoint if trainer.

            var canParseWeekStart = DateTime.TryParse(week.WeekStart, out var parsedDate);
            if (week.WeekStart != null && !canParseWeekStart)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.WeekStart, typeof(DateTime).ToString()));

            var canParseTSkill = Enum.TryParse<Proficiency>(week.TechnicalSkill, out var parsedTSkill);
            if (week.TechnicalSkill != null && !canParseTSkill)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.TechnicalSkill, typeof(Proficiency).ToString()));

            var canParseCSkill = Enum.TryParse<Proficiency>(week.ConsultantSkill, out var parsedCSkill);
            if (week.ConsultantSkill != null && !canParseCSkill)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.ConsultantSkill, typeof(Proficiency).ToString()));

            Week createdWeek = new Week
            {
                Start = week.Start,
                Stop = week.Stop,
                Continue = week.Continue,
                WeekStart = week.WeekStart != null ? parsedDate : DateTime.Now,
                TechnicalSkill = week.TechnicalSkill != null ? parsedTSkill : Proficiency.Unskilled,
                ConsultantSkill = week.ConsultantSkill != null ? parsedCSkill : Proficiency.Unskilled,
                GitHubLink = week.GitHubLink,
                Spartan = user,
                SpartanId = accessToken
            };

            await _service.AddWeek(createdWeek); // Add try catch here

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = DTOUtils.WeekToDto(createdWeek)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWeekByIdForSpartan(int? id)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            Week? week = _service.GetWeeksAsync().Result
                .Where(w => w.Id == id)
                .FirstOrDefault();

            if (week == null) return NotFound(ResponseBuilder.GetResponse_Error_NotFound());
            if (week.SpartanId != accessToken && !role.Contains("Trainer")) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = DTOUtils.WeekToDto(week)
            });
        }


        // Trainer can only update the comments field, trainee can only update their own weeks. Apply different dto to show spartan first and last name only for trainers.
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWeekByIdForSpartan(int id, WeekDTO week)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            Week? weekToUpdate = await _service.GetWeekByIdAsync(id);

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = DTOUtils.WeekToDto(weekToUpdate)
            });
        }
    }
}
