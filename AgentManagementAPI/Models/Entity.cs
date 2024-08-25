using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        

        public string? PhotoUrl { get; set; }

        

        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int LocationX { get; set; } = 1000;

        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int LocationY { get; set; } = 999;

        public Direction? Direction { get; set; }
    }
}
