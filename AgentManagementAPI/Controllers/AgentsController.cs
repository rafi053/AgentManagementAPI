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
        private readonly ServiceAgent _serviceAgent;
       


        public AgentsController(DbContextAPI dbContextAPI, ServiceAgent serviceAgent)
        {
            _dbContextAPI = dbContextAPI;
            _serviceAgent = serviceAgent;
            
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

        // הצג את כל הסוכנים הפעילים
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAllEntitiesActive()
        {
            var activeAgents = await _dbContextAPI.Agents
                .Where(agent => agent.StatusAgent == "IinActivity").ToListAsync();

            if (activeAgents == null)
            {
                return NotFound();
            }

            return Ok(activeAgents);
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
            await _serviceAgent.TimeMission(agent);
            await _dbContextAPI.SaveChangesAsync();
           
            return Ok();
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


        // קבלת פרטי כל הסוכנים
        [HttpGet("allDetails")]
        public async Task<IActionResult> GetAllAgantDetails()
        {
            List<Agent> detailsAllAgants = new List<Agent>();



            var Agants = await _dbContextAPI.Agents
                .ToArrayAsync();

            foreach (var agant in Agants)
            {
                Agent agantToAdd = new Agent();
                detailsAllAgants.Add(agantToAdd);
            }

            return Ok(detailsAllAgants);
        }



        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, [FromBody] string directionStr)
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

          
            int minX = 0;
            int minY = 0;
            int maxX = 1000;
            int maxY = 1000;

            int? currentX = agent.LocationX;
            int? currentY = agent.LocationY;

            switch (direction)
            {
                case Direction.nw:
                    agent.LocationX -= 1;
                    agent.LocationY += 1;
                    agent.Direction = Direction.nw;
                    break;
                case Direction.n:
                    agent.LocationY += 1;
                    agent.Direction = Direction.n;
                    break;
                case Direction.ne:
                    agent.LocationX += 1;
                    agent.LocationY += 1;
                    agent.Direction = Direction.ne;
                    break;
                case Direction.w:
                    agent.LocationX -= 1;
                    agent.Direction = Direction.w;
                    break;
                case Direction.e:
                    agent.LocationX += 1;
                    agent.Direction = Direction.e;
                    break;
                case Direction.sw:
                    agent.LocationX -= 1;
                    agent.LocationY -= 1;
                    agent.Direction = Direction.sw; 
                    break;
                case Direction.s:
                    agent.LocationY -= 1;
                    agent.Direction = Direction.s;
                    break;
                case Direction.se:
                    agent.LocationX += 1;
                    agent.LocationY -= 1;
                    agent.Direction = Direction.se;
                    break;
                default:
                    return BadRequest("Invalid direction value.");
            }

            if (agent.LocationX < minX || agent.LocationX > maxX || agent.LocationY < minY || agent.LocationY > maxY)
            {
              
                agent.LocationX = currentX;
                agent.LocationY = currentY;
                return BadRequest("Can't be moved. Agent outside the borders of the matrix.");
            }
            await _dbContextAPI.SaveChangesAsync();
            return Ok(agent);
        }

        // קבלת מספר יחסי של סוכנים למטרות
        [HttpGet("relative")]
        public async Task<IActionResult> GetRelativeAgant()
        {
            var agants = await _dbContextAPI.Agents.ToArrayAsync();

            var targets = await _dbContextAPI.Targets.ToArrayAsync();


            return Ok($"{agants.Length + 1} : {targets.Length + 1}");
        }


        // קבלת מספר יחסי של סוכנים למשימות
        [HttpGet("relativeAgant")]
        public async Task<IActionResult> GetRelativeAgantRole()
        {
            int relevantAgants = 0;

            int relevantMissions = 0;


            var Missions = await _dbContextAPI.Missions.ToArrayAsync();

            int i = 0;

            foreach (var mission in Missions)
            {

                if (mission.StatusMission == "offer" || mission.StatusMission == "Assigned")
                {
                    relevantMissions++;
                    relevantAgants += mission.Agents.Count + 1;
                }
                i++;
            }


            return Ok($"{relevantAgants} : {relevantMissions}");
        }

    }
}
