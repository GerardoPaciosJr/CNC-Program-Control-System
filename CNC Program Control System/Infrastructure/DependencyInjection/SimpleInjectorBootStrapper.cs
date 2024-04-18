using CommonServiceLocator;
using Prism.Events;
using SimpleInjector;
using System;

namespace CNC_Program_Control_System
{
    public class SimpleInjectorBootStrapper : CommonBootStrapper
    {
        #region Private Properties
        private Container _Container;
        #endregion

        protected override IServiceLocator CreateServiceLocator()
        {
            // Create SimpleInjector IoC instance
            _Container = new Container();

            RegisterCommon(_Container);
            RegisterForViewLocator(_Container);
            RegisterRepositories(_Container);
            RegisterServices(_Container);

            _Container.Verify();
            return new SimpleInjectorServiceLocatorAdapter(_Container);
        }

        private void RegisterCommon(Container container)
        {
            container.RegisterSingleton<IBaseDBContext, BaseDBContext>();
            container.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();
        }

        private void RegisterForViewLocator(Container container)
        {
            container.Register<MainPageViewModel>();
        }

        private void RegisterRepositories(Container container)
        {
            container.Register(typeof(IBaseRepo<>), typeof(BaseRepo<>), Lifestyle.Scoped);
            container.RegisterSingleton<IConfigRepo, ConfigRepo>();
        }

        private void RegisterServices(Container container)
        {
            //container.RegisterSingleton<IAppService, AppService>();
            container.RegisterSingleton<IConfigService, ConfigService>();
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
