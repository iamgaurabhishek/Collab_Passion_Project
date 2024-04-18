using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPassionProjectW2024n01605783.Startup))]
namespace MyPassionProjectW2024n01605783
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
