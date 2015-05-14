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
    /// <credits>
    /// TubeyNet (Known internally as TubeyForWin3)
    /// Thank you to the amazing people that helped me get this project off the ground.
    /// Without you TubeyNet Wouldn't have been made possible.
    /// mtj23 (Thanks for helping me setup the classes, basic logic of WPF and contiune to inspire me!)
    /// XVar (Thanks for the WPF magic you did with allowing me to display only certain collums in color codes.)
    /// Developed with <3 by OliPicard - github.com/olipicard/
    /// </credits>
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
            var client = new HttpClient(); //spin up a new HttpClient
            string display = ("Getting Latest Data"); //just a string..
            DateTime clockStatus = DateTime.Now; //what's the time?
            string ok = ("Updated:" + clockStatus); //ok string includes timestamp
            Status.IsEnabled = true; //enable the Status Label
            Status.Content = display; //Initialize request
            var response = await client.GetAsync(new Uri("https://api.tfl.gov.uk/line/mode/tube,overground,dlr,tram,national-rail,cable-car,river-bus,river-tour/status"));
            var content = await response.Content.ReadAsStringAsync(); //read json string Async.
            Status.Content = ok; //Status is green, prepair for conversion.
            var LineDisplayCollection = JsonConvert.DeserializeObject<IEnumerable<LineDisplay>>(content); // This is going to be an ObservableCollection now
            Grid1.ItemsSource = LineDisplayCollection; //bind the data to the grid.
            Grid1.IsEnabled = true; //enable the grid for user to see.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }
    }
}
