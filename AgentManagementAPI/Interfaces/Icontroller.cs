using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentManagementAPI.Interfaces
{
    public interface Icontroller
    {
        Task<IActionResult> CreateEntity();
        Task<ActionResult<IEnumerable<Agent>>> GetAllEntities();

        Task<IActionResult> StartingPosition();

        Task<IActionResult> DirectionPosition();




    }
}
