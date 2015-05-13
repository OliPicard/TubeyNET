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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;

namespace TubeyForWin3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Get();
        }
        async private void Get()
        {
            var client = new HttpClient();
            string display = ("Getting Latest Data");
            DateTime clockStatus = DateTime.Now;
            string ok = ("Updated:" + clockStatus);
            Status.IsEnabled = true;
            Status.Content = display;
            var response = await client.GetAsync(new Uri("https://api.tfl.gov.uk/line/mode/tube,overground,dlr,tram,national-rail,cable-car,river-bus,river-tour/status"));
            var content = await response.Content.ReadAsStringAsync();
            Status.Content = ok;
            // This is going to be an ObservableCollection now
            var LineDisplayCollection = JsonConvert.DeserializeObject<IEnumerable<LineDisplay>>(content);
            Grid1.ItemsSource = LineDisplayCollection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }
    }
}
