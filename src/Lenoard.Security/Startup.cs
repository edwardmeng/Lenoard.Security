using MassActivation;
using ServiceBridge;

namespace Lenoard.Security
{
    [ActivationPriority(ActivationPriority.Highest)]
    class Startup
    {
        public Startup(IActivatingEnvironment env)
        {
            env.Use<ISiteMapStore>(new DefaultSiteMapStore());
            env.Use<IPermissionStore>(new DefaultPermissionStore());
            env.Use<IAuthenticateStore>(new DefaultAuthenticateStore());
        }

        public void Configuration(IServiceContainer container)
        {
            container.Register<ISiteMapStore, DefaultSiteMapStore>();
            container.Register<IPermissionStore, DefaultPermissionStore>();
            container.Register<IAuthenticateStore, DefaultAuthenticateStore>();
        }
    }
}
