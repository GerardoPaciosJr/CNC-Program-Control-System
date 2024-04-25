using Microsoft.Extensions.Configuration;

namespace CNC_Program_Control_System
{
    public interface IAuthenticationService
    {
        DatabaseCredentialModel DatabaseConnectionModel { get; set; }
        bool IsValidDBConnection(bool isTest);
        string IsValidDBConnectionString();
        void CreateSQLConnectionOnModel(DatabaseCredentialModel model);
        void GetSQLConnectionOnModel(string conn);
    }
}
