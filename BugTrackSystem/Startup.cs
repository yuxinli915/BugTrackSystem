using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTrackSystem.Startup))]
namespace BugTrackSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
