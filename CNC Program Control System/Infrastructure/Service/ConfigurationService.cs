using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Configuration.Json;

namespace CNC_Program_Control_System
{
    public class ConfigurationService
    {
        public DatabaseCredentialModel DatabaseConnectionModel { get; set; }
        public IConfiguration _config { get; set; }

        public ConfigurationService(IConfiguration config)
        {
            _config = config;
        }

        public DatabaseCredentialModel InitDBConnection()
        {
            var _object = new DatabaseCredentialModel
            {
                DatabaseName = _config.GetSection("ConnectionStrings:dbsetting").ToString(),
                DatabasePassword = _config.GetSection("ConnectionStrings:dbsetting").ToString(),
                ServerHostName = _config.GetSection("ConnectionStrings:dbsetting").ToString(),
                DatabaseUser = _config.GetSection("ConnectionStrings:dbsetting").ToString()
            };

            return _object;
        }

        public bool IsValidDBConnection()
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                ctx.DatabaseCredential = InitDBConnection(); // DatabaseConnectionModel;
                try
                {
                    ctx.Database.BeginTransaction();
                    ctx.Configuration.GetSection("ConnectionStrings:dbsetting").Value = ctx.Database.GetConnectionString().ToString();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
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

        public void CreateSQLConnectionModel(DatabaseCredentialModel model)
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

    }
}
