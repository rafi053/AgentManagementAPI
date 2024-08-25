using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class LoginObject
    {
        [Key]
        public int IdKey { get; set; }
        public string Id { get; set; }
        

    }
}
