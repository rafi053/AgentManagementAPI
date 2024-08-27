using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentManagementAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public List<Agent>? Agents { get; set; }
        public int AgentID { get; set; }

        [NotMapped]
        public Target? Target { get; set; }

        public int TargetID { get; set; }

        public double? DurationTask { get; set; }

        public double? TimeLeft { get; set; }

        
        public Agent Agent { get; set; }

        public string? StatusMission { get; set; }
    }
}
