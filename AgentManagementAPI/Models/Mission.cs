using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public Agent Agent { get; set; }

        public int AgentID { get; set; }

        public Target Target { get; set; }

        public int TargetID { get; set; }

        public double? DurationTask { get; set; }

        public double? TimeLeft { get; set; }

        public StatusMission StatusMission { get; set; }
    }
}
