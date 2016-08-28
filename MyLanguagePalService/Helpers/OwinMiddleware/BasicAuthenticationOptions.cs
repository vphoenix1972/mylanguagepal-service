using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Owin.Security;

namespace MyLanguagePalService.Helpers.OwinMiddleware
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationOptions()
            : base("Basic")
        {
        }

        [NotNull]
        public List<BasicUser> Users { get; set; } = new List<BasicUser>();

        [CanBeNull]
        public BasicUser AnonymousUser { get; set; }
    }
}