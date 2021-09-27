using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace WpfWeather {
	/// <summary>
	/// Recieves city name from user, returns weather data. Can change units from metric to imperial. Weather icons change based on description.
	/// </summary>
	public partial class MainWindow : Window {
		// API Call Input
		const string key = ""; // move to external json file
		public string city; 
		public string units;

		// unit conversion
		public static string temp;
		public static string speed;

		public MainWindow() {
			InitializeComponent();
			StackPanel_Main.Visibility = Visibility.Collapsed;
		}
		
		public void Search(string citySearch) {
			SetUnits();
			using (WebClient wC = new WebClient()) {
				string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid=2dd01c4ddc1ad2bd353ce7f2f01e910f&units={1}", citySearch, units);
				try {
					string json = wC.DownloadString(url);
					// export api key to external json file
					SetData(json);
				} catch (WebException) {
					string errorMessage = string.Format("Invalid City Input");
					Epic_Fail(errorMessage);
				}
			}
		}

		public void SetData(string json) {
			JsonSerializerSettings settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore
			};
			WeatherInfo.Root info = JsonConvert.DeserializeObject<WeatherInfo.Root>(json, settings);

			double Snowfall() {
				double snow = info.Snow == null ? 0 : info.Snow.Hour1;
				return snow;
			}
			double Rainfall() {
				double rain = info.Rain == null ? 0 : info.Rain.Hour1;
				return rain;
			}
			string timeOfDay = DataManager.TimeOfDay(info.Dt, info.Sys.Sunrise, info.Sys.Sunset);
			string cloudCover = DataManager.CloudCover(info.Clouds.All);
			string precipDesc = DataManager.PrecipDesc(Rainfall(), Snowfall());
			double PrecipAmount() { // descides whether to tell rain or snow
				if (DataManager.smallPrecipIcon == "Snow") {
					return Snowfall();
				} else {
					return Rainfall();
				}
			}

			Label_Temp.Content = string.Format("{0}{1}", Math.Round(info.Main.Temp), temp);
			Label_TempHighLow.Content = string.Format("H: {0}{1} L: {2}{1}", Math.Round(info.Main.Temp_Max), temp, Math.Round(info.Main.Temp_Min));
			Label_CityName.Content = string.Format("{0}, {1}", info.Name, info.Sys.Country);
			Label_Coordinates.Content = string.Format("({0}, {1})", info.Coord.Lat, info.Coord.Lon);
			Label_Time.Content = string.Format("{0:ddd d MMM, hh:mm tt}", DataManager.UnixConvert(info.Dt, info.Timezone));
			Label_Desc.Content = string.Format("{0}", info.Weather[0].Main);
			Label_Feels.Content = string.Format("Feels like {0}{1}", Math.Round(info.Main.Feels_Like), temp);
			Label_Wind.Content = string.Format("{0}{1}, {2}", Math.Round(info.Wind.Speed), speed, DataManager.Compass(info.Wind.Deg));
			Label_Cloud.Content = string.Format("{0}, {1}%", DataManager.CloudCover(info.Clouds.All), info.Clouds.All);
			Label_Precip.Content = string.Format("{0}, {1}mm/h", precipDesc, PrecipAmount().ToString());
			Label_Humidity.Content = string.Format("Humidity: {0}%", Math.Round(info.Main.Humidity));
			Label_Sunrise.Content = string.Format("Sunrise: {0:hh:mm tt}", DataManager.UnixConvert(info.Sys.Sunrise, info.Timezone));
			Label_Sunset.Content = string.Format("Sunset: {0:hh:mm tt}", DataManager.UnixConvert(info.Sys.Sunset, info.Timezone));


			Icon_SunMoon.Source = IconManager.IconSunMoon(timeOfDay);
			Icon_Cloud.Source = IconManager.IconCloud(cloudCover, timeOfDay);
			Icon_Precip.Source = IconManager.IconPrecip(precipDesc);
			Icon_Other.Source = IconManager.IconOther(info.Weather[0].Id);
			Icon_PrecipLevel.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/Misc/{DataManager.smallPrecipIcon}.png"));

			if (StackPanel_Main.Visibility == Visibility.Collapsed) {
				StackPanel_Main.Visibility = Visibility.Visible;
			}
		}

		private void SetUnits() {
			if (!Button_C.IsEnabled) {
				units = "metric";
				temp = "C";
				speed = "m/s";
			}
			if (!Button_F.IsEnabled) {
				units = "imperial";
				temp = "F";
				speed = "mph";
			}
		}

		public void Epic_Fail(string errorMessage) {
			MessageBoxResult fail = MessageBox.Show(errorMessage, "Fail", MessageBoxButton.OK, MessageBoxImage.Warning);
			if (fail == MessageBoxResult.OK) {
				TextBox_CitySearch.Clear();
				StackPanel_Main.Visibility = Visibility.Collapsed;
			}
		}

		private void Button_CitySearch_Click(object sender, RoutedEventArgs e) {
			city = TextBox_CitySearch.Text;
			CityCheck();
		}

		private void Button_F_Click(object sender, RoutedEventArgs e) {
			Button_C.IsEnabled = true;
			Button_F.IsEnabled = false;
			CityCheck();
		}

		private void Button_C_Click(object sender, RoutedEventArgs e) {
			Button_F.IsEnabled = true;
			Button_C.IsEnabled = false;
			CityCheck();
		}
		private void CityCheck() { // checks for a blank search bar
			if (city == null) {
				Epic_Fail("Please enter a city");
			} else {
				Search(city);
			}
		}

		private void Button_Close_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		private void Button_Minimize_Click(object sender, RoutedEventArgs e) {
			WindowState = WindowState.Minimized;
		}
	}
}
