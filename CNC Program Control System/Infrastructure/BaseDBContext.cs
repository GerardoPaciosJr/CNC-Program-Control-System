using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using System.Xml;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace CNC_Program_Control_System
{
    public class BaseDBContext : DbContext, IBaseDBContext
    {
        public DatabaseCredentialModel DatabaseCredential { get; set; }

        public string ConnectionString { get; set; }

        public IConfiguration Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            ConnectionString = Configuration.GetConnectionString("dbsetting");

            if (DatabaseCredential != null)
            {
                ConnectionString = (@"server=" + DatabaseCredential.ServerHostName +
                                    ";database=" + DatabaseCredential.DatabaseName +
                                    ";uid=" + DatabaseCredential.DatabaseUser +
                                    ";password=" + DatabaseCredential.DatabasePassword +
                                    ";TrustServerCertificate=True");

            }

            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);

            if (DatabaseCredential != null) { SaveConnection(); }
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

        public void SaveConnection()
        {
            //To be modified
            //Configuration.GetSection("ConnectionStrings:dbsetting").Value = "Server=;database=;uid=;password=;TrustServerCertificate=True;";
            string pth = @"C:\Users\Jhe Pacios\source\repos\CNC Program Control System\CNC Program Control System\bin\Debug\net8.0-windows\appsettings.json"; 
            var json = File.ReadAllText(pth);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj["ConnectionStrings"]["dbsetting"] = ConnectionString;
            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(pth, output);
        }

        public string DefaultConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            return Configuration.GetConnectionString("defaultsetting");
        }
    }
}
