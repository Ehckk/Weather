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

/// <summary>
/// Takes the data from the API and rearranges it in a more presentable manner. Also sets weather icons.
/// </summary>
namespace WpfWeather {
	public class DataManager {
		public static string smallPrecipIcon; // changes the little precip icon next to precip info

		public static DateTime UnixConvert(double t, double tTimezone) { // takes unix string for time + timezone offset, converts it to readable datetime
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0); // initialize to epoch 
			dt = dt.AddSeconds(t + tTimezone); // adds the modified unix value
			return dt;
		}

		public static string Compass(double windDegrees) { // takes integer between 0-359, returns a wind heading
			string[] direction = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" }; // array containing all 8 headings
			int heading = (int)windDegrees / 45; // integer division
			string windDirection = direction[heading]; // uses said int to choose the corresponding heading
			return windDirection;
		}

		public static string TimeOfDay(double t, double tSunrise, double tSunset) { // checks the current time in relation to sunrise/sunset times, returns night/day
			string timeOfDay;
			if ((t < tSunrise) || (t >= tSunset)) { // before sunrise or past sunset 
				timeOfDay = "Night";
				return timeOfDay;
			} else { //  between sunrise or sunset
				timeOfDay = "Day";
				return timeOfDay;
			}
		}

		public static string PrecipDesc(double rain, double snow) { // takes rainfall/snowfall, returns a better description 
			string desc;
			snow = snow * 1.2d; // estimates total snowfall for the next 12 hours, converts mm to cm.
			if ((rain == 0) && (snow == 0)) { // no precip
				desc = "None";
				smallPrecipIcon = "Rain";
			} else if (snow > 0) { // if it is snowing
				smallPrecipIcon = "Snow";
				if (snow < 10) { // less than 10cm of snow in 12 hours
					desc = "Light Snow";
				} else { // more than 10cm of snow in 12 hours
					desc = "Heavy Snow";
				}
			} else { // no snow, instead shows rain
				smallPrecipIcon = "Rain";
				if (rain < 4) { // less than 4mm/h, light rain
					desc = "Light Rain";
				} else { // more than 2mm/h, heavy rain
					desc = "Heavy Rain"; // more than 4mm/h heavy rain
				}
			}
			return desc;
		}

		public static string CloudCover(double c) { // takes cloud cover info, returns cloud info
			string cover;
			if (c <= 10) { // 10% or less - clear sky
				cover = "Clear Sky";
			} else if (c <= 25) { // 11-25% - 'Mostly clear'
				cover = "Mostly Clear";
			} else if (c <= 50) { // 26-50% - "Partly cloudy'
				cover = "Partly Cloudy";
			} else if (c <= 84) { // between 51-84% - 'mostly cloudy'
				cover = "Mostly Cloudy";
			} else { // greater than 85% - 'Overcast'
				cover = "Overcast";
			}
			return cover;
		}
	}
}
