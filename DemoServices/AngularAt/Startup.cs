using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngularAt.Startup))]
namespace AngularAt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
