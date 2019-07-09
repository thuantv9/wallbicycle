using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DentistryManager.Startup))]
namespace DentistryManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
