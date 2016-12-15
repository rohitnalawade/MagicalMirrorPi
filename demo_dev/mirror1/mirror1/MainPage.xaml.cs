using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mirror1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ThreadPoolTimer _timer;
        private string _city = "Pune";
        public MainPage()
        {
            this.InitializeComponent();
            _timer = ThreadPoolTimer.CreatePeriodicTimer(_timer_Tick, TimeSpan.FromMilliseconds(1000));
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var client = new OpenWeatherMapClient("f106071d5c9f2a0f94eda7fb618ed294");
            var currentWeather = await client.CurrentWeather.GetByName(_city);

            // Celcius to Kelvin is -> -273.15

            textBlock.Text = String.Format("{0}- Humidity = {1} Temperature={2}-{3}  Weather = {4}",
                currentWeather.City.Name, currentWeather.Humidity.Value,
                currentWeather.Temperature.Value - 273.15, "Celcius", currentWeather.Weather.Value);
            BitmapImage bitmapImage = new BitmapImage();
            if (currentWeather.Weather.Value.ToLower().Contains("clouds"))
            {
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/clouds.png"));
            }
            else if (currentWeather.Weather.Value.ToUpper().Contains("sun"))
            {
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/sun.png"));
            }
            
        }

        private async void _timer_Tick(ThreadPoolTimer timer)
        {
            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(
             CoreDispatcherPriority.Normal, () =>
             {
                 // Your UI update code goes here!
                 textBlock1.Text = "";
                 textBlock1.Text = String.Format("{0}",
                     DateTime.Now);


             });

        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            var client2 = new OpenWeatherMapClient("f106071d5c9f2a0f94eda7fb618ed294");
            textBlock2.Text = String.Empty;
            var forecast = await client2.Forecast.GetByName(_city);
            var data = forecast.Forecast;
            foreach (var item in data)
            {
                textBlock2.Text += String.Format("{0} -- {1} -- {2} @ {3} to {4}",
                 item.Humidity.Value,
                item.Temperature.Value, item.Temperature.Unit, item.From, item.To);
                textBlock2.Text += Environment.NewLine;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var calendarData = File.ReadAllLines("holidays.csv");
            foreach (var line in calendarData)
            {
                listView.Items.Add(String.Format("{0} - {1}", line.Split(',')[0], line.Split(',')[1]));
            }
        }
    }
}
