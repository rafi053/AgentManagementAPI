using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Target 

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PhotoUrl { get; set; }



        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int? LocationX { get; set; }

        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int? LocationY { get; set; } 

        public Direction? Direction { get; set; }
        public string? Position { get; set; }
 
        public string? StatusTarget { get; set; }

        
    }
}
