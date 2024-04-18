using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public class BaseDBContext : DbContext, IBaseDBContext
    {
        public DatabaseCredentialModel DatabaseCredential { get; set; }

        public string ConnectionString { get; set; }

        public IConfiguration Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (DatabaseCredential == null)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = builder.Build();
                ConnectionString = Configuration.GetConnectionString("dbsetting");
            }
            else
            {
                ConnectionString = (@"server=" + DatabaseCredential.ServerHostName +
                                    ";database=" + DatabaseCredential.DatabaseName +
                                    ";uid=" + DatabaseCredential.UserID +
                                    ";password=" + DatabaseCredential.DatabasePassword +
                                    ";TrustServerCertificate=True");
            }

            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            System.Reflection.Assembly assemblyWithConfigurations = GetType().Assembly; //get whatever assembly you want
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
        }

        public Task BulkSave<T>(IEnumerable<T> entities) where T : class, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
