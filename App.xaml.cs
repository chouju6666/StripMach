using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using RT.Advanced;

namespace RT
{
    /// <summary>
    /// 
    /// </summary>
    class App : Application
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread()]
        static void Main()
        {
            Splasher.Splash = new SplashScreen();
            Splasher.ShowSplash();
            new App();
        }

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
            // Logger
            Iloggers = new List<csLogger>()
            {
                new csLogger("GUI"),
                new csLogger("")
            };
            MessageListener.Instance.ReceiveMessage(string.Format("Created system log"), 10);

            ICommGUI2RT = new Advanced.csCommunication();
            ICommGUI2RT.DataReceived -= ICommunication_DataReceived;
            ICommGUI2RT.DataReceived += ICommunication_DataReceived;
            ICommGUI2RT.IsDisConnected -= ICommunication_IsDisConnected;
            ICommGUI2RT.IsDisConnected += ICommunication_IsDisConnected;
            ICommGUI2RT.Listen("GUI2RT");

            ICommFA2RT = new Advanced.csCommunication();
            ICommFA2RT.DataReceived -= ICommunication_DataReceived;
            ICommFA2RT.DataReceived += ICommunication_DataReceived;
            ICommFA2RT.IsDisConnected -= ICommunication_IsDisConnected;
            ICommFA2RT.IsDisConnected += ICommunication_IsDisConnected;
            ICommFA2RT.Listen("FA2RT");

            //if (_main != null)
            //    _main.Iloggers.Where(r => r.logName == "GUI").FirstOrDefault().WriteLine(message);

            // User Account
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            mainlogin = new LogIn(viewModel);
            viewModel.Authenticated -= viewModel_Authenticated;
            viewModel.Authenticated += viewModel_Authenticated;
            Splasher.CloseSplash();
            mainlogin.ShowDialog();
        }

        void ICommunication_IsDisConnected(object sender, bool e)
        {
            
        }

        void ICommunication_DataReceived(object sender, string e)
        {
            
        }

        void viewModel_Authenticated(object sender, System.ComponentModel.HandledEventArgs e)
        {
            if ((bool)sender)
            {
                mainlogin.Hide();
                AutoLogOffHelper.ResetLogoffTimer();
                if (mainWindow == null)
                {
                    mainWindow = new MainWindow();
                    mainWindow.ShowDialog();
                }
                else
                    mainWindow.ShowDialog();
            }
        }

        public void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                default:
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
                case "ja":
                    dict.Source = new Uri("..\\Resources\\StringResources.ja.xaml", UriKind.Relative);
                    break;
                case "fr-CA":
                    dict.Source = new Uri("..\\Resources\\StringResource.fr-CA.xaml", UriKind.Relative);
                    break;
                case "zh-TW":
                    dict.Source = new Uri("..\\Resources\\StringResources.zh-TW.xaml", UriKind.Relative);
                    break;
                case "zh-CN":
                    dict.Source = new Uri("..\\Resources\\StringResources.zh-CN.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        public LogIn mainlogin = null;
        public MainWindow mainWindow = null;
        public CommInterface IFA = null;
        public string XmlFile = "RTAccount.xml";
        public XDocument _RTaccount = null;
        public NotifyUIBase INotifyUI = new NotifyUIBase();
        public csCommunication ICommGUI2RT;
        public csCommunication ICommFA2RT;
        public List<csLogger> Iloggers;
        public CustomPrincipal mainCustomPrincipal
        {
            get
            {
                return Thread.CurrentPrincipal as CustomPrincipal; 
            }
        }
    }
}
