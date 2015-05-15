using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
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
            string display = "Getting Latest Data"; //just a string..
            DateTime clockStatus = DateTime.Now; //what's the time?
            string ok = "Updated:" + clockStatus; //ok string includes timestamp
            Status.IsEnabled = true; //enable the Sus Label
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
        public bool handle = true;
        private bool check { get; set; } //setting the bools for timekeeper
        private bool checkten { get; set; }
        private bool checktwenty { get; set; }
        private bool checkthirty { get; set; }        
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
            switch (wombo.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last()) //
            {
                case "off":
                    check = true;
                    checkten = true;
                    checktwenty = true;
                    checkthirty = true;
                    break;
                case "5 Minutes":
                    check = false;
                    checkten = true;
                    checktwenty = true;
                    checkthirty = true;
                    TimeFiveDispatch();
                    break;
                case "10 Minutes":
                    TimeTenDispatch();
                    check = true;
                    checkten = false;
                    checktwenty = true;
                    checkthirty = true;
                    break;
                case "20 Minutes":
                    check = true;
                    checkten = true;
                    checktwenty = false;
                    checkthirty = true;
                    break;
                case "30 Minutes":
                    check = true;
                    checkten = true;
                    checktwenty = true;
                    checkthirty = false;
                    break;

            }

        }
        private int TimerTickCount = 0;
        private void TimeFiveDispatch()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick_Five);
            timer.Start();
        }
        private void Timer_Tick_Five(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            if (++TimerTickCount == 300) //if the timer equals 300 seconds
            {

                if (check == true) //if the timer is true, kill it!
                {
                    timer.Stop();
                    return;
                }
                else
                {
                    timer.Stop(); //stop the timer
                    Get(); //get data                    
                    TimerTickCount = 0; //reset the counter
                    timer.Start(); //start timer.
                }


            }
        }
        int TimerTickTenCount = 0;
        private void TimeTenDispatch()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick_Ten);
            timer.Start();
        }
        private void Timer_Tick_Ten(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            if (++TimerTickTenCount == 600)
            {
                if (checkten == true)
                {
                    timer.Stop();
                    return;
                }
                else
                {
                    timer.Stop();
                    Get();                  
                    TimerTickTenCount = 0;
                    timer.Start(); 
                }
            }
        }
        int TimerTickTwentyCount = 0;
        private void TimeTwenityDispatch()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick_Twenty);
            timer.Start();
        }
        private void Timer_Tick_Twenty(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            if (++TimerTickTwentyCount == 1200) 
            {
                if (checktwenty == true)
                {
                    timer.Stop();
                    return;
                }
                else
                {
                    timer.Stop();
                    Get();                
                    TimerTickTwentyCount = 0;
                    timer.Start(); 
                }
            }
        }
        int TimerTickThirtyCount = 0;
        private void TimeThirtyDispatch()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick_Thrity);
            timer.Start();
        }
        private void Timer_Tick_Thrity(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            if (++TimerTickThirtyCount == 1800) 
            {
                if (checkthirty == true)
                {
                    timer.Stop();
                    return;                
                }                
                else
                {
                    timer.Stop();
                    Get();              
                    TimerTickTwentyCount = 0;
                    timer.Start(); 
                }
            }
        }
    }
}