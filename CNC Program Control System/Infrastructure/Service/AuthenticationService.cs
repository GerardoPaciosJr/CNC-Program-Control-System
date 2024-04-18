using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace CNC_Program_Control_System
{
    public class AuthenticationService : IAuthenticationService
    {
        public DatabaseCredentialModel DatabaseConnectionModel { get; set; }
        public bool IsValidDBConnection()
        {
            using (BaseDBContext ctx = new BaseDBContext())
            {
                ctx.DatabaseCredential = DatabaseConnectionModel;
                try
                {
                    ctx.Database.BeginTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        //public bool IsValidDBConnection(DatabaseCredentialModel databaseCredentialModel)
        //{
        //    using (BaseDBContext ctx = new BaseDBContext())
        //    {
        //        //ctx.DatabaseCredential = DatabaseConnectionModel;// databaseCredentialModel;
        //        try
        //        {
        //            ctx.Database.BeginTransaction();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //    }
        //}
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

        //public string GetValidDBConnection()
        //{
        //    using (BaseDBContext ctx = new BaseDBContext())
        //    {
        //        //ctx.DatabaseCredential = DatabaseConnectionModel;
        //        try
        //        {
        //            //ctx.Database.SetCommandTimeout(0);
        //            //ctx.Database.BeginTransaction();
        //            return ctx.GetConnectionString(); ctx.Database.GetDbConnection().ConnectionString;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
        public void CreateSQLConnectionModel(DatabaseCredentialModel model)
        {

            var _object = new DatabaseCredentialModel
            {
                DatabaseName = model.DatabaseName,
                DatabasePassword = model.DatabasePassword,
                ServerHostName = model.ServerHostName,
                UserID = model.UserID
            };

            DatabaseConnectionModel = _object;
        }

    }
}
