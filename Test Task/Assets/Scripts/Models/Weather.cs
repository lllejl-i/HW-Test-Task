
using System;

public class Weather : ICloneable
{
	public string City;
	public string WeatherType;
	public string Temperature;
	public string TextureUrl;

	public object Clone()
	{
		return new Weather()
		{
			City = City,
			WeatherType = WeatherType,
			Temperature = Temperature,
			TextureUrl = TextureUrl
		};
	}
}