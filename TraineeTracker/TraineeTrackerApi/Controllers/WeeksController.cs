using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;
using TraineeTrackerApi.Data;
using TraineeTrackerApi.Data.Utils;
using TraineeTrackerApi.Data.DTO;
using System.Collections.Generic;
using TraineeTrackerApi.Data.BindingObjects;

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
            if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

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
        public async Task<ActionResult> CreateWeekForSpartan([FromBody] WeekDTO week)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            if (role.Contains("Trainer") || role.Contains("Admin"))
                return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

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

            await _service.AddWeek(createdWeek);

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
            if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());
            if (role.Contains("Admin")) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWeekByIdForSpartan(int id,[FromBody] WeekBindingObject week)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());
            if (role.Contains("Trainer") || role.Contains("Admin"))
                return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var canParseWeekStart = DateTime.TryParse(week.WeekStart, out var parsedDate);
            if (week.WeekStart != null && !canParseWeekStart)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.WeekStart, typeof(DateTime).ToString()));

            var canParseTSkill = Enum.TryParse<Proficiency>(week.TechnicalSkill, out var parsedTSkill);
            if (week.TechnicalSkill != null && !canParseTSkill)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.TechnicalSkill, typeof(Proficiency).ToString()));

            var canParseCSkill = Enum.TryParse<Proficiency>(week.ConsultantSkill, out var parsedCSkill);
            if (week.ConsultantSkill != null && !canParseCSkill)
                return BadRequest(ResponseBuilder.GetResponse_Error_CannotParse(week.ConsultantSkill, typeof(Proficiency).ToString()));

            Week? weekToUpdate = await _service.GetWeekByIdAsync(id);
            if (weekToUpdate == null) return NotFound(ResponseBuilder.GetResponse_Error_NotFound());
            if (weekToUpdate.SpartanId != accessToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            weekToUpdate.Start = week.Start ?? weekToUpdate.Start;
            weekToUpdate.Stop = week.Stop ?? weekToUpdate.Stop;
            weekToUpdate.Continue = week.Continue ?? weekToUpdate.Continue;
            weekToUpdate.WeekStart = week.WeekStart != null ? parsedDate : weekToUpdate.WeekStart;
            weekToUpdate.TechnicalSkill = week.TechnicalSkill != null ? parsedTSkill : weekToUpdate.TechnicalSkill;
            weekToUpdate.ConsultantSkill = week.ConsultantSkill != null ? parsedCSkill : weekToUpdate.ConsultantSkill;
            weekToUpdate.GitHubLink = week.GitHubLink ?? weekToUpdate.GitHubLink;

            await _service.SaveChangesAsync();

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                data = DTOUtils.WeekToDto(weekToUpdate)
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWeekById(int id)
        {
            var headerHasToken = Request.Headers.TryGetValue("Access-Token", out var accessToken);
            if (!headerHasToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccessToken());

            var user = await _userManager.FindByIdAsync(accessToken);
            if (user == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var role = await _userManager.GetRolesAsync(user);
            if (role == null) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());
            if (role.Contains("Trainer") || role.Contains("Admin"))
                return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            var weekToDelete = await _service.GetWeekByIdAsync(id);
            if(weekToDelete == null) return NotFound(ResponseBuilder.GetResponse_Error_NotFound());
            if (weekToDelete.SpartanId != accessToken) return Unauthorized(ResponseBuilder.GetResponse_Error_NoAccess());

            await _service.RemoveWeekAsync(weekToDelete);

            return Ok(new
            {
                link = ResponseBuilder.GetResponseLink(Request),
                status = "success",
                message = "Week has been deleted"
            });
        }
    }
}
