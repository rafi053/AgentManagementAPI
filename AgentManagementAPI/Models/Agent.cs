
using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        public string? Image { get; set; }

        public string Nickname { get; set; }

        public Location LocationAgent { get; set; }
        public StatusAgent StatusAgent { get; set; }

        public Direction DirectionAgent { get; set; }


    }
}
