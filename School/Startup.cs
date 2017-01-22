using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(School.Startup))]
namespace School
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
