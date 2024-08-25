using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Utils;
using AgentManagementAPI.Enums;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
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

        //// קביעת מיקום התחלתי
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> StartingPosition(int id, [FromBody] Location position)
        {
            Target? target = await this._dbContextAPI.Targets.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (target == null) return StatusCode(status, HttpUtils.Response(status, "target not found"));

            target.LocationX = position.X;
            target.LocationY = position.Y;
            _dbContextAPI.Update(target);
            await _dbContextAPI.SaveChangesAsync();
            return Ok();
        }



        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, string directionStr)
        {
            Target? target = await _dbContextAPI.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }
            // המרת ENUM לסטרינג

            if (!Enum.TryParse<Direction>(directionStr, true, out var direction))
            {
                return BadRequest("Invalid direction value.");
            }

            target.LocationX = 1000;
            target.LocationY = 999;

            
            switch (direction)
            {
                case Direction.nw:
                    target.LocationX -= 1;
                    target.LocationY += 1;
                    break;
                case Direction.n:
                    target.LocationY += 1;
                    break;
                case Direction.ne:
                    target.LocationX += 1;
                    target.LocationY += 1;
                    break;
                case Direction.w:
                    target.LocationX -= 1;
                    break;
                case Direction.e:
                    target.LocationX += 1;
                    break;
                case Direction.sw:
                    target.LocationX -= 1;
                    target.LocationY -= 1;
                    break;
                case Direction.s:
                    target.LocationY -= 1;
                    break;
                case Direction.se:
                    target.LocationX += 1;
                    target.LocationY -= 1;
                    break;
                default:
                    return BadRequest("Invalid direction value.");
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

   
