using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RT
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string _name, string _email, string[] _roles, string _AutoLogoutTime, List<AuthenticationService.InternalPage> _pages)
        {
            Name = _name;
            Email = _email;
            Roles = _roles;
            Pages = _pages;
            AutoLogoutTime = _AutoLogoutTime;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }
        public string AutoLogoutTime { get; private set; }
        public List<AuthenticationService.InternalPage> Pages { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}
