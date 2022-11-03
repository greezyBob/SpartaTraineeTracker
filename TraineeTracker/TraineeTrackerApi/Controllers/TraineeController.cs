using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TraineeTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {


        private ActionResult AccessTokenNotProvided()
        {
            var returnObj = new
            {
                message = "Access-Token MUST be provided in request header."
            };
            return Unauthorized(returnObj);
        }
    }
}
