using System.Collections.Generic;
using Microsoft.Owin;
using MyLanguagePalService.Helpers.OwinMiddleware;
using Owin;

[assembly: OwinStartup(typeof(MyLanguagePalService.Startup))]

namespace MyLanguagePalService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            var options = new BasicAuthenticationOptions();

            // Add admin user for administration panel
            options.Users.Add(new BasicUser()
            {
                Login = "admin",
                Password = "12345678",
                Roles = new List<string>() { "Admin" }
            });

            // Setup
            app.UseBasicAuthentication(options);
        }
    }
}
