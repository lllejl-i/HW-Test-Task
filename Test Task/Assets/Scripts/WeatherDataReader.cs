using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class WeatherDataController
{
	public static WeatherDataController Controller { get; } = new WeatherDataController();
	private WeatherDataController()
	{
		_cardHolder = new WeatherCardHolder
		{
			Weathers = new List<WeatherCard> { }
		};
		_weatherIcons = Resources.LoadAll<Texture2D>(Pathes.WeatherImagesPath);

		if (File.Exists(Pathes.SaveFile) && File.ReadAllText(Pathes.SaveFile).Length > 0)
		{
			ReadFromJson();
		}
		else
		{
			ReadFromAPI();
		}
	}
	private WeatherCardHolder _cardHolder;
	private Texture2D[] _weatherIcons;

	public void SaveData(WeatherCard[,] cards)
	{
		_cardHolder.Weathers.Clear();
		foreach (WeatherCard card in cards)
		{
			_cardHolder.Weathers.Add(card);
		}


		if (!File.Exists(Pathes.SaveFile))
			using (FileStream fs = File.Create(Pathes.SaveFile)) ;


		string json = JsonConvert.SerializeObject(_cardHolder);
		File.WriteAllText(Pathes.SaveFile, json);
	}

	public WeatherCard[,] CreateWeatherCards(List<VisualElement> weatherCards)
	{
		WeatherCard[,] result = new WeatherCard[3, 3];

		for (int i = 0; i < weatherCards.Count; i++)
		{
			if (_cardHolder.Weathers[i].Weather != null)
			{
				_cardHolder.Weathers[i].WeatherCardItem = weatherCards[i];
				weatherCards[i].Q<Label>("cityName").text = _cardHolder.Weathers[i].Weather.City;
				weatherCards[i].Q<Label>("tempValue").text = _cardHolder.Weathers[i].Weather.Temperature;
				var icon = _weatherIcons.FirstOrDefault(ic => ic.name == _cardHolder.Weathers[i].Weather.WeatherType.ToLower()
										|| ic.name.Contains(_cardHolder.Weathers[i].Weather.WeatherType.ToLower())
										|| _cardHolder.Weathers[i].Weather.WeatherType.ToLower().Contains(ic.name))
										?? _weatherIcons.FirstOrDefault(ic => ic.name == "clear");
				weatherCards[i].Q<VisualElement>("icon").style.backgroundImage = icon;
			}
		}

		return GetCards();
	}

	public WeatherCard[,] GetCards()
	{
		WeatherCard[,] result = new WeatherCard[3, 3];
		for (int i = 0; i < _cardHolder.Weathers.Count; i++)
		{
			result[(i - i % 3) / 3, i % 3] = (WeatherCard)_cardHolder.Weathers[i].Clone();
		}
		return result;
	}

	private void ReadFromJson()
	{
		string json = File.ReadAllText(Pathes.SaveFile);
		_cardHolder = JsonConvert.DeserializeObject<WeatherCardHolder>(json);
	}

	private void ReadFromAPI()
	{
		string[] citiesName = new string[] { "Kyiv", "Lviv", "Kharkiv", "Odesa", "Mykolaiv", "Cairo", "Warsaw", "London", "Paris" };
		for (int i = 0; i < citiesName.Length; i++)
		{
			var weather = ReadWeather(citiesName[i]);
			_cardHolder.Weathers.Add(new WeatherCard
			{
				Weather = weather,
				RowGridPosition = (i - i % 3) / 3,
				ColGridPosition = i % 3,
			});
		}
	}


	private Weather ReadWeather(string city)
	{
		Weather weather = new Weather();
		string apiKey = "1de5bbf2ee83210b56bc7a57582d9ce1";
		string geoUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(geoUrl);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		using (StreamReader reader = new StreamReader(response.GetResponseStream()))
		{
			string line = reader.ReadToEnd();
			var weatherObj = JsonConvert.DeserializeObject<WeatherApiModel>(line);
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
	}

	private class MainBlock
	{
		public float temp;
	}
	#endregion
}

public class WeatherCardHolder
{
	public List<WeatherCard> Weathers { get; set; }
}
