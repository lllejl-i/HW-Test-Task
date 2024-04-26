using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class AboutUsDoc : AnimatedToolkitPage
{
    UIDocument _doc;
	Audio _audio;

	[Inject]
	public void InjectDependencies(Audio audio)
	{
		_audio = audio;
	}

	private void Awake()
	{
		_doc = GetComponent<UIDocument>();
	}

	private void OnEnable()
	{
		StartCoroutine(OpenAnimation(_doc.rootVisualElement));
		var link = _doc.rootVisualElement.Q<Button>("linkButton");
		link.clicked += () =>
		{
			Application.OpenURL(Pathes.LinkedInUrl);
			_audio.OnClickMusic();
		};

		var close = _doc.rootVisualElement.Q<Button>("closeButton");
		close.clicked += () =>
		{
			StartCoroutine(CloseAnimation(_doc.rootVisualElement));
			_audio.OnClickMusic();
		};
		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(link, AnimationType.Growing,
			new Dictionary<AnimationDataType, object>()
			{
				{AnimationDataType.GrowingValue, 1.1f }
			});
	}

	private IEnumerator OpenAnimation(VisualElement root)
	{
		yield return new WaitForEndOfFrame();
		root.style.opacity = new StyleFloat(0f);
		while (root.style.opacity.value <= 1)
		{
			root.style.opacity = new StyleFloat(root.resolvedStyle.opacity + 0.003f);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator CloseAnimation(VisualElement root)
	{
		while (root.resolvedStyle.opacity > 0)
		{
			yield return new WaitForEndOfFrame();
			root.style.opacity = new StyleFloat(root.resolvedStyle.opacity - 0.003f);
		}
		gameObject.SetActive(false);

	}
}
