
using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Agent : Entity
    {
        public StatusAgent StatusAgent { get; set; }

        public Direction DirectionAgent { get; set; }
    }
   
}

