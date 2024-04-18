using System;
using System.Collections.Generic;
using CommonServiceLocator;
using SimpleInjector;
using System.Linq;

namespace CNC_Program_Control_System
{
    public class SimpleInjectorServiceLocatorAdapter : IServiceLocator
    {
        private readonly Container _Container;

        public SimpleInjectorServiceLocatorAdapter(Container container)
        {
            this._Container = container;
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return _Container.GetAllInstances(typeof(TService)).Cast<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            IServiceProvider serviceProvider = _Container;
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var collection = (IEnumerable<object>)serviceProvider.GetService(collectionType);
            return collection ?? Enumerable.Empty<object>();
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)this.GetInstance(typeof(TService));
        }

        public TService GetInstance<TService>()
        {
            return (TService)_Container.GetInstance(typeof(TService));
        }

        public object GetInstance(Type serviceType, string key)
        {
            return this.GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType)
        {
            return _Container.GetInstance(serviceType);
        }

        public object GetService(Type serviceType)
        {
            IServiceProvider serviceProvider = _Container;
            return serviceProvider.GetService(serviceType);
        }

    }
}
