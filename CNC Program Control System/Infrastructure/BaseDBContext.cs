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

        public IServiceProvider ServiceProvider { get; set; }
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

        public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, Dictionary<string, object> parameters)
        {

            DataTable dataTable = new DataTable();

            var conn = this.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            string query = string.Format("{0};", storedProcedureName);
            command.CommandText = query;
            command.CommandType = CommandType.StoredProcedure;

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value;
                command.Parameters.Add(parameter);
            }

            var reader = await command.ExecuteReaderAsync();
            await Task.Run(() => dataTable.Load(reader));
            await conn.CloseAsync();
            return dataTable;
        }

        public async Task CreateDatabase(string NewDatabaseName, string NewDBUsername, string NewDBPassword)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.Database.GetDbConnection().ConnectionString))
                {
                    connection.Open();
                    string createDatabaseQuery = $"CREATE DATABASE {NewDatabaseName}";
                    SqlCommand command = new SqlCommand(createDatabaseQuery, connection);
                    command.ExecuteNonQuery();

                    // Create User with Admin Permissions
                    string createUserQuery = $"CREATE LOGIN {NewDBUsername} WITH PASSWORD = '{NewDBPassword}';" +
                                                $"USE {NewDatabaseName}; " +
                                                $"CREATE USER {NewDBUsername} FOR LOGIN {NewDBUsername}; " +
                                                $"ALTER ROLE db_owner ADD MEMBER {NewDBUsername};";

                    SqlCommand createUserCommand = new SqlCommand(createUserQuery, connection);
                    createUserCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("");
            }
        }


    }
}
