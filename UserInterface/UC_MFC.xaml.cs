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
    public partial class UC_MFC : UserControl
    {
        public UC_MFC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ucGasNameProperty = DependencyProperty.Register("ucGasName", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));
        public string ucGasName
        {
            get { return (string)this.GetValue(ucGasNameProperty); }
            set { 
                this.SetValue(ucGasNameProperty, value);
                this.iName.Content = value;
            }
        }

        public static readonly DependencyProperty ucGasSetpointProperty = DependencyProperty.Register("ucGasSetpoint", typeof(double), typeof(MessageListener), new UIPropertyMetadata(null));
        public double ucGasSetpoint
        {
            get { return (double)this.GetValue(ucGasSetpointProperty); }
            set
            {
                this.SetValue(ucGasSetpointProperty, value);
                this.iSetPoint.Content = value;
            }
        }

        public static readonly DependencyProperty ucGasReadValueProperty = DependencyProperty.Register("ucGasReadValue", typeof(double), typeof(MessageListener), new UIPropertyMetadata(null));
        public double ucGasReadValue
        {
            get { return (double)this.GetValue(ucGasReadValueProperty); }
            set
            {
                this.SetValue(ucGasReadValueProperty, value);
                this.iReadValue.Content = value;
            }
        }
    }
}
