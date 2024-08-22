using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Utils;
using AgentManagementAPI.Enums;

namespace AgentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        public DbContextAPI _dbContextAPI;
        public TargetsController(DbContextAPI dbContextAPI)
        {
            _dbContextAPI = dbContextAPI;
        }




        // יצירת מטרה
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTarget(Target target)
        {
            target.StatusTarget = StatusTarget.Live;
            _dbContextAPI.Targets.Add(target);
            await _dbContextAPI.SaveChangesAsync();

            return StatusCode(
           StatusCodes.Status201Created,
           new { targetId = target.Id });
        }


        // הצג את כל המטרות
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Target>>> GetAllTargets()
        {
            return await _dbContextAPI.Targets.ToListAsync();
        }


        // קביעת מיקום התחלתי
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> StartingPosition(int id, Target target, int x, int y)
        {
            target = await this._dbContextAPI.Targets.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (target == null) return StatusCode(status, HttpUtils.Response(status, "target not found"));
            if (target.StatusTarget == StatusTarget.Eliminated)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(
                    status,
                    new
                    {
                        success = false,
                        error = "Cannot Starting Position an target that hasalready Eliminated."
                    }
                );

            }

            target.LocationTarget.X = x;
            target.LocationTarget.Y = y;
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Position success." }
            );

        }

        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, Target target, string directionT)
        {
            target = await this._dbContextAPI.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<Direction>(directionT, true, out var direction))
            {
                return BadRequest("Invalid direction value.");
            }

            if (target.LocationTarget == null)
            {
                target.LocationTarget = new Location();
            }

            switch (direction)
            {
                case Direction.nw:
                    target.LocationTarget.X -= 1;
                    target.LocationTarget.Y += 1;
                    break;
                case Direction.n:
                    target.LocationTarget.Y += 1;
                    break;
                case Direction.ne:
                    target.LocationTarget.X += 1;
                    target.LocationTarget.Y += 1;
                    break;
                case Direction.w:
                    target.LocationTarget.X -= 1;
                    break;
                case Direction.e:
                    target.LocationTarget.X += 1;
                    break;
                case Direction.sw:
                    target.LocationTarget.X -= 1;
                    target.LocationTarget.Y -= 1;
                    break;
                case Direction.s:
                    target.LocationTarget.Y -= 1;
                    break;
                case Direction.se:
                    target.LocationTarget.X += 1;
                    target.LocationTarget.Y -= 1;
                    break;
            }

            _dbContextAPI.SaveChanges();
            return Ok(target);
        }

        private bool TargetExists(int id)
        {
            return _dbContextAPI.Targets.Any(e => e.Id == id);
        }
    }
}

   
