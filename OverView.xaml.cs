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
using WPF.MDI;

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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MdiChild_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
