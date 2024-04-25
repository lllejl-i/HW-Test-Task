using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FirstSceneDoc : AnimatedToolkitPage
{
	ObservableSubject _secondSceneLoader;
	UIDocument _document;
	VisualElement _root;
	private void OnEnable()
	{
		if (_document == null)
		{
			_document = GetComponent<UIDocument>();
			_secondSceneLoader = GetComponent<ObservableSubject>();
		}

		_root = _document.rootVisualElement;
		VisualElement button = _root.Q<VisualElement>("LoadButton");
		AddAnimation<PointerEnterEvent, PointerLeaveEvent>(button, AnimationType.Growing);
		AddAnimation<PointerEnterEvent, PointerLeaveEvent>(button, AnimationType.BackgroundColorChanging);

		VisualElement image = _root.Q<VisualElement>("Logo");
		AddAnimation<PointerEnterEvent, PointerLeaveEvent>(image, AnimationType.Growing);
	}

	private void Start()
	{
		VisualElement button = _root.Q<VisualElement>("LoadButton");
		button.RegisterCallback<PointerUpEvent>((e) =>
		{
			RadialProgressBar.ProgressBar.Activate();
			var loadingResult = SceneManager.LoadSceneAsync("SecondScene");
			StartCoroutine(LoadScene(loadingResult));
		});
	}

	private IEnumerator LoadScene(AsyncOperation loadingResult)
	{
		while (loadingResult.progress < 1)
		{
			Debug.Log($"Loading progress: {loadingResult.progress}");
			_secondSceneLoader.Notify(loadingResult.progress);
			yield return new WaitForEndOfFrame();
		}
	}
}
