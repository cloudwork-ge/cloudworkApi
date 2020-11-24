using System;
using System.Configuration;
using cloudworkApi.DataManagers;
using cloudworkApi.Models.dsModels;
using cloudworkApi.Models.tableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace cloudworkApi.SqlDataBaseEntity
{
    public class CloudWorkContext : DbContext
    {
        public DbSet<tbProjectBids> ProjectBids { get; set; }
        public DbSet<tbProjects> Projects { get; set; }
        public DbSet<Project> V_Projects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DataManager._connectionString);
        }

    }
}
