using System;
using System.Configuration;
using cloudworkApi.DataManagers;
using cloudworkApi.Models.dsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace cloudworkApi.SqlDataBaseEntity
{
    public class CloudWorkContext : DbContext
    {
        public DbSet<ProjectBids> ProjectBids { get; set; }
        public DbSet<Project> Projects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DataManager._connectionString);
        }

    }
}
