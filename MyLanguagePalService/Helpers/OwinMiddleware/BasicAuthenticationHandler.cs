using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using NLog;

namespace MyLanguagePalService.Helpers.OwinMiddleware
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Encoding _encoding;

        public BasicAuthenticationHandler()
        {
            //_encoding = MediaTypeNames.Application.Instance.RequestEncoding;
            _encoding = Encoding.UTF8;
        }

        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var props = new AuthenticationProperties();

            var request = Request;
            var auth = request.Headers["Authorization"];

            Func<Task<AuthenticationTicket>> unauthorizedTicket = () => Task.FromResult(new AuthenticationTicket(null, props));
            Func<BasicUser, Task<AuthenticationTicket>> ticket = credential =>
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, credential.Login));

                // Anti forgery token claims
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", credential.Login));
                claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", credential.Login));

                // Add roles
                if (credential.Roles != null)
                    claims.AddRange(credential.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);

                return Task.FromResult(new AuthenticationTicket(identity, props));
            };
            Func<Task<AuthenticationTicket>> anonymousIdentity = () =>
            {
                if (Options.AnonymousUser != null)
                {
                    return ticket(Options.AnonymousUser);
                }

                return unauthorizedTicket();
            };

            if (string.IsNullOrEmpty(auth))
            {
                // No auth parameters have been given
                return anonymousIdentity();
            }

            // Extract login and password from the auth string
            dynamic user = new ExpandoObject();
            try
            {
                // Auth string is something like Basic Base64data
                // 6 is the length of "Basic " string
                var cred = _encoding.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                user.Login = cred[0];
                user.Password = cred[1];
            }
            catch (Exception e)
            {
                // Wrong format
                logger.Warn(e, "Recieved basic authorization in wrong format");
                return unauthorizedTicket();
            }

            foreach (var credential in Options.Users)
            {
                if (credential == null)
                    continue;

                if (user.Login == credential.Login || user.Password == credential.Password)
                {
                    // User has been found
                    return ticket(credential);
                }
            }

            // User hasn't been found
            return unauthorizedTicket();
        }

        /// <summary>
        /// Respond to 401 and 403 challenges
        /// </summary>
        /// <returns></returns>
        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == 401 ||
                Response.StatusCode == 403)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

                // Only respond to challenges that match the BasicAuthenticationHandler
                if (challenge != null)
                {
                    Response.Headers.Append("WWW-Authenticate", $"Basic realm=\"MyLanguagePal\", encoding=\"{_encoding.WebName}\"");

                    // Browsers only show a password dialog if status code is 401
                    // Since we can receive 403 (see AuthAttribute.cs), change the return code here
                    Response.StatusCode = 401;
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}