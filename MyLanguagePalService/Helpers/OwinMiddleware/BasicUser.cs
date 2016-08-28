using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.Helpers.OwinMiddleware
{
    public class BasicUser
    {
        [CanBeNull]
        public string Login { get; set; }

        [CanBeNull]
        public string Password { get; set; }

        [CanBeNull]
        public List<string> Roles { get; set; }
    }
}