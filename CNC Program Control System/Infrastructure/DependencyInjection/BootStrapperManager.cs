using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    public class BootStrapperManager
    {
        private static CommonBootStrapper _BootStrapper;

        public static void Initialize(CommonBootStrapper bootStrapper)
        {
            _BootStrapper = bootStrapper;
        }

        public static CommonBootStrapper BootStrapper
        {
            get { return _BootStrapper; }
        }
    }
}
