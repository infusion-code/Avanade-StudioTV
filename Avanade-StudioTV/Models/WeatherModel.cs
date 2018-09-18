using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvanadeStudioTV.Models
{
	//see:    API
	//see: https://www.apixu.com/my/

	#region Forecast objects

	public class WeatherModel
	{
		public Location location { get; set; }
		public Current current { get; set; }
		public Forecast forecast { get; set; }
	}
	public class Location
	{
		public string name { get; set; }
		public string region { get; set; }
		public string country { get; set; }
		public double lat { get; set; }
		public double lon { get; set; }
		public string tz_id { get; set; }
		public int localtime_epoch { get; set; }
		public string localtime { get; set; }
	}

	public class Condition
	{
		public string text { get; set; }
		public string icon { get; set; }
		public int code { get; set; }
	}

	public class Current
	{
		public int last_updated_epoch { get; set; }
		public string last_updated { get; set; }
		public double temp_c { get; set; }
		public double temp_f { get; set; }
		public int is_day { get; set; }
		public Condition condition { get; set; }
		public double wind_mph { get; set; }
		public double wind_kph { get; set; }
		public int wind_degree { get; set; }
		public string wind_dir { get; set; }
		public double pressure_mb { get; set; }
		public double pressure_in { get; set; }
		public double precip_mm { get; set; }
		public double precip_in { get; set; }
		public int humidity { get; set; }
		public int cloud { get; set; }
		public double feelslike_c { get; set; }
		public double feelslike_f { get; set; }
		public double vis_km { get; set; }
		public double vis_miles { get; set; }
	}

	public class Condition2
	{
		public string text { get; set; }
		public string icon { get; set; }
		public int code { get; set; }
	}

	public class Day
	{
		public double maxtemp_c { get; set; }
		public double maxtemp_f { get; set; }
		public double mintemp_c { get; set; }
		public double mintemp_f { get; set; }
		public double avgtemp_c { get; set; }
		public double avgtemp_f { get; set; }
		public double maxwind_mph { get; set; }
		public double maxwind_kph { get; set; }
		public double totalprecip_mm { get; set; }
		public double totalprecip_in { get; set; }
		public double avgvis_km { get; set; }
		public double avgvis_miles { get; set; }
		public double avghumidity { get; set; }
		public Condition2 condition { get; set; }
		public double uv { get; set; }
	}

	public class Astro
	{
		public string sunrise { get; set; }
		public string sunset { get; set; }
		public string moonrise { get; set; }
		public string moonset { get; set; }
	}

	public class Forecastday
	{
		public string date { get; set; }
		public int date_epoch { get; set; }
		public Day day { get; set; }
		public Astro astro { get; set; }
	}

	public class Forecast
	{
		public List<Forecastday> forecastday { get; set; }
	}




	#endregion

	#region DayWeatherModel
	public class DayWeatherModel
	{
		public string Day { get; set; }
		public WeatherType type { get; set; }
		public string MaxTemp { get; set; }

		public string MinTemp { get; set; }
		public string MaxTempC { get; set; }
	}

	public enum WeatherType
	{
		Sunny, Cloudy, Storm
	};

	#endregion


	public class UnixTimestampConverter : Newtonsoft.Json.JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(DateTime);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
			.AddSeconds((long)reader.Value);
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

}

/* EXAMPLE WEATHER CONDITION CODES FROM APX WEATHER API see: http://www.apixu.com/doc/Apixu_weather_conditions.json
 * Note for this app we only have 3 weather types : Sunny, Cloudy, or Story (anything else) -this is a high level status view and  not a complete app:
 * 1000 = Sunny
 * 1003, 1006, 1009 = Cloudy
 * Rest are Storm
 * 
 * [
	{
		"code" : 1000,
		"day" : "Sunny",
		"night" : "Clear",
		"icon" : 113
	},
	{
		"code" : 1003,
		"day" : "Partly cloudy",
		"night" : "Partly cloudy",
		"icon" : 116
	},
	{
		"code" : 1006,
		"day" : "Cloudy",
		"night" : "Cloudy",
		"icon" : 119
	},
	{
		"code" : 1009,
		"day" : "Overcast",
		"night" : "Overcast",
		"icon" : 122
	},
	{
		"code" : 1030,
		"day" : "Mist",
		"night" : "Mist",
		"icon" : 143
	},
	{
		"code" : 1063,
		"day" : "Patchy rain possible",
		"night" : "Patchy rain possible",
		"icon" : 176
	},
	{
		"code" : 1066,
		"day" : "Patchy snow possible",
		"night" : "Patchy snow possible",
		"icon" : 179
	},
	{
		"code" : 1069,
		"day" : "Patchy sleet possible",
		"night" : "Patchy sleet possible",
		"icon" : 182
	},
	{
		"code" : 1072,
		"day" : "Patchy freezing drizzle possible",
		"night" : "Patchy freezing drizzle possible",
		"icon" : 185
	},
	{
		"code" : 1087,
		"day" : "Thundery outbreaks possible",
		"night" : "Thundery outbreaks possible",
		"icon" : 200
	},
	{
		"code" : 1114,
		"day" : "Blowing snow",
		"night" : "Blowing snow",
		"icon" : 227
	},
	{
		"code" : 1117,
		"day" : "Blizzard",
		"night" : "Blizzard",
		"icon" : 230
	},
	{
		"code" : 1135,
		"day" : "Fog",
		"night" : "Fog",
		"icon" : 248
	},
	{
		"code" : 1147,
		"day" : "Freezing fog",
		"night" : "Freezing fog",
		"icon" : 260
	},
	{
		"code" : 1150,
		"day" : "Patchy light drizzle",
		"night" : "Patchy light drizzle",
		"icon" : 263
	},
	{
		"code" : 1153,
		"day" : "Light drizzle",
		"night" : "Light drizzle",
		"icon" : 266
	},
	{
		"code" : 1168,
		"day" : "Freezing drizzle",
		"night" : "Freezing drizzle",
		"icon" : 281
	},
	{
		"code" : 1171,
		"day" : "Heavy freezing drizzle",
		"night" : "Heavy freezing drizzle",
		"icon" : 284
	},
	{
		"code" : 1180,
		"day" : "Patchy light rain",
		"night" : "Patchy light rain",
		"icon" : 293
	},
	{
		"code" : 1183,
		"day" : "Light rain",
		"night" : "Light rain",
		"icon" : 296
	},
	{
		"code" : 1186,
		"day" : "Moderate rain at times",
		"night" : "Moderate rain at times",
		"icon" : 299
	},
	{
		"code" : 1189,
		"day" : "Moderate rain",
		"night" : "Moderate rain",
		"icon" : 302
	},
	{
		"code" : 1192,
		"day" : "Heavy rain at times",
		"night" : "Heavy rain at times",
		"icon" : 305
	},
	{
		"code" : 1195,
		"day" : "Heavy rain",
		"night" : "Heavy rain",
		"icon" : 308
	},
	{
		"code" : 1198,
		"day" : "Light freezing rain",
		"night" : "Light freezing rain",
		"icon" : 311
	},
	{
		"code" : 1201,
		"day" : "Moderate or heavy freezing rain",
		"night" : "Moderate or heavy freezing rain",
		"icon" : 314
	},
	{
		"code" : 1204,
		"day" : "Light sleet",
		"night" : "Light sleet",
		"icon" : 317
	},
	{
		"code" : 1207,
		"day" : "Moderate or heavy sleet",
		"night" : "Moderate or heavy sleet",
		"icon" : 320
	},
	{
		"code" : 1210,
		"day" : "Patchy light snow",
		"night" : "Patchy light snow",
		"icon" : 323
	},
	{
		"code" : 1213,
		"day" : "Light snow",
		"night" : "Light snow",
		"icon" : 326
	},
	{
		"code" : 1216,
		"day" : "Patchy moderate snow",
		"night" : "Patchy moderate snow",
		"icon" : 329
	},
	{
		"code" : 1219,
		"day" : "Moderate snow",
		"night" : "Moderate snow",
		"icon" : 332
	},
	{
		"code" : 1222,
		"day" : "Patchy heavy snow",
		"night" : "Patchy heavy snow",
		"icon" : 335
	},
	{
		"code" : 1225,
		"day" : "Heavy snow",
		"night" : "Heavy snow",
		"icon" : 338
	},
	{
		"code" : 1237,
		"day" : "Ice pellets",
		"night" : "Ice pellets",
		"icon" : 350
	},
	{
		"code" : 1240,
		"day" : "Light rain shower",
		"night" : "Light rain shower",
		"icon" : 353
	},
	{
		"code" : 1243,
		"day" : "Moderate or heavy rain shower",
		"night" : "Moderate or heavy rain shower",
		"icon" : 356
	},
	{
		"code" : 1246,
		"day" : "Torrential rain shower",
		"night" : "Torrential rain shower",
		"icon" : 359
	},
	{
		"code" : 1249,
		"day" : "Light sleet showers",
		"night" : "Light sleet showers",
		"icon" : 362
	},
	{
		"code" : 1252,
		"day" : "Moderate or heavy sleet showers",
		"night" : "Moderate or heavy sleet showers",
		"icon" : 365
	},
	{
		"code" : 1255,
		"day" : "Light snow showers",
		"night" : "Light snow showers",
		"icon" : 368
	},
	{
		"code" : 1258,
		"day" : "Moderate or heavy snow showers",
		"night" : "Moderate or heavy snow showers",
		"icon" : 371
	},
	{
		"code" : 1261,
		"day" : "Light showers of ice pellets",
		"night" : "Light showers of ice pellets",
		"icon" : 374
	},
	{
		"code" : 1264,
		"day" : "Moderate or heavy showers of ice pellets",
		"night" : "Moderate or heavy showers of ice pellets",
		"icon" : 377
	},
	{
		"code" : 1273,
		"day" : "Patchy light rain with thunder",
		"night" : "Patchy light rain with thunder",
		"icon" : 386
	},
	{
		"code" : 1276,
		"day" : "Moderate or heavy rain with thunder",
		"night" : "Moderate or heavy rain with thunder",
		"icon" : 389
	},
	{
		"code" : 1279,
		"day" : "Patchy light snow with thunder",
		"night" : "Patchy light snow with thunder",
		"icon" : 392
	},
	{
		"code" : 1282,
		"day" : "Moderate or heavy snow with thunder",
		"night" : "Moderate or heavy snow with thunder",
		"icon" : 395
	}
]

	*/


//Example JSON

/*
 * {"location":{"name":"Houston","region":"Texas","country":"USA","lat":29.77,"lon":-95.4,"tz_id":"America/Chicago","localtime_epoch":1537056878,"localtime":"2018-09-15 19:14"},"current":{"last_updated_epoch":1537056009,"last_updated":"2018-09-15 19:00","temp_c":31.0,"temp_f":87.8,"is_day":1,"condition":{"text":"Sunny","icon":"//cdn.apixu.com/weather/64x64/day/113.png","code":1000},"wind_mph":0.0,"wind_kph":0.0,"wind_degree":0,"wind_dir":"N","pressure_mb":1012.0,"pressure_in":30.4,"precip_mm":0.3,"precip_in":0.01,"humidity":63,"cloud":0,"feelslike_c":35.7,"feelslike_f":96.3,"vis_km":14.0,"vis_miles":8.0},"forecast":{"forecastday":[{"date":"2018-09-15","date_epoch":1536969600,"day":{"maxtemp_c":29.2,"maxtemp_f":84.6,"mintemp_c":24.2,"mintemp_f":75.6,"avgtemp_c":26.3,"avgtemp_f":79.4,"maxwind_mph":4.9,"maxwind_kph":7.9,"totalprecip_mm":2.2,"totalprecip_in":0.09,"avgvis_km":16.7,"avgvis_miles":10.0,"avghumidity":81.0,"condition":{"text":"Moderate or heavy rain shower","icon":"//cdn.apixu.com/weather/64x64/day/356.png","code":1243},"uv":9.5},"astro":{"sunrise":"07:06 AM","sunset":"07:27 PM","moonrise":"12:57 PM","moonset":"11:52 PM"}}]}}

	*/
