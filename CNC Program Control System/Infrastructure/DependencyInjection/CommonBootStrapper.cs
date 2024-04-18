using System;
using CommonServiceLocator;

namespace CNC_Program_Control_System
{
    public abstract class CommonBootStrapper : IDisposable
    {
        public IServiceLocator ServiceLocator;
        protected abstract IServiceLocator CreateServiceLocator();

        protected CommonBootStrapper()
        {
            ServiceLocator = CreateServiceLocator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected abstract void Dispose(bool disposing);
    }
}
