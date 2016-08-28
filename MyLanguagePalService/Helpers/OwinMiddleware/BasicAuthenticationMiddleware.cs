using Microsoft.Owin.Security.Infrastructure;

namespace MyLanguagePalService.Helpers.OwinMiddleware
{
    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        public BasicAuthenticationMiddleware(Microsoft.Owin.OwinMiddleware next, BasicAuthenticationOptions options) :
            base(next, options)
        {
        }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler();
        }
    }
}