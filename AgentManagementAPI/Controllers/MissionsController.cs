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
        private readonly DbContextAPI _dbContextAPI;
        private readonly ServiceMissions _serviceMissions;
        public MissionsController(DbContextAPI dbContextAPI, ServiceMissions serviceMissionsr)
        {
            _dbContextAPI = dbContextAPI;
            _serviceMissions = serviceMissionsr;
        }

        // הצג את כל המשימות הפעילים
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Mission>>> GetAllEntitiesActive()
        {
            var activeAgents = await _dbContextAPI.Missions
                .Where(agent => agent.StatusMission == "Assigned").ToListAsync();

            if (activeAgents == null)
            {
                return NotFound();
            }

            return Ok(activeAgents);
        }

        // קבל את מספר המשימות
        [HttpGet("count")]
        public async Task<IActionResult> GetAllTargetsCount()
        {
            var activeAgents = await _dbContextAPI.Missions.ToArrayAsync();

            return Ok(activeAgents.Length + 1);
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
            await _serviceMissions.UpdateTime(mission);
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
            mission.StatusMission = StatusMission.Assigned.ToString();
           _dbContextAPI.Missions.Update(mission);
            await _dbContextAPI.SaveChangesAsync();
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { mission = mission }));
        }

        

        // עדכון מצב משימות
        //[HttpPost("update")]
        //public async Task<IActionResult> UpdateTimeLeft()
        //{
        //    int status = StatusCodes.Status200OK;
        //    var missions = await _dbContextAPI.Missions.ToListAsync();

        //    foreach (var mission in missions)
        //    {
        //        if (mission.StatusMission == StatusMission.Assigned)
        //        {
        //            var agent = await _dbContextAPI.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentID);
        //            var target = await _dbContextAPI.Targets.FirstOrDefaultAsync(t => t.Id == mission.TargetID);

        //            if (agent == null || target == null)
        //            {
        //                return StatusCode(StatusCodes.Status404NotFound, "Agent or Target not found for mission");
        //            }

        //            double distance = ServiceAgent.GetDistance(target.LocationX, target.LocationY, agent.LocationX, agent.LocationY);
        //            var moveDirection = ServiceAgent.GetDistance(target.LocationX, target.LocationY, agent.LocationX, agent.LocationY).ToString();

        //            mission.TimeLeft = distance;

        //            if (string.IsNullOrEmpty(moveDirection))
        //            {
        //                agent.StatusAgent = StatusAgent.IinActivity;
        //                target.StatusTarget = StatusTarget.Eliminated;
        //                target.LocationX = -1;
        //                target.LocationY =-1;

        //                mission.StatusMission = StatusMission.Finished;

        //                _dbContextAPI.Agents.Update(agent);
        //                _dbContextAPI.Targets.Update(target);
        //                _dbContextAPI.Missions.Update(mission);
        //            }
        //            else
        //            {

        //                _agentsController.DirectionPosition(agent.Id, agent.Direction.ToString());

        //                _dbContextAPI.Agents.Update(agent);
        //            }

        //            await _dbContextAPI.SaveChangesAsync();
        //        }
        //    }

        //    return StatusCode(
        //        status,
        //        HttpUtils.Response(status, new { missions = missions })
        //    );
        //}




    }
}





