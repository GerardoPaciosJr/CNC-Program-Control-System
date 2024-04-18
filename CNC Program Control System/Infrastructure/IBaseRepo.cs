using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public interface IBaseRepo<T> where T : class, new()
    {
        
    }
}
