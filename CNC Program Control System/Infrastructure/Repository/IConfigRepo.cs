using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public interface IConfigRepo
    {
        bool CheckIsDatabaseExist(NewDatabaseModel db);
        Task CreateDatabase(NewDatabaseModel db);
        Task CreateDatabaseTables(DatabaseCredentialModel db);
    }
}
