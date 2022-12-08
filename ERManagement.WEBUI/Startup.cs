using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HRManagement.WEBUI.Startup))]
namespace HRManagement.WEBUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
