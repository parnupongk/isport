using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(isportMVC.Startup))]
namespace isportMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
