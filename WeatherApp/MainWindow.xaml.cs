using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;


namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly string apiKey = "71ca44faebb420b85887b2a70da1f540";
        private string requestUrl = "https://api.openweathermap.org/data/2.5/weather?lang=de&units=metric";

        public MainWindow()
        {
            InitializeComponent();

            UpdateUI();
        }


        public void UpdateUI()
        {
            string city = textBoxCity.Text;

            WeatherAppResponse result = GetWeatherData(city);
     
            //City found
            if(result.main != null)
            {
                string currentWeather = result.weather[0].main.ToLower();

                string backgroundImage = "Images/Sun.png";

                if (currentWeather.Contains("clouds") || currentWeather.Contains("fog") || currentWeather.Contains("mist"))
                {
                    backgroundImage = "Images/Cloud.png";
                }
                else if (currentWeather.Contains("rain") || currentWeather.Contains("drizzle"))
                {
                    backgroundImage = "Images/Rain.png";
                }
                else if (currentWeather.Contains("snow"))
                {
                    backgroundImage = "Images/Snow.png";
                }

                string countryIcon = "Images/Countries/" + result.sys.country.ToLower() + ".png";

                this.backgroundImage.ImageSource = new BitmapImage(new Uri(backgroundImage, UriKind.Relative));

                labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
                labelFeelsLike.Content = "(gefühlt " + result.main.feels_like.ToString("F1") + "°C)";
                labelInfo.Content = result.weather[0].description;
                imageCountry.Source = new BitmapImage(new Uri(countryIcon, UriKind.Relative));

            } else
            {
                labelTemperature.Content = "";
                labelFeelsLike.Content = "";
                labelInfo.Content = "Keine Daten gefunden";
             
            }

        }

        public WeatherAppResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();

            var finalUri = requestUrl + "&q=" + city + "&appid="+apiKey;

            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;

            string response = httpResponse.Content.ReadAsStringAsync().Result;

            WeatherAppResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherAppResponse>(response);

            return weatherMapResponse;
        }


        /***********************************************************************************************/
        /* Event-Functions
        /***********************************************************************************************/

        private void ButtonSearchWeather(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void KeyDownSearchWeather(object sender, KeyEventArgs e)
        {
            if(e.Key.ToString() == "Return")
            {
                UpdateUI();
            }
        }
    }
}
