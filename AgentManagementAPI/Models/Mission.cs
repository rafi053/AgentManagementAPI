using AgentManagementAPI.Enums;

namespace AgentManagementAPI.Models
{
    public class Mission
    {
        public int Id { get; set; }

        public int AgentID { get; set; }

        public DateTime TimeLeft { get; set; }

        public StatusMission StatusMission { get; set; }
    }
}
