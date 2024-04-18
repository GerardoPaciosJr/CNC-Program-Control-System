namespace CNC_Program_Control_System
{
    public interface IAuthenticationService
    {
        DatabaseCredentialModel DatabaseConnectionModel { get; set; }
        bool IsValidDBConnection();
        //bool IsValidDBConnection(DatabaseCredentialModel databaseCredentialModel);
        string IsValidDBConnectionString();
        //string GetValidDBConnection();

        void CreateSQLConnectionModel(DatabaseCredentialModel model);
    }
}
