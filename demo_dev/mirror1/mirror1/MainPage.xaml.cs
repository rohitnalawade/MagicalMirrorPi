using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public MainPage()
        {
            this.InitializeComponent();
            _timer = ThreadPoolTimer.CreatePeriodicTimer(_timer_Tick, TimeSpan.FromMilliseconds(1000));
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var client = new OpenWeatherMapClient("f106071d5c9f2a0f94eda7fb618ed294");
            var currentWeather = await client.CurrentWeather.GetByName("Pune");
            textBlock.Text = String.Format("{0}-{1}-{2}-{3}",
                currentWeather.City.Name, currentWeather.Humidity.Value,
                currentWeather.Temperature.Value, currentWeather.Weather.Value);
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
    }
}
