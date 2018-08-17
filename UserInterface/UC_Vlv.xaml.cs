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
    public partial class UC_Vlv : UserControl
    {
        public UC_Vlv()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ucVlvGasNameProperty = DependencyProperty.Register("ucVlvGasName", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));
        public string ucVlvGasName
        {
            get { return (string)this.GetValue(ucVlvGasNameProperty); }
            set {
                this.SetValue(ucVlvGasNameProperty, value);
                this.iName.Content = value;
            }
        }

        public static readonly DependencyProperty ucVlvGasName2Property = DependencyProperty.Register("ucVlvGasName2", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));
        public string ucVlvGasName2
        {
            get { return (string)this.GetValue(ucVlvGasName2Property); }
            set
            {
                this.SetValue(ucVlvGasName2Property, value);
                this.iName2.Content = value;
            }
        }

        public static readonly DependencyProperty ucVlvColorProperty = DependencyProperty.Register("ucVlvColor", typeof(SolidColorBrush), typeof(MessageListener), new UIPropertyMetadata(null));
        public SolidColorBrush ucVlvColor
        {
            get { return (SolidColorBrush)this.GetValue(ucVlvColorProperty); }
            set
            {
                this.SetValue(ucVlvColorProperty, value);
                this.iBorderColor.Background = value;
            }
        }
    }
}
