using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERManagement.WEBUI.Startup))]
namespace ERManagement.WEBUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
