using AgentManagementAPI.Data;
using AgentManagementAPI.Models;

namespace AgentManagementAPI.Services
{
    public class Time
    {
       

        //בדיקת המרחק בין הסוכן למטרה
        public static double GetDistance(Location target, Location agent)
        {
            int x1 = target.X;
            int y1 = target.Y;

            int x2 = agent.X;
            int y2 = agent.Y;

            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return distance;
        }

        //חישוב זמן שנותר 
        public Mission GetTimeLeft(Mission mission)
        {
            if (mission.StatusMission.ToString() != "MitzvahForTheTask")
            {
                return mission;
            }

            double distance = GetDistance(mission.Agent.LocationAgent, mission.Target.LocationTarget);
            mission.TimeLeft = (distance / 5);

            return mission;
        }

        //חישוב סופי של המשימה
        public Mission GetTimeMission(Mission mission)
        {
            if (mission.StatusMission.ToString() != "MitzvahForTheTask")
            {
                return mission;
            }

            double distance = GetDistance(mission.Agent.LocationAgent, mission.Target.LocationTarget);
            double time = (distance / 5);
            mission.DurationTask += time;
            return mission;
        }
    }
}
