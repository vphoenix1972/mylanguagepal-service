using Owin;

namespace MyLanguagePalService.Helpers.OwinMiddleware
{
    public static class BasicAuthenticationExtensions
    {
        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, BasicAuthenticationOptions options)
        {
            return app.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}