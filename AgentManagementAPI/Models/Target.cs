using AgentManagementAPI.Enums;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Target
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Role { get; set; }

        public Point LocationTarget = new Point();

        public StatusTarget StatusTarget { get; set; }
    }
}
