using MassActivation;
using ServiceBridge;

namespace Lenoard.Security
{
    [ActivationPriority(ActivationPriority.Highest)]
    public class Startup
    {
        public Startup(IActivatingEnvironment env)
        {
            env.Use<ISiteMapProvider>(new DefaultSiteMapProvider());
            env.Use<IActionMapProvider>(new DefaultActionMapProvider());
            env.Use<IAuthenticateProvider>(new DefaultAuthenticateProvider());
        }

        public void Configuration(IServiceContainer container)
        {
            container.Register<ISiteMapProvider, DefaultSiteMapProvider>();
            container.Register<IActionMapProvider, DefaultActionMapProvider>();
            container.Register<IAuthenticateProvider, DefaultAuthenticateProvider>();
        }
    }
}
