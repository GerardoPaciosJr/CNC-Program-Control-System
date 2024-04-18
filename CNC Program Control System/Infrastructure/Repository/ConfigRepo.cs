using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace CNC_Program_Control_System
{
    public class ConfigRepo : BaseRepo<NewDatabaseModel>, IConfigRepo
    {
        private readonly IBaseDBContext _IBaseDBContext;
        public ConfigRepo(IBaseDBContext context) : base(context) {
            _IBaseDBContext = context;
        }

        public async Task CreateDatabase(NewDatabaseModel db)
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                using (SqlConnection connection = new SqlConnection(ctx.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    string createDatabaseQuery = $"CREATE DATABASE {db.DatabaseName}";
                    SqlCommand command = new SqlCommand(createDatabaseQuery, connection);
                    command.ExecuteNonQuery();

                    string createUserQuery = $"CREATE LOGIN {db.DatabaseUser} WITH PASSWORD = '{db.DatabasePassword}';" +
                                                $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                                                $"USE {db.DatabaseName}; " +
                                                $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                                                $"USE {db.DatabaseName}; " +
                                                $"ALTER ROLE db_owner ADD MEMBER {db.DatabaseUser};";
                    SqlCommand createUserCommand = new SqlCommand(createUserQuery, connection);
                    await createUserCommand.ExecuteNonQueryAsync();

                }
            }
        }

        public async Task CreateDatabaseTables(DatabaseCredentialModel db)
        {
            string sqlFilePath = Directory.GetCurrentDirectory() + "\\tables.sql";
            if (!File.Exists(sqlFilePath))
            {
                MessageBox.Show("SQL file not found.");
                return;
            }
            string sqlScript = File.ReadAllText(sqlFilePath);

            using (BaseDBContext ctx = new BaseDBContext())
            {
                ctx.DatabaseCredential = db;
                await ctx.Database.ExecuteSqlAsync(FormattableStringFactory.Create(sqlScript));

            }
        }
    }
}
