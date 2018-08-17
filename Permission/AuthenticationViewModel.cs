using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;

namespace RT
{
    public interface IViewModel { }

    public class AuthenticationViewModel : NotifyUIBase, IViewModel
    {
        public event HandledEventHandler Authenticated;
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        //private readonly DelegateCommand _logoutCommand;
        //private readonly DelegateCommand _showViewCommand;
        private string _username;
        private string _status;
        private string _password;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            //_logoutCommand = new DelegateCommand(Logout, CanLogout);
            //_showViewCommand = new DelegateCommand(ShowView, null);
        }

        #region Properties
        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value;  }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Admin") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }
        
        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }
        //public DelegateCommand LogoutCommand { get { return _logoutCommand; } }
        //public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }
        #endregion

        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;
            try
            {
                //Validate credentials through the authentication service
                //User user = _authenticationService.AuthenticateUser(Username, clearTextPassword);
                User user = _authenticationService.AuthenticateUser(Username, Password);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles, user.AutoLogoutTime, user.Pages);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                //_loginCommand.RaiseCanExecuteChanged();
                //_logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                Password = string.Empty;
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
            }

            catch (UnauthorizedAccessException)
            {
                Status = "Login failed, please check your user id and password.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
            finally
            {
                if (Authenticated != null) Authenticated(IsAuthenticated, null);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        public void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                //_loginCommand.RaiseCanExecuteChanged();
                //_logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;
                //IView view;
                //if (parameter != null)
                //    view = new MainWindow();
                //else
                //    view = new AdminWindow();

                //view.Show();
            }
            catch (SecurityException)
            {
                Status = "You are not authorized!";
            }
        }
    }
}
