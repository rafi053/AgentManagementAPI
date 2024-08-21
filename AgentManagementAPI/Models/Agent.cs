
using AgentManagementAPI.Enums;

namespace AgentManagementAPI.Models
{
    public class Agent
    {
        public int Id { get; set; }

        public string? Image { get; set; }

        public string Nickname { get; set; }

        public Location location { get; set; }

        public Status status { get; set; }




    }
}
