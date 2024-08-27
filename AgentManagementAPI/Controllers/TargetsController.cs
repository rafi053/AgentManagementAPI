using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Utils;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Services;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly DbContextAPI _dbContextAPI;
        private readonly ServiceTrget _serviceTrget;

        public TargetsController(DbContextAPI dbContextAPI, ServiceTrget serviceTrget)
        {
            _dbContextAPI = dbContextAPI;
            _serviceTrget = serviceTrget;
            
        }


        

        // יצירת מטרה
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEntity(Target target)
        {
            _dbContextAPI.Targets.Add(target);
            await _dbContextAPI.SaveChangesAsync();
            
            return StatusCode(
               StatusCodes.Status200OK,
               new { Id = target.Id });
        }

        // הצג את כל המטרות
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Target>>> GetAllEntities()
        {
            return await _dbContextAPI.Targets.ToListAsync();
        }

        // קבל את מספר המטרות
        [HttpGet("count")]
        public async Task<IActionResult> GetAllTargetsCount()
        {
            var targets = await _dbContextAPI.Targets.ToArrayAsync();

            return Ok(targets.Length + 1);
        }

        // קבל את כל המטרות שחוסלו
        [HttpGet("KilledCount")]
        public async Task<IActionResult> GetKilledTargets()
        {
            int i = 0;
            var targets = await _dbContextAPI.Targets.ToArrayAsync();
            foreach (var target in targets)
            {

                if (target.StatusTarget == StatusTarget.Eliminated.ToString())
                    i++;
            }
            return Ok(i);
        }


        // הצג מטרה
        [HttpGet("{id}")]
        public async Task<ActionResult<Target>> GetEntity(int id)
        {
            Target? target = await this._dbContextAPI.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            return target;
        }


        // קבלת פרטי כל המטרות
        [HttpGet("allDetails")]
        public async Task<IActionResult> GetAllTargetsDetails()
        {
            List<string> detailsAlltargets = new List<string>();

            var targets = await _dbContextAPI.Targets
                .Include(a => a.Position)
                .Include(a => a.LocationX)
                .Include(a => a.LocationY)
                .Include(a => a.StatusTarget)
                .ToArrayAsync();

            foreach (var target in targets)
            {
                string detailOneTarget = $"position: {target.Position}, location: ({target.LocationX}, {target.LocationY}), status: {target.StatusTarget}{Environment.NewLine}";
                detailsAlltargets.Add(detailOneTarget);
            }

            return Ok(detailsAlltargets);
        }



        // קביעת מיקום התחלתי
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> StartingPosition(int id, [FromBody] Location position)
        {
            Target? target = await this._dbContextAPI.Targets.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (target == null) return StatusCode(status, HttpUtils.Response(status, "target not found"));

            target.LocationX = position.X;
            target.LocationY = position.Y;
            _dbContextAPI.Update(target);
            await _serviceTrget.TimeMission(target);
            await _dbContextAPI.SaveChangesAsync();
            return Ok();
        }



        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, [FromBody] string directionStr)
        {
            Target? target = await _dbContextAPI.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<Direction>(directionStr, true, out var direction))
            {
                return BadRequest("Invalid direction value.");
            }

            int minX = 0;
            int minY = 0;
            int maxX = 1000;
            int maxY = 1000;

            int? currentX = target.LocationX;
            int? currentY = target.LocationY;

            switch (direction)
            {
                case Direction.nw:
                    target.LocationX -= 1;
                    target.LocationY += 1;
                    target.Direction = Direction.nw;
                    break;
                case Direction.n:
                    target.LocationY += 1;
                    target.Direction = Direction.n;
                    break;
                case Direction.ne:
                    target.LocationX += 1;
                    target.LocationY += 1;
                    target.Direction = Direction.ne;
                    break;
                case Direction.w:
                    target.LocationX -= 1;
                    target.Direction = Direction.w;
                    break;
                case Direction.e:
                    target.LocationX += 1;
                    target.Direction = Direction.e;
                    break;
                case Direction.sw:
                    target.LocationX -= 1;
                    target.LocationY -= 1;
                    target.Direction = Direction.sw;
                    break;
                case Direction.s:
                    target.LocationY -= 1;
                    target.Direction = Direction.s;
                    break;
                case Direction.se:
                    target.LocationX += 1;
                    target.LocationY -= 1;
                    target.Direction = Direction.se;
                    break;
                default:
                    return BadRequest("Invalid direction value.");
            }

            if (target.LocationX < minX || target.LocationX > maxX || target.LocationY < minY || target.LocationY > maxY)
            {
                target.LocationX = currentX;
                target.LocationY = currentY;
                return BadRequest("Can't be moved. Target outside the borders of the matrix.");
            }

            await _dbContextAPI.SaveChangesAsync();
            return Ok(target);
        }


        // מחיקת מטרה
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            Target? target = await this._dbContextAPI.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            _dbContextAPI.Targets.Remove(target);
            await _dbContextAPI.SaveChangesAsync();

            return NoContent();
        }

    }
}

   
