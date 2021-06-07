using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FacebookStats.Startup))]
namespace FacebookStats
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
