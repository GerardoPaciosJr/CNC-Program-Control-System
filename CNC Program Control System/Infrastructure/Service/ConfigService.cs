using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public class ConfigService : IConfigService
    {
        private readonly IConfigRepo _configRepo;


        public ConfigService(IConfigRepo configRepo)
        {
            _configRepo = configRepo;
        }

        public async Task CreateNewDBAsync(NewDatabaseModel db)
        {
            await _configRepo.CreateDatabase(db);
        }
        public async Task CreateNewDBTablesAsync(DatabaseCredentialModel db)
        {
            await _configRepo.CreateDatabaseTables(db);
        }


    }
}
