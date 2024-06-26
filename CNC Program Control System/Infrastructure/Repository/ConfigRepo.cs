﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

        public bool GetDatabaseConnection(NewDatabaseModel db)
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                using (SqlConnection connection = new SqlConnection(ctx.Database.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select count(*) from master.dbo.sysdatabases where name=@database";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@database", System.Data.SqlDbType.NVarChar).Value = db.DatabaseName;
                    command.ExecuteNonQuery();
                    return Convert.ToInt32(command.ExecuteScalar()) == 1;
                }
            }
        }

        public bool CheckIsDatabaseExist(NewDatabaseModel db)
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                using (SqlConnection conn = new SqlConnection(ctx.DefaultConnectionString().ToString()))
                {
                    conn.Open();
                    string query = "select count(*) from master.dbo.sysdatabases where name=@database";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.Add("@database", System.Data.SqlDbType.NVarChar).Value = db.DatabaseName;
                    command.ExecuteNonQuery();
                    return Convert.ToInt32(command.ExecuteScalar()) == 1;
                }
            }
        }

        public async Task CreateDatabase(NewDatabaseModel db)
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                ctx.Database.SetConnectionString(ctx.DefaultConnectionString().ToString());
                ctx.Database.SetCommandTimeout(0);
                string createDatabaseQuery = $"CREATE DATABASE {db.DatabaseName}; ";
                ctx.Database.ExecuteSqlRaw(FormattableStringFactory.Create(createDatabaseQuery).ToString());

                string createUserQuery = $"CREATE LOGIN {db.DatabaseUser} WITH PASSWORD = '{db.DatabasePassword}'; " +
                                         $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                                         $"USE {db.DatabaseName}; " +
                                         $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                                         $"USE {db.DatabaseName}; " +
                                         $"ALTER ROLE db_owner ADD MEMBER {db.DatabaseUser};";
                ctx.Database.ExecuteSqlRaw(FormattableStringFactory.Create(createUserQuery).ToString());

                //using (SqlConnection connection = new SqlConnection(ctx.DefaultConnectionString().ToString()))
                //{
                //    connection.Open();
                //    string createDatabaseQuery = $"CREATE DATABASE {db.DatabaseName}";
                //    SqlCommand command = new SqlCommand(createDatabaseQuery, connection);
                //    command.ExecuteNonQuery();

                //    string createUserQuery = $"CREATE LOGIN {db.DatabaseUser} WITH PASSWORD = '{db.DatabasePassword}';" +
                //                                $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                //                                $"USE {db.DatabaseName}; " +
                //                                $"CREATE USER {db.DatabaseUser} FOR LOGIN {db.DatabaseUser}; " +
                //                                $"USE {db.DatabaseName}; " +
                //                                $"ALTER ROLE db_owner ADD MEMBER {db.DatabaseUser};";
                //    SqlCommand createUserCommand = new SqlCommand(createUserQuery, connection);
                //    await createUserCommand.ExecuteNonQueryAsync();
                //    connection.Close();

                //}
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
                //ctx.Database.BeginTransaction();
                await ctx.Database.ExecuteSqlAsync(FormattableStringFactory.Create(sqlScript));

            }
        }
    }
}
