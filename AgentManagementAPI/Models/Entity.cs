using AgentManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        public string? Image { get; set; }

        public string Name { get; set; }

        public int LocationX { get; set; }

        public int LocationY { get; set; }
       
    }
}
