using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Parses the json data from the API call
/// </summary>
namespace WpfWeather {
	public class WeatherInfo {
		public class Coord {
			public double Lon { get; set; }
			public double Lat { get; set; }
		}

		public class Weather {
			public int Id { get; set; }
			public string Main { get; set; }
			public string Description { get; set; }
		}

		public class Main {
			public double Temp { get; set; }
			public double Feels_Like { get; set; }
			public double Temp_Min { get; set; }
			public double Temp_Max { get; set; }
			public double Humidity { get; set; }
		}

		public class Wind {
			public double Speed { get; set; }
			public double Deg { get; set; }
		}

		public class Rain {
			[JsonProperty(PropertyName = "1h")]
			public double Hour1 { get; set; }
		}

		public class Snow {
			[JsonProperty(PropertyName = "1h")]
			public double Hour1 { get; set; }
		}

		public class Clouds {
			public double All { get; set; }
		}

		public class Sys {
			public string Country { get; set; }
			public double Sunrise { get; set; }
			public double Sunset { get; set; }
		}

		public class Root {
			public string Name { get; set; }
			public double Dt { get; set; }
			public double Timezone { get; set; }
			public Coord Coord { get; set; }
			public List<Weather> Weather { get; set; }
			public Main Main { get; set; }
			public Wind Wind { get; set; }
			public Rain Rain { get; set; }
			public Snow Snow { get; set; }
			public Clouds Clouds { get; set; }
			public Sys Sys { get; set; }
		}
	}
}
