using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TweetFromOffice.BackEnd.Startup))]
namespace TweetFromOffice.BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
