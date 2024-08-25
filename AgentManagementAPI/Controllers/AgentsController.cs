using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Data;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Utils;
using AgentManagementAPI.Services;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase 
    {
        private readonly DbContextAPI _dbContextAPI;
       


        public AgentsController(DbContextAPI dbContextAPI, Time time)
        {
            _dbContextAPI = dbContextAPI;
           
        }


        // יצירת סוכן
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEntity(Agent agent)
        {
            _dbContextAPI.Agents.Add(agent);
            await _dbContextAPI.SaveChangesAsync();
            
            return StatusCode(
               StatusCodes.Status200OK,
               new { Id = agent.Id });
        }

        // הצג את כל הסוכנים
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAllEntities()
        {
            return await _dbContextAPI.Agents.ToListAsync();
        }

        // הצג סוכן
        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetEntity(int id)
        {
            Agent? agent = await this._dbContextAPI.Agents.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            return agent;
        }

        // קביעת מיקום התחלתי
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> StartingPosition(int id, Location position)
        {
            Agent? agent = await this._dbContextAPI.Agents.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (agent == null) return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            agent.LocationX = position.X;
            agent.LocationY = position.Y;
            _dbContextAPI.Update(agent);
            await _dbContextAPI.SaveChangesAsync();
            return Ok();
        }

        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, string directionStr)
        {
            Agent? agent = await _dbContextAPI.Agents.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<Direction>(directionStr, true, out var direction))
            {
                return BadRequest("Invalid direction value.");
            }

            agent.LocationX = 1000;
            agent.LocationY = 999;

            // עיבוד הכיוון וביצוע עדכון למיקום
            switch (direction)
            {
                case Direction.nw:
                    agent.LocationX -= 1;
                    agent.LocationY += 1;
                    break;
                case Direction.n:
                    agent.LocationY += 1;
                    break;
                case Direction.ne:
                    agent.LocationX += 1;
                    agent.LocationY += 1;
                    break;
                case Direction.w:
                    agent.LocationX -= 1;
                    break;
                case Direction.e:
                    agent.LocationX += 1;
                    break;
                case Direction.sw:
                    agent.LocationX -= 1;
                    agent.LocationY -= 1;
                    break;
                case Direction.s:
                    agent.LocationY -= 1;
                    break;
                case Direction.se:
                    agent.LocationX += 1;
                    agent.LocationY -= 1;
                    break;
                default:
                    return BadRequest("Invalid direction value.");
            }

            await _dbContextAPI.SaveChangesAsync();
            return Ok(agent);
        }

        // מחיקת סוכן
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            Agent? agent = await this._dbContextAPI.Agents.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            _dbContextAPI.Agents.Remove(agent);
            await _dbContextAPI.SaveChangesAsync();

            return NoContent();
        }



    }
}