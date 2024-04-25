using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Configuration;

namespace CNC_Program_Control_System
{
    public class AuthenticationService : IAuthenticationService
    {
        public DatabaseCredentialModel DatabaseConnectionModel { get; set; }

        public bool IsValidDBConnection(bool isTest)
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                if (isTest) ctx.DatabaseCredential = DatabaseConnectionModel;
                try
                {
                    ctx.Database.BeginTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    GetSQLConnectionOnModel(ctx.Database.GetConnectionString());
                }
            }
        }

        public string IsValidDBConnectionString()
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                ctx.DatabaseCredential = DatabaseConnectionModel;
                try
                {
                    ctx.Database.BeginTransaction();
                    return ctx.Database.GetConnectionString();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void CreateSQLConnectionOnModel(DatabaseCredentialModel model)
        {

            var _object = new DatabaseCredentialModel
            {
                DatabaseName = model.DatabaseName,
                DatabasePassword = model.DatabasePassword,
                ServerHostName = model.ServerHostName,
                DatabaseUser = model.DatabaseUser
            };

            DatabaseConnectionModel = _object;
        }

        public void GetSQLConnectionOnModel(string conn)
        {

            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder.ConnectionString = conn;

            var _object = new DatabaseCredentialModel
            {
                DatabaseName =  (builder.TryGetValue("database", out var db))? db.ToString():"", // ?"": builder["database"].ToString(),
                DatabasePassword = (builder.TryGetValue("password", out var ps)) ? ps.ToString() : "", //string.IsNullOrEmpty(builder["password"].ToString())?"": builder["password"].ToString(),
                ServerHostName = (builder.TryGetValue("Server", out var sr)) ? sr.ToString() : "", //string.IsNullOrEmpty(builder["Server"].ToString())?"": builder["Server"].ToString(),
                DatabaseUser = (builder.TryGetValue("uid", out var ui)) ? ui.ToString() : "" //string.IsNullOrEmpty(builder["uid"].ToString())?"": builder["uid"].ToString()
            };

            DatabaseConnectionModel = _object;
        }

    }
}
