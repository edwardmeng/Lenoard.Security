using MassActivation;
using ServiceBridge;

namespace Lenoard.Security
{
    [ActivationPriority(ActivationPriority.Highest)]
    class Startup
    {
        public Startup(IActivatingEnvironment env)
        {
            env.Use<ISiteMapProvider>(new DefaultSiteMapProvider());
            env.Use<IPermissionProvider>(new DefaultPermissionProvider());
            env.Use<IAuthenticateProvider>(new DefaultAuthenticateProvider());
        }

        public void Configuration(IServiceContainer container)
        {
            container.Register<ISiteMapProvider, DefaultSiteMapProvider>();
            container.Register<IPermissionProvider, DefaultPermissionProvider>();
            container.Register<IAuthenticateProvider, DefaultAuthenticateProvider>();
        }
    }
}
