using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        App _main = ((App)Application.Current);
        public LogIn(AuthenticationViewModel _viewMode)
        {
            ViewModel = _viewMode;
            InitializeComponent();
            //LoginImage.Source = new BitmapImage(new Uri(string.Format("\\Images\\cubes.png",System.Environment.CurrentDirectory), UriKind.RelativeOrAbsolute));
            //LoginImage.Source = new BitmapImage(new Uri("cubes.png", UriKind.Relative));
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var text = ((sender as ComboBox).SelectedItem as ComboBoxItem).Name as string;
            switch (text)
            {
                default:
                case "English":
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    break;
                case "Japanese":
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ja");
                    break;
                case "ChineseSimplified":
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                    break;
                case "ChineseTraditional":
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-tw");
                    break;
            }
            ((App)Application.Current).SetLanguageDictionary(); 
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue == true)
            //{
            CommFeature.Instance.UpdateRTXml();
            //}
        }

        public class ImageConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                    return null;

                //Your incoming ToolImage object
                var image = (System.Drawing.Image)value;

                //new-up a BitmapImage
                var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();

                //stream image info from Image to BitmapImage
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();

                //return the BitmapImage that you can bind to the XAML
                return bitmap;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }
    }
}
