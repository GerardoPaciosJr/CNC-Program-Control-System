using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public interface IConfigService
    {
        bool CheckNewDBExist(NewDatabaseModel db);
        Task CreateNewDBAsync(NewDatabaseModel db);
        Task CreateNewDBTablesAsync(DatabaseCredentialModel db);
    }
}
