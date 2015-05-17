using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TubeyForWin3
{
    /// <credits>
    /// TubeyNet (Known internally as TubeyForWin3)
    /// Thank you to the amazing people that helped me get this project off the ground.
    /// Without you TubeyNet Wouldn't have been made possible.
    /// mtj23 (Thanks for helping me setup the classes, basic logic of WPF and contiune to inspire me!)
    /// XVar (Thanks for the WPF magic you did with allowing me to display only certain collums in color codes.)
    /// Suchiman - For making the timing module suck less. I did it for Nic Cage!
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
            var client = new HttpClient();

            Status.IsEnabled = true;
            Status.Content = "Getting Latest Data";

            var response = await client.GetAsync(new Uri("https://api.tfl.gov.uk/line/mode/tube,overground,dlr,tram,national-rail,cable-car,river-bus,river-tour/status"));
            var content = await response.Content.ReadAsStringAsync(); //read json string Async.

            Status.Content = "Updated:" + DateTime.Now.ToString(); //Status is green, prepair for conversion.

            var LineDisplayCollection = JsonConvert.DeserializeObject<IEnumerable<LineDisplay>>(content); // This is going to be an ObservableCollection now
            Grid1.ItemsSource = LineDisplayCollection; //bind the data to the grid.
            Grid1.IsEnabled = true; //enable the grid for user to see.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }

        public bool handle = true;

        private void wombo_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) Handle();
            handle = true;
        }

        private void wombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen; //if the object has been opened.
            Handle(); //switch logic. what am i grabbing?
        }

        private void Handle()
        {
            string selection = wombo.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            if (selection == "off")
            {
                ConfigureTimer(0);
                return;
            }

            int intervall = Int32.Parse(selection.Remove(2).TrimEnd());
            ConfigureTimer(intervall);
            Get();
        }

        private DispatcherTimer UpdateTimer;

        private void ConfigureTimer(int minuteIntervall)
        {
            //initialize the timer if it didn't happen before
            if (UpdateTimer == null)
            {
                UpdateTimer = new DispatcherTimer();
                UpdateTimer.Tick += (sender, e) => Get();
            }

            //if the timer is running stop it
            if (UpdateTimer.IsEnabled)
            {
                UpdateTimer.Stop();
            }

            //if no intervall is requested quit here since the timer is already stopped
            if (minuteIntervall <= 0)
            {
                return;
            }

            //configure new intervall and restart timer
            UpdateTimer.Interval = TimeSpan.FromMinutes(minuteIntervall);
            UpdateTimer.Start();
        }
    }
}
