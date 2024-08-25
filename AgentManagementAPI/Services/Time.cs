using AgentManagementAPI.Data;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Reflection;

namespace AgentManagementAPI.Services
{
    public class Time
    {
        private readonly DbContextAPI _dbContextAPI;
        public Time(DbContextAPI dbContextAPI)
        { 
            _dbContextAPI = dbContextAPI;
        }
       

        //בדיקת המרחק בין הסוכן למטרה
        public static double GetDistance(int targetX, int targetY, int agentX, int agentY)
        {
            int x1 = targetX;
            int y1 = targetY;

            int x2 = agentX;
            int y2 = agentY;

            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return distance;
        }

        //חישוב זמן שנותר 
        public async Task<ActionResult<IEnumerable<Target>>> TimeMission(Agent agent)
        {
            var targets = await _dbContextAPI.Targets.ToListAsync();
            foreach (var target in targets)
            {
                var distance = GetDistance(target.LocationX, target.LocationY, agent.LocationX, agent.LocationY);
                if (distance < 200)
                {
                    await CreateMissionAsync(agent, target);
                    return (new List<Target> { target });
                }
            }
            return (new List<Target>());
        }

        // יצירת משימה
        private async Task CreateMissionAsync(Agent agent, Target target)
        {
            Mission mission = new Mission
            {
                AgentID = agent.Id,
                TargetID = target.Id,
                DurationTask = double.MinValue,
                TimeLeft = double.MaxValue,
                StatusMission = StatusMission.Offer,
            };
            _dbContextAPI.Missions.Add(mission);
            await _dbContextAPI.SaveChangesAsync();
        }


        




        
        //חישוב סופי של המשימה
        //public Mission GetTimeMission(Mission mission)
        //{
        //    if (mission.StatusMission.ToString() != "MitzvahForTheTask")
        //    {
        //        return mission;
        //    }

        //    double distance = GetDistance(mission.Target.LocationX, mission.Target.LocationY, mission.Agent.LocationX, mission.Agent.LocationY);
        //    double time = (distance / 5);
        //    mission.DurationTask += time;
        //    return mission;
        //}

        //public Mission TaskChecker()
        //{
        //    if (mission.StatusMission.ToString() != "MitzvahForTheTask")
        //    {
        //        return mission;
        //    }

        //    double distance = GetDistance(mission.Agent.LocationAgent, mission.Target.LocationTarget);
        //    double time = (distance / 5);
        //    mission.DurationTask += time;
        //    return mission;
        //}

    }
}
