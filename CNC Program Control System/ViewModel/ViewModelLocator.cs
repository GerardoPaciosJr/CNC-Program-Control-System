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
        public MainWindowViewModel MainWindowVWM => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        public MainPageViewModel MainPageVWM => ServiceLocator.Current.GetInstance<MainPageViewModel>();
    }
}
