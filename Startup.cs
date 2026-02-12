using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SplitTree.Startup))]
namespace SplitTree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
