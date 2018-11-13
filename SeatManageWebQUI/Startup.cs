using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeatManageWebQUI.Startup))]
namespace SeatManageWebQUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
