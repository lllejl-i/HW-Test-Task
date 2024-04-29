using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class WeatherDataController
{
	private static List<string> cities = new List<string>() { "Kyiv", "Lviv", "Kharkiv", "Odesa", "Mykolaiv", "Cairo", "Warsaw", "London", "Paris" };
	private WeatherCardHolder _cardHolder;
	private JsonSerializer _serializer;
	private ApiDataReader _reader;

	[Inject]
	public void InjectDependencies(JsonSerializer serializer, ApiDataReader reader)
	{
		_serializer = serializer;
		_reader = reader;
		ReadData();
	}

	public WeatherDataController(){ }

	private void ReadData() { 
		if (File.Exists(Pathes.SaveFile) && File.ReadAllText(Pathes.SaveFile).Length > 0)
		{
			_cardHolder = _serializer.ReadData();
		}
		else
		{
			_cardHolder = _reader.ReadData(cities);
		}
	}
	public WeatherCard[,] CreateWeatherCards(List<VisualElement> weatherCards)
	{
		for (int i = 0; i < weatherCards.Count; i++)
		{
			if (_cardHolder.Weathers[i].Weather != null)
			{
				_cardHolder.Weathers[i].WeatherCardItem = weatherCards[i];
				weatherCards[i].Q<Label>("cityName").text = _cardHolder.Weathers[i].Weather.City;
				weatherCards[i].Q<Label>("tempValue").text = _cardHolder.Weathers[i].Weather.Temperature;
			}
		}

		return GetCards();
	}

	public WeatherCard[,] ResetCards(List<VisualElement> weatherCards)
	{
		_cardHolder = _reader.ReadData(cities);
		return CreateWeatherCards(weatherCards);
	}

	private WeatherCard[,] GetCards()
	{
		WeatherCard[,] result = new WeatherCard[3, 3];
		for (int i = 0; i < _cardHolder.Weathers.Count; i++)
		{
			result[(i - i % 3) / 3, i % 3] = (WeatherCard)_cardHolder.Weathers[i].Clone();
		}
		return result;
	}

	public void SaveData(WeatherCard[,] cards)
	{
		_cardHolder.Weathers.Clear();
		foreach (WeatherCard weatherCard in cards)
		{
			_cardHolder.Weathers.Add(weatherCard);
		}
		_serializer.SaveData(cards);
	}
}
