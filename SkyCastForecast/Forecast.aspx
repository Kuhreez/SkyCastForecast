<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forecast.aspx.cs" Inherits="SkyCastForecast.Forecast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>SkyCast Forecast</title>
	<link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
	<link href="Content/bootstrap.min.css" rel="stylesheet" />
	<link href="Content/weather-icons.min.css" rel="stylesheet" />
	<link href="Content/weather-icons-wind.min.css" rel="stylesheet" />
	<link href="Content/CustomStyle.css" rel="stylesheet" />
	<script src="Scripts/jquery-1.9.1.min.js"></script>
	<script src="Scripts/bootstrap.min.js"></script>
	<script src="Scripts/Plotly/plotly.min.js"></script>
	<script>
		// Viewport Width
		var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0) * .45

		// plotly layout
		var layout = {
			showlegend: false,
			xaxis: {
				showline: true,
				showgrid: true,
				showticklabels: true,
				linecolor: 'rgb(204,204,204)',
				linewidth: 2,
				autotick: false,
				tickfont: {
					size: 12,
					color: 'rgb(82, 82, 82)'
				}
			},
			yaxis: {
				showgrid: false,
				zeroline: false,
				showline: false,
				showticklabels: false
			},
			paper_bgcolor: '#EDF2F4',
			plot_bgcolor: '#EDF2F4',
			width: w,
			autosize: false,
		};

		function createHourlyPlot() {
			var datum = JSON.parse(document.getElementById('hf_hourlyData').value);
			var data = [datum];
			Plotly.newPlot('hourlyData', data, layout);
		}

		// Max Date for Date field is today and make hourly plot
		$(function () {
			$('[type="date"]').prop('max', function () {
				return new Date().toJSON().split('T')[0];
			});
			createHourlyPlot();
		});

	</script>
</head>
<body>
	<form id="form1" runat="server">

		<asp:HiddenField ID="hf_hourlyData" runat="server" />
		<asp:HiddenField ID="hf_lattitude" runat="server" />
		<asp:HiddenField ID="hf_longitude" runat="server" />

		<!-- First Container -->
		<div class="container-fluid bg-1 text-center">
			<h1><i class="glyphicon glyphicon-search"></i>&nbsp;Search a location</h1>
			<br />
			<div>
				<asp:TextBox ID="tb_searchLocation" ForeColor="Black" runat="server"></asp:TextBox>
				<asp:Button ID="btn_searchLocation" CssClass="btn btn-primary btn-lg" OnClick="SearchLocation" Text="Search Location" runat="server" />
			</div>

			<div>
				<p style="display: inline">Query Log: </p>
				<asp:DropDownList ID="ddl_queryHistory" AutoPostBack="true" OnSelectedIndexChanged="ddl_queryHistory_SelectedIndexChanged" ForeColor="Black" runat="server"></asp:DropDownList>
			</div>

			<div>
				<asp:Label ID="lb_errorMessages" ForeColor="Red" runat="server"></asp:Label>
			</div>
		</div>

		<!-- Second Container -->
		<asp:Panel ID="pn_current" CssClass="container-fluid bg-2 text-center" Visible="false" runat="server">
			<h3 id="currentDate" runat="server"></h3>
			<h4 id="locationName" runat="server"></h4>

			<div>
				<i id="currentWeatherIcon" style="display: inline" runat="server"></i>
				<p id="currentTemperature" class="big-font" style="display: inline" runat="server"></p>
				<i class="wi wi-fahrenheit big-font" style="display: inline"></i>
			</div>
			<div>
				<p id="currentSummary" runat="server"></p>
			</div>
			<div>
				<p style="display: inline">Chance of Precipitation: </p>
				<p id="precipChance" style="display: inline" runat="server"></p>
				<p style="display: inline">&nbsp;</p>
				<i id="precipIcon" style="display: inline" runat="server"></i>
			</div>
		</asp:Panel>

		<!-- Third Container -->
		<asp:Panel ID="pn_hourly" CssClass="container-fluid bg-3 text-center" Visible="false" runat="server">
			<h3>Hourly Temperature</h3>
			<div id="hourlyData" class="col-sm-6 col-centered">
			</div>
		</asp:Panel>

		<!-- Fourth Container -->
		<asp:Panel ID="pn_history" CssClass="container-fluid bg-4 text-center" Visible="false" runat="server">
			<a id="historyPanel"></a>
			<div>
				<input id="datePicker" style="color: black" type="date" runat="server" />
				<asp:Button ID="btn_seeHistory" CssClass="btn btn-primary btn-lg" OnClick="SeeHistory" Text="Search History" runat="server" />
			</div>
			<div id="historyContainer" runat="server" visible="false">
				<div>
					<h4 id="historySummary" runat="server"></h4>
				</div>
				<div>
					<p id="historyMinTemp" style="display: inline" runat="server"></p>
					<i class="wi wi-fahrenheit" style="display: inline"></i>
					<p style="display: inline">&nbsp;~&nbsp;</p>
					<p id="historyMaxTemp" style="display: inline" runat="server"></p>
					<i class="wi wi-fahrenheit" style="display: inline"></i>
				</div>
				<div>
					<i class="wi wi-sunrise" style="display: inline"></i>
					<p id="sunriseTime" style="display: inline" runat="server"></p>
					<p style="display: inline">&nbsp;&nbsp;</p>
					<i class="wi wi-sunset" style="display: inline"></i>
					<p id="sunsetTime" style="display: inline" runat="server"></p>
				</div>
				<div>
					<p style="display: inline">Humidity: </p>
					<p id="humidityLevel" style="display: inline" runat="server"></p>
					<i class="wi wi-humidity" style="display: inline"></i>
				</div>
				<div>
					<p style="display: inline">Cloud Cover: </p>
					<p id="cloudCover" style="display: inline" runat="server"></p>
					<i class="wi wi-cloud" style="display: inline"></i>
				</div>
			</div>
		</asp:Panel>

	</form>
</body>
</html>
