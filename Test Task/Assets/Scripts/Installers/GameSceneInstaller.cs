using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
	[SerializeField]
	private AboutUsDoc _about;
	[SerializeField]
	private SettingsDoc _settings;
	[SerializeField]
	private Audio _audio;
	public override void InstallBindings()
	{
		Container.Bind<JsonSerializer>().AsSingle().NonLazy();
		Container.Bind<ApiDataReader>().AsSingle().NonLazy();
		Container.Bind<WeatherDataController>().AsSingle().NonLazy();
		Container.BindInstance(_about);
		Container.BindInstance(_settings);
		Container.BindInstance(_audio);
	}
}