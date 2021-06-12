using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigShop___.Startup))]
namespace BigShop___
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
