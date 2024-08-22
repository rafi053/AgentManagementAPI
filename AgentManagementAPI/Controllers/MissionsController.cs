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
    [Route("[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        public DbContextAPI _dbContextAPI;
        public MissionsController(DbContextAPI dbContextAPI)
        {
            _dbContextAPI = dbContextAPI;
        }


        // הצג את כל המשימות
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mission>>> GetAllMissions()
        {
            return await _dbContextAPI.Missions.ToListAsync();
        }

        // הצג משימה
        [HttpGet("{id}")]
        public async Task<ActionResult<Mission>> GetOneMission(int id)
        {
            Mission mission = await _dbContextAPI.Missions.FindAsync(id);

            if (mission == null)
            {
                return NotFound();
            }

            return mission;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AgentToMission(int id, Mission mission)
        {
            if (id != mission.Id)
            {
                return BadRequest();
            }

            _dbContextAPI.Entry(mission).State = EntityState.Modified;

            try
            {
                await _dbContextAPI.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //    [HttpPut("update")]
        //    public async Task<IActionResult> ControlleMission(StatusMission assignToTask)
        //    {
        //        List<Mission> statusMissions = await _dbContextAPI.Missions.Where(m => m.StatusMission == "AssignToTask").ToListAsync();


        //        return NoContent();
        //    }

        private bool Exists(int id)
        {
            return _dbContextAPI.Targets.Any(e => e.Id == id);
        }


        //    // יצירת משימה
        //    [HttpPost]
        //    [Produces("application/json")]
        //    [ProducesResponseType(StatusCodes.Status201Created)]
        //    public async Task<IActionResult> CreateMissions(Mission mission)
        //    {
        //        mission.StatusTarget = StatusTarget.Live;
        //        _dbContextAPI.Targets.Add(target);
        //        await _dbContextAPI.SaveChangesAsync();

        //        return StatusCode(
        //       StatusCodes.Status201Created,
        //       new { targetId = target.Id });
        //    }

        //    [HttpPost("update")]
        //    [Produces("application/json")]
    }
}


   

