namespace AgentManagementAPI.Models
{
    public class Kill
    {
        public int Id { get; set; }

        public int AgentID { get; set; }

        public int TargetID { get; set; }

        public DateTime ExecutionTime { get; set; }
    }

}
