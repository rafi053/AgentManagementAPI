using AgentManagementAPI.Data;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AgentManagementAPI.Services
{
    public class ServiceTrget
    {
        private readonly DbContextAPI _dbContextAPI;
        public ServiceTrget(DbContextAPI dbContextAPI)
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
        public async Task<ActionResult<IEnumerable<Agent>>> TimeMission(Target target)
        {
            var agents = await _dbContextAPI.Agents.ToListAsync();
            foreach (var agent in agents)
            {
                if (agent.LocationX.HasValue && agent.LocationY.HasValue &&
                    agent.LocationX.HasValue && agent.LocationY.HasValue)
                {
                    double distance = GetDistance(agent.LocationX.Value, agent.LocationY.Value,
                                                  agent.LocationX.Value, agent.LocationY.Value);
                    if (distance < 200)
                    {
                        await CreateMission(agent, target);
                        await _dbContextAPI.SaveChangesAsync();
                        return new List<Agent> { agent };
                    }
                }
            }
            return new List<Agent>();
        }

        // יצירת משימה
        private async Task CreateMission(Agent agent, Target target)
        {
            Mission mission = new Mission
            {
                AgentID = agent.Id,
                TargetID = target.Id,
                DurationTask = double.MinValue,
                TimeLeft = double.MaxValue,
                StatusMission = StatusMission.Offer.ToString(),
            };
            _dbContextAPI.Missions.Add(mission);
            await _dbContextAPI.SaveChangesAsync();
        }
    }
}
