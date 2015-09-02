using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoShare.Web.Startup))]
namespace PhotoShare.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
