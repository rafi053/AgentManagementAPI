using AgentManagementAPI.Data;
using AgentManagementAPI.Enums;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Reflection;

namespace AgentManagementAPI.Services
{
    public class ServiceMissions
    {
        private readonly DbContextAPI _dbContextAPI;
        public ServiceMissions(DbContextAPI dbContextAPI)
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
        public async Task CreateMission(Agent agent, Target target)
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



        // עדכון זמן בכל קריאה למשימה

        public async Task UpdateTime(Mission mission)
        {
            if (mission.StatusMission == StatusMission.Assigned.ToString())
            {
                if (mission.Target.LocationX.HasValue && mission.Target.LocationY.HasValue &&
                    mission.Agent.LocationX.HasValue && mission.Agent.LocationY.HasValue)
                {
                    double distance = GetDistance(mission.Target.LocationX.Value, mission.Target.LocationY.Value,
                                                  mission.Agent.LocationX.Value, mission.Agent.LocationY.Value);
                    double time = distance / 5; 

                    mission.TimeLeft = time;
                    await _dbContextAPI.SaveChangesAsync();
                }
            }
        }

        private async Task UpdateDurationTaskTime(Mission mission)
        {
            if (mission.StatusMission == StatusMission.Finished.ToString())
            {
                double cumulativeTime = 0;
                bool isTargetMoving = true;

                while (isTargetMoving)
                {

                    isTargetMoving = IsTargetMoving(mission.Target);

                    if (mission.Target.LocationX.HasValue && mission.Target.LocationY.HasValue &&
                        mission.Agent.LocationX.HasValue && mission.Agent.LocationY.HasValue)
                    {
                        double distance = GetDistance(mission.Target.LocationX.Value, mission.Target.LocationY.Value,
                                                      mission.Agent.LocationX.Value, mission.Agent.LocationY.Value);
                        double time = distance / 5;
                        cumulativeTime += time;
                    }
                    await Task.Delay(1000);
                }

                mission.DurationTask = cumulativeTime;
                await _dbContextAPI.SaveChangesAsync();
            }
        }

        private bool IsTargetMoving(Target target)
        {
            
            return true;
        }


        

    }
}
