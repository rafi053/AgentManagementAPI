using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        Task<IActionResult> CreateEntity(int jv) {  }
        Task<ActionResult<IEnumerable<Agent>>> GetAllEntities();

        Task<IActionResult> StartingPosition();

        Task<IActionResult> DirectionPosition();
    }
}
