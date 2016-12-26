using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(isport_mvc.Startup))]
namespace isport_mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
