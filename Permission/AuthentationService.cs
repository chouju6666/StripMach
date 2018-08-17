using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RT
{
    public interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public class InternalUserData
        {
            public InternalUserData(string _username, string _email, string _hashedPassword, string[] _roles, string _AutoLogoutTime)
            {
                Username = _username;
                Email = _email;
                HashedPassword = _hashedPassword;
                Roles = _roles;
                AutoLogoutTime = _AutoLogoutTime;
            }
            public string Username { get; private set; }
            public string Email { get; private set; }
            public string HashedPassword{ get; private set; }
            public string[] Roles { get; private set; }
            public string AutoLogoutTime { get; private set; }
        }

        public class InternalRoleData
        {
            public InternalRoleData()
            {

            }
            public string role { get; set; }
            public List<InternalPage> pages { get; set; }
        }

        public class InternalPage
        {
            public string authority { get; set; }
            public string pagename { get; set; }
        }

        public static List<InternalRoleData> _roles;

        public static List<InternalUserData> _users ;

        public User AuthenticateUser(string username, string clearTextPassword)
        {
            InternalUserData userData = _users.FirstOrDefault(u => u.Username.Equals(username)
                && u.HashedPassword.Equals(CryptorEngine.Encrypt(clearTextPassword, true)));

            if (userData == null)
                throw new UnauthorizedAccessException("Login failed, please check your user id and password.");

            InternalRoleData RoleData = _roles.FirstOrDefault(u => userData.Roles.Contains(u.role));

            return new User(userData.Username, userData.Email, userData.Roles, userData.AutoLogoutTime, RoleData.pages);
        }
    }

    public class User
    {
        public User(string _username, string _email, string[] _roles,string _AutoLogoutTime, List<AuthenticationService.InternalPage> _pages)
        {
            Username = _username;
            Email = _email;
            Roles = _roles;
            Pages = _pages;
            AutoLogoutTime = _AutoLogoutTime;
        }

        public string Username { get; set; }
        public string Email {get ; set;}
        public string[] Roles { get; set; }
        public string AutoLogoutTime { get; set; }
        public List<AuthenticationService.InternalPage> Pages { get; set; }
    }
}
