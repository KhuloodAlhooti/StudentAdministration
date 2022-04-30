using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentAdministration.Startup))]
namespace StudentAdministration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
