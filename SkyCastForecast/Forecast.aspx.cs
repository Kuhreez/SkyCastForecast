using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkyCastForecast
{
	public partial class Forecast : Page
	{
		readonly string googleGeocodeURI = "https://maps.googleapis.com/maps/api/geocode/json";
		readonly string darkSkyURI = "https://api.darksky.net/forecast";
		readonly string geocodeAPIKey = "AIzaSyANU2G-H2pV0D-fJWkY3MQ6Y7nm43SOeYY";
		readonly string darkSkyAPIKey = "a3498df0271903a6b99e0c9e48c2cec3";

		Dictionary<string, string> weatherIconDict = new Dictionary<string, string>()
		{
			{ "clear-day", "wi wi-day-sunny"},
			{ "clear-night", "wi wi-night-clear"},
			{ "rain", "wi wi-rain"},
			{ "snow", "wi wi-snow"},
			{ "sleet", "wi wi-sleet"},
			{ "wind", "wi wi-windy"},
			{ "fog", "wi wi-fog"},
			{ "cloudy", "wi wi-cloud"},
			{ "partly-cloudy-day", "wi wi-day-cloudy"},
			{ "partly-cloudy-night", "wi wi-night-alt-cloudy"}
		};

		protected void Page_Load(object sender, EventArgs e)
		{
			lb_errorMessages.Text = "";
			if (Application["queryHistory"] == null)
				Application["queryHistory"] = new List<string>();
			else
			{
				if (!IsPostBack)
				{
					var queryHistory = Application["queryHistory"] as List<string>;
					ddl_queryHistory.DataSource = queryHistory;
					ddl_queryHistory.DataBind();
				}
			}

		}

		protected void SearchLocation(object sender, EventArgs e)
		{
			pn_current.Visible = true;
			pn_hourly.Visible = true;
			pn_history.Visible = true;
			historyContainer.Visible = false;
			var queryHistory = Application["queryHistory"] as List<string>;

			if (!queryHistory.Contains(tb_searchLocation.Text))
				queryHistory.Add(tb_searchLocation.Text);

			Application["queryHistory"] = queryHistory;
			ddl_queryHistory.DataSource = queryHistory;
			ddl_queryHistory.DataBind();

			string lattitude = "";
			string longitude = "";
			string location = "";

			var uriBuilder = new UriBuilder(googleGeocodeURI);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query["address"] = tb_searchLocation.Text.Trim();
			query["key"] = geocodeAPIKey;
			uriBuilder.Query = query.ToString();
			var googleGeocodeURIfull = uriBuilder.ToString();

			try
			{
				var hwr = (HttpWebRequest)WebRequest.Create(googleGeocodeURIfull);
				if (hwr != null)
				{
					hwr.Method = "GET";
					hwr.Timeout = 12000;
					using (Stream s = hwr.GetResponse().GetResponseStream())
					using (StreamReader sr = new StreamReader(s))
					{
						var jsonResponse = sr.ReadToEnd();
						var result = JsonConvert.DeserializeObject<RootObject>(jsonResponse);
						lattitude = result.results[0].geometry.location.lat.ToString();
						longitude = result.results[0].geometry.location.lng.ToString();
						location = result.results[0].formatted_address;
						s.Close();
						sr.Close();
					}
				}
			}
			catch (Exception ex)
			{
				//lb_errorMessages.Text = ex.ToString();
				lb_errorMessages.Text = "Something went wrong, try a different address please";
				pn_current.Visible = false;
				pn_hourly.Visible = false;
				pn_history.Visible = false;
				return;
			}

			if (string.IsNullOrEmpty(lattitude) || string.IsNullOrEmpty(longitude))
			{
				lb_errorMessages.Text = "Could not find Lattitude/Longitude for this location";
				pn_current.Visible = false;
				pn_hourly.Visible = false;
				pn_history.Visible = false;
				return;
			}

			hf_lattitude.Value = lattitude;
			hf_longitude.Value = longitude;

			try
			{
				var hwr2 = (HttpWebRequest)WebRequest.Create($"{darkSkyURI}/{darkSkyAPIKey}/{lattitude},{longitude}?exclude=minutely,flags");
				if (hwr2 != null)
				{
					hwr2.Method = "GET";
					hwr2.Timeout = 12000;
					using (Stream s = hwr2.GetResponse().GetResponseStream())
					using (StreamReader sr = new StreamReader(s))
					{
						var jsonResponse = sr.ReadToEnd();
						var result = JsonConvert.DeserializeObject<RootObject2>(jsonResponse);

						// Populate Current Weather Information
						locationName.InnerText = location;
						currentDate.InnerText = ToDateTimeFromEpoch(result.currently.time);
						if (!string.IsNullOrEmpty(result.currently.icon))
							currentWeatherIcon.Attributes["class"] = weatherIconDict[result.currently.icon] + " big-font";
						currentTemperature.InnerText = result.currently.apparentTemperature.ToString();
						currentSummary.InnerText = result.currently.summary;
						precipChance.InnerText = (result.daily.data[0].precipProbability * 100).ToString() + "%";

						if (!string.IsNullOrEmpty(result.daily.data[0].precipType))
							precipIcon.Attributes["class"] = weatherIconDict[result.daily.data[0].precipType];

						// Populate Hidden Field with JSON data needed to plot chart
						HourlyPlot hp = new HourlyPlot()
						{
							type = "scatter",
							mode = "markers+text",
							textposition = "top",
						};

						var hoursData = result.hourly.data.Where((x, i) => i % 3 == 0).Take(8).ToList();
						var hours = hoursData.Select(x => ToDateTimeFromEpochDate(x.time).ToString("hh tt")).ToList();

						for (int i = 0; i < hours.Count; i++)
							if (hours[i][0] == '0') hours[i] = hours[i].Substring(1);

						var temps = hoursData.Select(x => Convert.ToInt32(x.apparentTemperature)).ToList();
						var texts = hoursData.Select(x => Convert.ToInt32(x.apparentTemperature).ToString() + "\u00B0").ToList();
						hp.x = hours;
						hp.y = temps;
						hp.text = texts;
						string hourlyJsonData = JsonConvert.SerializeObject(hp);
						hf_hourlyData.Value = hourlyJsonData;
						hf_hourlyData.DataBind();

						ScriptManager.RegisterStartupScript(Page, typeof(Page), "createHourlyPlot", "createHourlyPlot();", true);
						sr.Close();
					}
				}
			}
			catch (Exception ex)
			{
				//lb_errorMessages.Text = ex.ToString();
				lb_errorMessages.Text = "Uh oh, something went wrong. Please try a different address";
				pn_current.Visible = false;
				pn_hourly.Visible = false;
				pn_history.Visible = false;
				return;
			}
		}

		protected void SeeHistory(object sender, EventArgs e)
		{
			historyContainer.Visible = true;
			var lattitude = hf_lattitude.Value;
			var longitude = hf_longitude.Value;
			var unixTime = ConvertToUnixTime(Convert.ToDateTime(datePicker.Value));

			try
			{
				var hwr = (HttpWebRequest)WebRequest.Create($"{darkSkyURI}/{darkSkyAPIKey}/{lattitude},{longitude},{unixTime}?exclude=hourly,minutely,flags");
				if (hwr != null)
				{
					hwr.Method = "GET";
					hwr.Timeout = 12000;
					using (Stream s = hwr.GetResponse().GetResponseStream())
					using (StreamReader sr = new StreamReader(s))
					{
						var jsonResponse = sr.ReadToEnd();
						var result = JsonConvert.DeserializeObject<RootObject2>(jsonResponse);
						var dailyData = result.daily.data[0];
						var currentlyData = result.currently;

						//if (!string.IsNullOrEmpty(currentlyData.icon))
						//	historyWeatherIcon.Attributes["class"] = weatherIconDict[currentlyData.icon] + " big-font";
						//historyTemperature.InnerText = currentlyData.apparentTemperature.ToString();
						historySummary.InnerText = currentlyData.summary;

						historyMinTemp.InnerText = dailyData.apparentTemperatureMin.ToString();
						historyMaxTemp.InnerText = dailyData.apparentTemperatureMax.ToString();

						sunriseTime.InnerText = ToDateTimeFromEpochTime(dailyData.sunriseTime);
						sunsetTime.InnerText = ToDateTimeFromEpochTime(dailyData.sunsetTime);

						humidityLevel.InnerText = (currentlyData.humidity * 100).ToString();
						cloudCover.InnerText = (currentlyData.cloudCover * 100).ToString() + "%";

						sr.Close();
					}
				}
				ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScrollScript", "document.location.hash = '#historyPanel'", true);
			}
			catch (Exception ex)
			{
				lb_errorMessages.Text = "Uh oh, something went wrong. Please try something different.";
				pn_current.Visible = false;
				pn_hourly.Visible = false;
				pn_history.Visible = false;
				return;
			}
		}

		public static string ToDateTimeFromEpoch(int unixTime)
		{
			var timeInTicks = unixTime * TimeSpan.TicksPerSecond;
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks).ToLongDateString();
		}

		public static string ToDateTimeFromEpochTime(int unixTime)
		{
			var timeInTicks = unixTime * TimeSpan.TicksPerSecond;
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks).ToShortTimeString();
		}

		public static DateTime ToDateTimeFromEpochDate(int unixTime)
		{
			var timeInTicks = unixTime * TimeSpan.TicksPerSecond;
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
		}

		public static string ConvertToUnixTime(DateTime datetime)
		{
			DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (datetime - sTime).TotalSeconds.ToString();
		}

		protected void ddl_queryHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
			tb_searchLocation.Text = ((DropDownList)sender).SelectedValue;
		}
	}

}