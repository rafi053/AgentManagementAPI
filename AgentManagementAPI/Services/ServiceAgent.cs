using AgentManagementAPI.Data;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Reflection;

namespace AgentManagementAPI.Services
{
    public class ServiceAgent
    {
        private readonly DbContextAPI _dbContextAPI;
        public ServiceAgent(DbContextAPI dbContextAPI)
        { 
            _dbContextAPI = dbContextAPI;
        }
       

        //בדיקת המרחק 
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
                
                if (target.LocationX.HasValue && target.LocationY.HasValue &&
                    agent.LocationX.HasValue && agent.LocationY.HasValue)
                {
                    double distance = GetDistance(target.LocationX.Value, target.LocationY.Value,
                                                  agent.LocationX.Value, agent.LocationY.Value);
                    if (distance < 200)
                    {
                        await CreateMission(agent, target);
                        await _dbContextAPI.SaveChangesAsync();
                        return new List<Target> { target };
                    }
                }
            }
            return new List<Target>();
        }

        // יצירת משימה
        private async Task CreateMission(Agent agent, Target target)
        {
            Mission mission = new Mission
            {
                AgentID = agent.Id,
                TargetID = target.Id,
                DurationTask = 0,
                TimeLeft =  GetTimeMission(agent, target),
                StatusMission = StatusMission.Offer.ToString(),
            };
            _dbContextAPI.Missions.Add(mission);
            await _dbContextAPI.SaveChangesAsync();
        }








        // חישוב סופי של המשימה
        private double GetTimeMission(Agent agent, Target target)
        {
            if (target.LocationX.HasValue && target.LocationY.HasValue &&
                agent.LocationX.HasValue && agent.LocationY.HasValue)
            {
                double distance = GetDistance(target.LocationX.Value, target.LocationY.Value,
                                              agent.LocationX.Value, agent.LocationY.Value);
                double time = distance / 5; 
                return time;
            }
            else
            {
                return double.MaxValue; 
            }
        }

       

        

        
       

    }
}
