//using LiveCharts;
//using LiveCharts.Defaults;
//using LiveCharts.Wpf;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RT
{
    /// <summary>
    /// Interaction logic for EndPoint.xaml
    /// </summary>
    public partial class EndPoint : Window
    {
        //SeriesCollection _collect = new SeriesCollection();
        public EndPoint()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //Chart1.SeriesCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Values = new ChartValues<ObservableValue>
            //        {
            //            new ObservableValue(3),
            //            new ObservableValue(5),
            //            new ObservableValue(2),
            //            new ObservableValue(7),
            //            new ObservableValue(7),
            //            new ObservableValue(17),
            //            new ObservableValue(27),
            //            new ObservableValue(20),
            //            new ObservableValue(26),
            //            new ObservableValue(15),
            //            new ObservableValue(4)
            //        },
            //        PointGeometrySize = 0,
            //        StrokeThickness = 4,
            //        Fill = Brushes.Transparent
            //    },
            //    new LineSeries
            //    {
            //        Values = new ChartValues<ObservableValue>
            //        {
            //            new ObservableValue(3),
            //            new ObservableValue(4),
            //            new ObservableValue(6),
            //            new ObservableValue(8),
            //            new ObservableValue(7),
            //            new ObservableValue(5)
            //        },
            //        PointGeometrySize = 0,
            //        StrokeThickness = 4,
            //        Fill = Brushes.Transparent
            //    }
            //};
        }
    }
}
