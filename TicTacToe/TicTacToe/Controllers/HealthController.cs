using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : BaseController
    {
        [HttpGet]
        public IActionResult Healt()
        {
            return Ok();
        }
    }
}
