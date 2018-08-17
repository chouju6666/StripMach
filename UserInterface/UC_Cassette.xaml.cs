using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RT.UserInterface
{
    /// <summary>
    /// Interaction logic for UC_Line.xaml
    /// </summary>
    public partial class UC_Cassette : UserControl
    {
        private string _CassetteStatus;
        private List<char> _StrStatus = new List<char>();
        public UC_Cassette()
        {
            InitializeComponent();
            //ucCassetteStatus = "0".PadRight(25,'0');
        }

        public static readonly DependencyProperty ucCassetteNameProperty = DependencyProperty.Register("ucCassetteName", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));
        public string ucCassetteName
        {
            get { return (string)this.GetValue(ucCassetteNameProperty); }
            set {
                this.SetValue(ucCassetteNameProperty, value);
                this.iName.Content = value;
            }
        }

        public static readonly DependencyProperty ucCassetteStatusProperty = DependencyProperty.Register("ucCassetteStatus", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));
        public string ucCassetteStatus
        {
            get { return (string)this.GetValue(ucCassetteStatusProperty); }
            set
            {
                _StrStatus.Clear();
                _CassetteStatus = value.Length < 25 ? value.PadRight(25, '0') : value.Substring(0,25);
                int i = 0 ;
                foreach (char str in _CassetteStatus)
                {
                    ((Border)(this.FindName(string.Format("iBorderColor3_Copy{0}",i)))).Background = GetColor(str.ToString());
                    i ++;
                }

                this.SetValue(ucCassetteStatusProperty, _CassetteStatus);
            }
        }

        Brush GetColor(string Status)
        {
            switch (Status)
            {
                    // No wafer
                case "0":
                    return Brushes.White;
                    // has wafer
                case "1":
                    return Brushes.Gray;
                    // associated recipe
                case "2":
                    return Brushes.Black;
                    // Processing
                case "3":
                    return Brushes.LightGreen;
                    // Processed
                case "4":
                    return Brushes.BlueViolet;
                    // Wafer has warning 
                case "8":
                    return Brushes.Yellow;
                    // Wafer has error
                case "9":
                    return Brushes.Red;
                default :
                    return Brushes.White;
            }
        }
    }
}
