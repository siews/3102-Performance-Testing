using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SgProperty.Startup))]
namespace SgProperty
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
