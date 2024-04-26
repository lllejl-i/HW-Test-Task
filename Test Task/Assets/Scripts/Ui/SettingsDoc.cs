using System.Collections.Generic;
using UnityEngine.UIElements;
using Zenject;

public class SettingsDoc : AnimatedToolkitPage
{
    private UIDocument _doc;
	private AboutUsDoc _aboutUsDoc;
	private Audio _audio;
	[Inject]
	public void InjectDependencies(AboutUsDoc aboutUs, Audio audio)
	{
		_aboutUsDoc = aboutUs;
		_audio = audio;
	}

	private void Awake()
	{
		_doc = GetComponent<UIDocument>();
	}

	private void OnEnable()
	{
		var volumeSlider = _doc.rootVisualElement.Q<Slider>("volumeSlider");
		volumeSlider.RegisterValueChangedCallback((e) =>
		{
			_audio.ChangeVolume(volumeSlider.value);
		});
		var musicChb = _doc.rootVisualElement.Q<Toggle>("isMusicChecked");
		musicChb.RegisterValueChangedCallback((e) =>
		{
			_audio.OnClickMusic();
			_audio.ChangePlayingMode(musicChb.value);
		});
		var soundChb = _doc.rootVisualElement.Q<Toggle>("isSoundChecked");
		soundChb.RegisterValueChangedCallback((e) =>
		{
			_audio.OnClickMusic();
			_audio.ChangeSoundsMode(soundChb.value);
		});
		var closeButton = _doc.rootVisualElement.Q<Button>("closeButton");
		closeButton.clicked += () =>
		{
			_audio.OnClickMusic();
			gameObject.SetActive(false);
		};
		var aboutUsButton = _doc.rootVisualElement.Q<Button>("aboutUsButton");
		aboutUsButton.clicked += () =>
		{
			_audio.OnClickMusic();
			_aboutUsDoc.gameObject.SetActive(true);
		};
		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(aboutUsButton, AnimationType.Growing, new Dictionary<AnimationDataType, object>()
			{
				{AnimationDataType.GrowingValue, 1.2f }
			});
		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(aboutUsButton, AnimationType.BackgroundColorChanging, new Dictionary<AnimationDataType, object>()
		{ 
			{ AnimationDataType.ColorToChange, Properties.ButtonChangedColor } 
		}); 
	}
}
