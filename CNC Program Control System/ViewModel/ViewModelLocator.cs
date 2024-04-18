using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNC_Program_Control_System
{
    class ViewModelLocator
    {
        public MainPageViewModel MainPageVWM => ServiceLocator.Current.GetInstance<MainPageViewModel>();
    }
}
