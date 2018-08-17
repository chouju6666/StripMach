using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using WPF.MDI;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace RT
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        App _main = ((App)Application.Current);
        /// <summary>
        /// Log out timer, minute
        /// </summary>
        int logOffTime = 1;
        Timer aTimer = null;
        delegate void TimerDispatcherDelegate();

        public MainWindow()
        {
            InitializeComponent();
            _main.SetLanguageDictionary();
            this.Loaded -= new RoutedEventHandler(Window_Loaded);
            this.Closed -= new EventHandler(Window_Closed);
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            this.Closed += new EventHandler(Window_Closed);
            this.SizeChanged -= MainWindow_SizeChanged;
            this.SizeChanged += MainWindow_SizeChanged;
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource windowSpecificOSMessageListener = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            windowSpecificOSMessageListener.AddHook(new HwndSourceHook(CallBackMethod));
            AutoLogOffHelper.LogOffTime = logOffTime;
            AutoLogOffHelper.MakeAutoLogOffEvent -= new AutoLogOffHelper.MakeAutoLogOff(LogOffEvent);
            AutoLogOffHelper.MakeAutoLogOffEvent += new AutoLogOffHelper.MakeAutoLogOff(LogOffEvent);
            AutoLogOffHelper.StartAutoLogoffOption();
            UserName.Header = _main.mainCustomPrincipal.Identity.Name;

            aTimer = new Timer(800);
            aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;

            // Add hook communication with Real Time Control
            HwndSource hWndSource;
            WindowInteropHelper wih = new WindowInteropHelper(this);
            hWndSource = HwndSource.FromHwnd(wih.Handle);
            //添加處理進程 
            hWndSource.AddHook(Win32.WndProc);
        }        

        private void OnTimedEvent(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new TimerDispatcherDelegate(updateUI));
        }

        void updateUI()
        {
            NowTime.Header = "Now Time : " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private IntPtr CallBackMethod(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //  Listening OS message to test whether it is a user activity
            if ((msg >= 0x0200 && msg <= 0x020A) || (msg <= 0x0106 && msg >= 0x00A0) || msg == 0x0021)
            {
                AutoLogOffHelper.ResetLogoffTimer();
            }
            return IntPtr.Zero;
        }

        void LogOffEvent()
        {
            _main.mainCustomPrincipal.Identity = new AnonymousIdentity();
            this.CloseAllWindows();
            this.Hide();
            if (_main.mainlogin != null)
            {
                _main.mainlogin.Hide();
                _main.mainlogin.ShowDialog();
            }
        }

        #region Theme Menu Events

        /// <summary>
        /// Handles the Click event of the Generic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Generic_Click(object sender, RoutedEventArgs e)
        {
            Container.Theme = ThemeType.Generic;
        }

        /// <summary>
        /// Handles the Click event of the Luna control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Luna_Click(object sender, RoutedEventArgs e)
        {

            Container.Theme = ThemeType.Luna;
        }

        /// <summary>
        /// Handles the Click event of the Aero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Aero_Click(object sender, RoutedEventArgs e)
        {

            Container.Theme = ThemeType.Aero;
        }

        private void ChangeLayout_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header.ToString())
            {
                case "Cascade":
                    Container.MdiLayout = MdiLayout.Cascade;
                    break;
                case "TileHorizontal":
                    Container.MdiLayout = MdiLayout.TileHorizontal;
                    break;
                case "TileVertical":
                    Container.MdiLayout = MdiLayout.TileVertical;
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the 'Normal window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {
            var ItemName = (sender as MenuItem).Name;
            if (ItemName == "LogOut")
            {
                //CloseAllWindows();
                LogOffEvent();
                return;
            }
            if (ItemName == "Exit")
            {
                if (MessageBox.Show("Are you sure to shutdown GUI ?", "Warning", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
                Application.Current.Shutdown();
                return;
            }

            var _permission = _main.mainCustomPrincipal.Identity.Pages.Where(r => r.pagename == ItemName && int.Parse(r.authority.ToString()) > 0).FirstOrDefault();
            if (_permission == null) { MessageBox.Show(string.Format("You do not have permission of {0}", ItemName), "Warning"); return; }

            if (Container.Children.Cast<MdiChild>().Where(p => p.Title == ItemName).Count() > 0)
            {
                Container.Children.Cast<MdiChild>().Where(p => p.Title == ItemName).First().Focused = true;
                return;
            }
            Window window = null;
            switch (ItemName)
            {
                default:
                case "OverView":
                    window = new OverView();
                    break;
                case "Chamber":
                    window = new Chamber();
                    break;
                case "TM_LL":
                    window = new TM_LL();
                    break;
                case "EndPoint":
                    window = new EndPoint();
                    break;
                case "RecipeEditor":
                    window = new RecipeEditor();
                    break;
                case "SequenceEditor":
                    window = new SequenceEditor();
                    break;
                case "RecipeConfig":
                    window = new RecipeConfig();
                    break;
                case "EC":
                    window = new EC();
                    break;
                case "LeakCheck":
                    window = new LeakCheck();
                    break;
                case "RFCalibration":
                    window = new RFCalibration();
                    break;
                case "GasVerification":
                    window = new GasVerification();
                    break;
                case "IOList":
                    window = new IOList();
                    break;
                case "UserList":
                    window = new UserList();
                    break;
                case "AccRole":
                    window = new AccRole();
                    break;
                case "About":
                    window = new OverView();
                    break;
            }

            Container.Children.Add(new MdiChild()
            {
                Title = window.Title,
                Content = window.Content as UIElement,
                Height = (System.Windows.SystemParameters.PrimaryScreenHeight / 1.5),
                Width = (System.Windows.SystemParameters.PrimaryScreenWidth / 1.5),
                //Name = window.Title,
                //Resizable = true,
                //Margin = new Thickness((ActualWidth-this.ActualWidth)/2, (ActualHeight-this.ActualHeight)/2, 0 ,0),
                //Width = 800,
                //Height = 500,
                //WindowState = System.Windows.WindowState.Maximized
            });
        }

        private void CloseAllWindows()
        {
            List<MdiChild> Listfrms = new List<MdiChild>();

            foreach (MdiChild frm in this.Container.Children)
            {
                Listfrms.Add(frm);
            }

            foreach (MdiChild frm2 in Listfrms)
            {
                string name = frm2.Title;
                if (name == "MainWindow" || name == "LogIn") continue;
                if (frm2.Visibility == System.Windows.Visibility.Visible)
                {
                    //frm.Visibility = System.Windows.Visibility.Hidden;
                    this.Container.Children.Remove(frm2);
                    frm2.Close();
                }
            }
        }

        private void Container_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                AutoLogOffHelper.EnableLogoffTimer();
                //MenuItem _startPage = new MenuItem();
                //_startPage.Name = "OverView";
                //AddWindow_Click(_startPage, null);
            }
            else
                AutoLogOffHelper.DisableLogoffTimer();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (msgBoxResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
