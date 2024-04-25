using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class JsonSerializer
{
	public void SaveData(WeatherCard[,] cards)
	{
		WeatherCardHolder holder = new WeatherCardHolder()
		{
			Weathers = new List<WeatherCard>()
		};
		foreach (WeatherCard card in cards)
		{
			holder.Weathers.Add(card);
		}
		if (!File.Exists(Pathes.SaveFile))
			using (FileStream fs = File.Create(Pathes.SaveFile));
		string json = JsonConvert.SerializeObject(holder);
		File.WriteAllText(Pathes.SaveFile, json);
	}

	public WeatherCardHolder ReadData()
	{
		string json = File.ReadAllText(Pathes.SaveFile);
		return JsonConvert.DeserializeObject<WeatherCardHolder>(json);
	}
}
