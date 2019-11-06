using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{

    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetCamps()
        {
            return Ok (new  { Moniker = "ATL3020", Name = "Atlanta Code Camp" });
        }
    }
}