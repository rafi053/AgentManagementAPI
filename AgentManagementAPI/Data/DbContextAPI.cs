using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace AgentManagementAPI.Data
{
    public class DbContextAPI : DbContext
    {
        public DbContextAPI(DbContextOptions<DbContextAPI> options) : base(options) { Database.EnsureCreated(); }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<LoginObject> Logins { get; set; }



    }
}
