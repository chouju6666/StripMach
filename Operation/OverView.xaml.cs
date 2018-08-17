using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RT
{
    /// <summary>
    /// Interaction logic for OverView.xaml
    /// </summary>
    public partial class OverView : Window
    {
        App _main = ((App)Application.Current);
        public OverView()
        {
            InitializeComponent();
            _main.SetLanguageDictionary();
            
            UC_MFC1.ucGasName = "O2";
            UC_MFC2.ucGasName = "N2";
            UC_MFC3.ucGasName = "N2";
            UC_MFC4.ucGasName = "CF4";
            UC_MFC5.ucGasName = "O2";
            UC_MFC6.ucGasName = "C2F6";
            UC_MFC7.ucGasName = "N2";
            UC_MFC8.ucGasName = "CF4";

            UC_Vlv1.ucVlvGasName = "CM";
            UC_Vlv1.ucVlvGasName2 = "";

            UC_Vlv2.ucVlvGasName = "N2";
            UC_Vlv2.ucVlvGasName2 = "BL";

            UC_Vlv3.ucVlvGasName = "N2";
            UC_Vlv3.ucVlvGasName2 = "LB";

            UC_Vlv4.ucVlvGasName = "N2";
            UC_Vlv4.ucVlvGasName2 = "SB";

            UC_Vlv5.ucVlvGasName = "Vac";
            UC_Vlv5.ucVlvGasName2 = "SV";

            UC_Vlv2_1.ucVlvGasName = "Vac";
            UC_Vlv2_1.ucVlvGasName2 = "CV";

            UC_Vlv2_2.ucVlvGasName = "0 0";
            UC_Vlv2_2.ucVlvGasName2 = "TV";
            UC_Vlv2_2.ucVlvColor = new SolidColorBrush(Color.FromRgb(255, 0, 255));

            UC_Vlv2_3.ucVlvGasName = "0 0";
            UC_Vlv2_3.ucVlvGasName2 = "TV";
            UC_Vlv2_3.ucVlvColor = new SolidColorBrush(Color.FromRgb(255, 0, 255));

            UC_Vlv2_4.ucVlvGasName = "Vac";
            UC_Vlv2_4.ucVlvGasName2 = "CV";

            InCassette.ucCassetteStatus = "33333333300000";
            OutCassette.ucCassetteStatus = "1111222222";

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            //ZoomIn(1.5);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            labEventTime.Content = DateTime.Now.ToString("ddd MM dd HH:mm:ss yyyy");
            labNowTime.Content = DateTime.Now.ToString("ddd MM dd HH:mm:ss yyyy");

            var rnd = new Random(DateTime.Now.Millisecond);
            int ticks = rnd.Next(0, 3000);
            UC_MFC1.ucGasSetpoint = ticks + 20;
            UC_MFC1.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC2.ucGasSetpoint = ticks + 20;
            UC_MFC2.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC3.ucGasSetpoint = ticks + 20;
            UC_MFC3.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC4.ucGasSetpoint = ticks + 20;
            UC_MFC4.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC5.ucGasSetpoint = ticks + 20;
            UC_MFC5.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC6.ucGasSetpoint = ticks + 20;
            UC_MFC6.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC7.ucGasSetpoint = ticks + 20;
            UC_MFC7.ucGasReadValue = ticks;

            ticks = rnd.Next(0, 3000);
            UC_MFC8.ucGasSetpoint = ticks + 20;
            UC_MFC8.ucGasReadValue = ticks;
        }

        private void ZoomIn(double zoomValue)
        {
            this.GridPanel.LayoutTransform = new ScaleTransform(zoomValue, zoomValue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MdiChild_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
