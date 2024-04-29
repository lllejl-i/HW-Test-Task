using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
public class ApiDataReader
{
	private string _apiKey = "1de5bbf2ee83210b56bc7a57582d9ce1";
	
	public WeatherCardHolder ReadData(List<string> cities)
	{ 
		WeatherCardHolder cardHolder = new WeatherCardHolder()
		{
			Weathers = new List<WeatherCard>()
		};
		for (int i = 0; i < cities.Count; i++)
		{
			var weather = ReadCard(cities[i]);
			cardHolder.Weathers.Add(new WeatherCard
			{
				Weather = weather,
				RowGridPosition = (i - i % 3) / 3,
				ColGridPosition = i % 3,
			});
		}
		return cardHolder;
	}
	public Weather ReadCard(string city)
	{
		Weather weather = new Weather();
		string geoUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(geoUrl);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		using (StreamReader reader = new StreamReader(response.GetResponseStream()))
		{
			string line = reader.ReadToEnd();
			var weatherObj = JsonConvert.DeserializeObject<WeatherApiModel>(line);
			weather.TextureUrl = $"http://openweathermap.org/img/wn/{weatherObj.weather[0].icon}@2x.png";			
			weather.City = city;
			weather.Temperature = Math.Round(weatherObj.main.temp - 273, 1).ToString();
			weather.WeatherType = weatherObj.weather[0].main;
		}
		return weather;

	}

	#region API model`s class system
	private class WeatherApiModel
	{
		public WeatherBlock[] weather;
		public MainBlock main;
	}

	private class WeatherBlock
	{
		public string main;
		public string icon;
	}

	private class MainBlock
	{
		public float temp;
	}
	#endregion
}
