using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NLBPenziskiFond.Startup))]
namespace NLBPenziskiFond
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
