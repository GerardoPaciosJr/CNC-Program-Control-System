using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
//using NinjaNye.SearchExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class, new()
    {
        private readonly IBaseDBContext _IBaseDBContext;

        public BaseRepo(IBaseDBContext context) { _IBaseDBContext = context; }

       

    }
}
