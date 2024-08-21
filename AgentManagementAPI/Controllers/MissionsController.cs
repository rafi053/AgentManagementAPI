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
    public class MissionsController : ControllerBase
    {
        public DbContextAPI _dbContextAPI;
        public MissionsController(DbContextAPI dbContextAPI)
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

        [HttpPost("update")]
        [Produces("application/json")]



        // הצג את כל המטרות
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mission>>> GetAllTargets()
        {
            return await _dbContextAPI.Missions.ToListAsync();
        }


        // קביעת מיקום התחלתי
        [HttpPut("{id}")]
        public async Task<IActionResult> StartingPosition(int id, Mission mission, StatusMission statusMission)
        {
            mission = await this._dbContextAPI.Missions.FindAsync(id);

            int status = StatusCodes.Status404NotFound;
            if (mission == null) return StatusCode(status, HttpUtils.Response(status, "target not found"));
            if (mission.StatusMission != StatusMission.Assigned)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(
                    status,
                    new
                    {
                        success = false,
                        error = "Cannot Assigned mission  that hasalready Finished."
                    }
                );

            }

            mission.StatusMission = statusMission;
            
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Assigned success." }
            );

        }

        // קביעת מיקום חדש
        [HttpPut("{id}/move")]
        public async Task<IActionResult> DirectionPosition(int id, Target target, Direction direction)
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
                        error = "Cannot Direction Position an target that hasalready Eliminated."
                    }
                );

            }

            target.DirectionTarget = direction;
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Direction success." }
            );

        }
    }
}
