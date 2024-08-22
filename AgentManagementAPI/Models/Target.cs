using AgentManagementAPI.Enums;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Target : Entity
    {
 
        public StatusTarget StatusTarget { get; set; }

        public Direction DirectionTarget { get; set; }
    }
}
