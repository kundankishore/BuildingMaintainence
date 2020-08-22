using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFIRM.Startup))]
namespace EFIRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
