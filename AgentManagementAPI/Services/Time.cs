using AgentManagementAPI.Data;
using AgentManagementAPI.Models;

namespace AgentManagementAPI.Services
{
    public class Time
    {
       

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
        public Mission GetTimeLeft(Mission mission)
        {
            if (mission.StatusMission.ToString() != "MitzvahForTheTask")
            {
                return mission;
            }

            double distance = GetDistance(mission.Target.LocationX, mission.Target.LocationY, mission.Agent.LocationX, mission.Agent.LocationY);
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

            double distance = GetDistance(mission.Target.LocationX, mission.Target.LocationY, mission.Agent.LocationX, mission.Agent.LocationY);
            double time = (distance / 5);
            mission.DurationTask += time;
            return mission;
        }

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
