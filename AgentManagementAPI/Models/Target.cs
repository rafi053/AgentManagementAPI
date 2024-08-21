using AgentManagementAPI.Enums;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Target
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Role { get; set; }

        public Location LocationTarget { get; set; }

        public StatusTarget StatusTarget { get; set; }

        public Direction DirectionTarget { get; set; }
    }
}
