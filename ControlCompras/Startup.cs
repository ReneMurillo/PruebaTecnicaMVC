using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ControlCompras.Startup))]
namespace ControlCompras
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
