using System;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using OpenWeatherMap;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mirror1
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly string _city = "Pune";
        public ThreadPoolTimer _timer;

        public MainPage()
        {
            InitializeComponent();
            _timer = ThreadPoolTimer.CreatePeriodicTimer(_timer_Tick, TimeSpan.FromMilliseconds(1000));
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var client = new OpenWeatherMapClient("f106071d5c9f2a0f94eda7fb618ed294");
            var currentWeather = await client.CurrentWeather.GetByName(_city);

            // Celcius to Kelvin is -> -273.15

            textBlock.Text = string.Format("{0}- Humidity = {1} Temperature={2}-{3}  Weather = {4}",
                currentWeather.City.Name, currentWeather.Humidity.Value,
                currentWeather.Temperature.Value - 273.15, "Celcius", currentWeather.Weather.Value);
            var bitmapImage = new BitmapImage();
            if (currentWeather.Weather.Value.ToLower().Contains("few clouds"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/cloudy1.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("clear sky"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/sunny.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("scattered clouds"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/cloudy3.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("broken clouds"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/cloudy5.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("shower rain"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/shower2.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("rain"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/shower3.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("thunderstorm"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/tstorm2.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("mist"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/mist.png"));
            else if (currentWeather.Weather.Value.ToLower().Contains("smoke"))
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/overcast.png"));
        }

        private async void _timer_Tick(ThreadPoolTimer timer)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, () =>
                {
                    // Your UI update code goes here!
                    textBlock1.Text = "";
                    textBlock1.Text = string.Format("{0}",
                        DateTime.Now);
                });
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            var client2 = new OpenWeatherMapClient("f106071d5c9f2a0f94eda7fb618ed294");
            textBlock2.Text = string.Empty;
            var forecast = await client2.Forecast.GetByName(_city);
            var data = forecast.Forecast;
            foreach (var item in data)
            {
                textBlock2.Text += string.Format("{0} -- {1} -- {2} @ {3} to {4}",
                    item.Humidity.Value,
                    item.Temperature.Value, item.Temperature.Unit, item.From, item.To);
                textBlock2.Text += Environment.NewLine;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var calendarData = File.ReadAllLines("holidays.csv");
            foreach (var line in calendarData)
                listView.Items.Add(string.Format("{0} - {1}", line.Split(',')[0], line.Split(',')[1]));
        }
    }
}