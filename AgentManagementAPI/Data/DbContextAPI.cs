using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentManagementAPI.Data
{
    public class DbContextAPI : DbContext
    {
        public DbContextAPI(DbContextOptions<DbContextAPI> options) : base(options) { }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Mission> Missions { get; set; }

        public DbSet<Kill> Kills { get; set; }

    }
}
