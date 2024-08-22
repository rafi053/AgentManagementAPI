using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Location
    {
        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int X { get; set; }
        [Range(0, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int Y { get; set; }
    }
}