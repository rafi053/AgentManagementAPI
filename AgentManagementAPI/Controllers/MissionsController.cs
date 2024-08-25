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
using AgentManagementAPI.Controllers;
using Microsoft.AspNetCore.Hosting.Server;
using AgentManagementAPI.Services;

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
            Mission? mission = await _dbContextAPI.Missions.FindAsync(id);

            if (mission == null)
            {
                return NotFound();
            }

            return mission;
        }

        // מחיקת משימה
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMission(int id)
        {
            Mission? mission = await _dbContextAPI.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }

            _dbContextAPI.Missions.Remove(mission);
            await _dbContextAPI.SaveChangesAsync();

            return NoContent();
        }

        //  יצירת משימה
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateMission(Mission mission)
        {
            if (ModelState.IsValid)
            {
                _dbContextAPI.Missions.Add(mission);
                await _dbContextAPI.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new { MissionId = mission.Id });
            }
            return BadRequest(ModelState);
        }


        //  לצוות סוכן למטרה       
        [HttpPut("{id}")]
        public async Task<IActionResult> AgentToMission(int id)
        {
            int status;
            Mission? mission = await _dbContextAPI.Missions.FindAsync(id);
            if (mission == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "mission not found"));
            }
            mission.StatusMission = StatusMission.Assigned;
           _dbContextAPI.Missions.Update(mission);
            await _dbContextAPI.SaveChangesAsync();
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { mission = mission }));
        }
        // עדכון מצב משימות
        [HttpPost("update")]
        public async Task<IActionResult> UpdateTimeLeft()
        {
            int status = StatusCodes.Status200OK;
            var missions = await _dbContextAPI.Missions.ToListAsync();
            foreach (Mission mission in missions)
            {
                Agent? agent = await _dbContextAPI.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentID);
                Target? target = await _dbContextAPI.Targets.FirstOrDefaultAsync(t => t.Id == mission.TargetID);
                if (agent == null || target == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Agent or Target not found for mission");
                }
                mission.TimeLeft = Time.GetDistance(target.LocationX, target.LocationY, agent.LocationX, agent.LocationY);
                await this._dbContextAPI.SaveChangesAsync();
            }
            return StatusCode(
                status,
                HttpUtils.Response(status, new { missions = missions })
                );
        }


    }
}





