// Geocode Classes
using System.Collections.Generic;

namespace SkyCastForecast
{

	public class AddressComponent
	{
		public string long_name { get; set; }
		public string short_name { get; set; }
		public List<string> types { get; set; }
	}

	public class Northeast
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Southwest
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Bounds
	{
		public Northeast northeast { get; set; }
		public Southwest southwest { get; set; }
	}

	public class Location
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Northeast2
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Southwest2
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Viewport
	{
		public Northeast2 northeast { get; set; }
		public Southwest2 southwest { get; set; }
	}

	public class Geometry
	{
		public Bounds bounds { get; set; }
		public Location location { get; set; }
		public string location_type { get; set; }
		public Viewport viewport { get; set; }
	}

	public class Result
	{
		public List<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
		public Geometry geometry { get; set; }
		public bool partial_match { get; set; }
		public string place_id { get; set; }
		public List<string> types { get; set; }
	}

	public class RootObject
	{
		public List<Result> results { get; set; }
		public string status { get; set; }
	}

	// End GeoCode Classes

	// DarkSky Classes
	public class Currently
	{
		public int time { get; set; }
		public string summary { get; set; }
		public string icon { get; set; }
		public double nearestStormDistance { get; set; }
		public double nearestStormBearing { get; set; }
		public double precipIntensity { get; set; }
		public double precipProbability { get; set; }
		public double temperature { get; set; }
		public double apparentTemperature { get; set; }
		public double dewPoint { get; set; }
		public double humidity { get; set; }
		public double windSpeed { get; set; }
		public double windBearing { get; set; }
		public double visibility { get; set; }
		public double cloudCover { get; set; }
		public double pressure { get; set; }
		public double ozone { get; set; }
	}

	public class Datum
	{
		public int time { get; set; }
		public double precipIntensity { get; set; }
		public double precipProbability { get; set; }
	}

	public class Minutely
	{
		public string summary { get; set; }
		public string icon { get; set; }
		public List<Datum> data { get; set; }
	}

	public class Datum2
	{
		public int time { get; set; }
		public string summary { get; set; }
		public string icon { get; set; }
		public double precipIntensity { get; set; }
		public double precipProbability { get; set; }
		public double temperature { get; set; }
		public double apparentTemperature { get; set; }
		public double dewPoint { get; set; }
		public double humidity { get; set; }
		public double windSpeed { get; set; }
		public double windBearing { get; set; }
		public double visibility { get; set; }
		public double cloudCover { get; set; }
		public double pressure { get; set; }
		public double ozone { get; set; }
	}

	public class Hourly
	{
		public string summary { get; set; }
		public string icon { get; set; }
		public List<Datum2> data { get; set; }
	}

	public class Datum3
	{
		public int time { get; set; }
		public string summary { get; set; }
		public string icon { get; set; }
		public int sunriseTime { get; set; }
		public int sunsetTime { get; set; }
		public double moonPhase { get; set; }
		public double precipIntensity { get; set; }
		public double precipIntensityMax { get; set; }
		public double precipProbability { get; set; }
		public double temperatureMin { get; set; }
		public int temperatureMinTime { get; set; }
		public double temperatureMax { get; set; }
		public int temperatureMaxTime { get; set; }
		public double apparentTemperatureMin { get; set; }
		public int apparentTemperatureMinTime { get; set; }
		public double apparentTemperatureMax { get; set; }
		public int apparentTemperatureMaxTime { get; set; }
		public double dewPoint { get; set; }
		public double humidity { get; set; }
		public double windSpeed { get; set; }
		public double windBearing { get; set; }
		public double visibility { get; set; }
		public double cloudCover { get; set; }
		public double pressure { get; set; }
		public double ozone { get; set; }
		public int? precipIntensityMaxTime { get; set; }
		public string precipType { get; set; }
	}

	public class Daily
	{
		public string summary { get; set; }
		public string icon { get; set; }
		public List<Datum3> data { get; set; }
	}

	public class RootObject2
	{
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string timezone { get; set; }
		public int offset { get; set; }
		public Currently currently { get; set; }
		public Minutely minutely { get; set; }
		public Hourly hourly { get; set; }
		public Daily daily { get; set; }
	}

	// End Dark Sky Classes

	// Dark Sky Report Objects
	//public class Line
	//{
	//	public string color { get; set; }
	//	public int width { get; set; }
	//}

	public class HourlyPlot
	{
		public string type { get; set; }
		public List<string> x { get; set; }
		public List<int> y { get; set; }
		public List<string> text { get; set; }
		public string mode { get; set; }
		public string textposition { get; set; }
	}
}