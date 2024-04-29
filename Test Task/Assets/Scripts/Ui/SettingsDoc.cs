using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SettingsDoc : AnimatedToolkitPage
{
    private UIDocument _doc;
	private AboutUsDoc _aboutUsDoc;
	private Audio _audio;
	private bool _isInitialized = false;
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
		var musicChb = _doc.rootVisualElement.Q<Toggle>("isMusicChecked");
		var soundChb = _doc.rootVisualElement.Q<Toggle>("isSoundChecked");
		musicChb.RegisterValueChangedCallback((e) =>
		{
			if (_isInitialized)
			{
				_audio.OnClickMusic();
				_audio.ChangePlayingMode(musicChb.value);
			}
		});
		soundChb.RegisterValueChangedCallback((e) =>
		{
			if (_isInitialized)
			{
				_audio.OnClickMusic();
				_audio.ChangeSoundsMode(soundChb.value);
			}
		});
		var volumeSlider = _doc.rootVisualElement.Q<Slider>("volumeSlider");
		volumeSlider.RegisterValueChangedCallback((e) =>
		{
			_audio.ChangeVolume(volumeSlider.value/100);
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
		StartCoroutine(ChangeCheckBoxValues(musicChb, soundChb));
	}

	private IEnumerator ChangeCheckBoxValues(Toggle musicChb, Toggle soundChb)
	{
		yield return new WaitForEndOfFrame();

		musicChb.value = _audio.PlayMusic;
		soundChb.value = _audio.MakeSounds;
		_isInitialized = true;
	}

	private void OnDisable()
	{
		_isInitialized = false;
	}
}
