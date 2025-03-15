using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonalProject.Startup))]
namespace PersonalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
