using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DriveApi.Startup))]
namespace DriveApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
        }
    }
}
