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

namespace AgentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly DbContextAPI _dbContextAPI;

        public AgentController(DbContextAPI dbContextAPI)
        {
            _dbContextAPI = dbContextAPI;
        }


        // יצירת סוכן
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAgent(Agent agent)
        {
            _dbContextAPI.Agents.Add(agent);
            await _dbContextAPI.SaveChangesAsync();

            return StatusCode(
           StatusCodes.Status201Created,
           new { agentId = agent.Id });
        }

        // הצג את כל הסוכנים
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAllAgents()
        {
            return await _dbContextAPI.Agents.ToListAsync();
        }

        // קביעת מיקום התחלתי
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> StartingPosition(int id, Agent agent, int x, int y)
        {
            agent = await this._dbContextAPI.Agents.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (agent == null) return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            if (agent.StatusAgent == StatusAgent.Dormant)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(
                    status,
                    new
                    {
                        success = false,
                        error = "Cannot Starting Position an agent that hasalready IinActivity."
                    }
                );

            }

            agent.LocationAgent.X = x;
            agent.LocationAgent.Y = y;
           
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Position success." }
            );

        }

        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, Agent agent, Direction direction)
        {
            agent = await this._dbContextAPI.Agents.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (agent == null) return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            if (agent.StatusAgent == StatusAgent.Dormant)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(
                    status,
                    new
                    {
                        success = false,
                        error = "Cannot Direction Position an agent that hasalready IinActivity."
                    }
                );

            }

            agent.DirectionAgent = direction;
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Direction success." }
            );

        }


    }
}