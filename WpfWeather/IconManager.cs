using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfWeather {
	public class IconManager {
		public static BitmapImage IconCloud(string coverType, string time) {
			string iC;
			BitmapImage iconCloud;
			switch (coverType) {
				case "Mostly Clear":
					iC = string.Format("Clouds/{0}CloudFew");
					break;
				case "Partly Cloudy":
					iC = string.Format("Clouds/{0}CloudScattered", time);
					break;
				case "Mostly Cloudy":
					iC = string.Format("Clouds/{0}CloudBroken", time);
					break;
				case "Overcast":
					iC = string.Format("Clouds/{0}CloudOvercast", time);
					break;
				default: // no clouds
					iC = "Misc/Blank";
					break;
			}
			iconCloud = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/{iC}.png"));
			return iconCloud;
		}

		public static BitmapImage IconSunMoon(string daytime) {
			string iT;
			BitmapImage iconSunMoon;
			if (daytime == "Night") {
				iT = "Moon";
			} else {
				iT = "Sun";
			}
			string iwuuw = $"*/Assets/Icons/Sun and Moon/{iT}.png";
			iconSunMoon = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/Sun and Moon/{iT}.png"));
			return iconSunMoon;
		}

		public static BitmapImage IconPrecip(string precipDesc) {
			string iP;
			BitmapImage iconPrecip;
			switch (precipDesc) {
				case "Light Rain": // light rain
					iP = "Precipitation/RainLight";
					break;
				case "Heavy Rain": // heavy rain
					iP = "Precipitation/RainHeavy";
					break;
				case "Light Snow": // light snow
					iP = "Precipitation/SnowLight";
					break;
				case "Heavy Snow": // heavy snow
					iP = "Precipitation/SnowHeavy";
					break;
				default:
					iP = "Misc/Blank";
					break;
			}
			iconPrecip = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/{iP}.png"));
			// set the precip icon to the snowflake in case of snow as well
			return iconPrecip;
		}

		public static BitmapImage IconOther(int condition) {
			string iO;
			BitmapImage iconOther;
			if (condition >= 200 && condition < 300) { // 200-299 is Thunderstorm, set Lightning Icon
				iO = "Other/Lightning";
			} else if (condition >= 700 && condition < 800) { // 700-799 is Atmospheric conditions, set MistIcon
				iO = "Other/Mist";
			} else { // no extra icons needed, set blank icons
				iO = "Misc/Blank";
			}
			iconOther = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/{iO}.png"));
			return iconOther;
		}
	}
}
