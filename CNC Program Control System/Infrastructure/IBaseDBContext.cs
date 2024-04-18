using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public interface IBaseDBContext : IDisposable
    {
        ChangeTracker ChangeTracker { get; }
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        Task BulkSave<T>(IEnumerable<T> entities) where T : class, new();
        DatabaseCredentialModel DatabaseCredential { get; set; }

        Task<DataTable> ExecuteStoredProcedureAsync(string storedProcName, Dictionary<string, object> parameters);
       

        int SaveChanges();
        string ConnectionString { get; set; }
      
    }
}
