using Newtonsoft.Json;
using System;
using UnityEngine.UIElements;

public class WeatherCard : ICloneable
{
	[JsonIgnore]
	public VisualElement WeatherCardItem { get; set; }
	public Weather Weather;
	public int RowGridPosition;
	public int ColGridPosition;

	public object Clone()
	{
		return new WeatherCard()
		{
			WeatherCardItem = WeatherCardItem,
			RowGridPosition = RowGridPosition,
			ColGridPosition = ColGridPosition,
			Weather = (Weather)Weather?.Clone() ?? null
		};
	}
}