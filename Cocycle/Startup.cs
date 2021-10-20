using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cocycle.Startup))]
namespace Cocycle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
