using UnityEngine;
using UnityEngine.UI;

public class RadialProgressBar : MonoBehaviour, IObserver
{
	private static RadialProgressBar progressBar;
	public static RadialProgressBar ProgressBar => progressBar;
	private Image _mainImage;
	private void Awake()
	{
		if (progressBar == null)
		{
			DontDestroyOnLoad(transform.parent);
			progressBar = this;
			_mainImage = GetComponent<Image>();
			gameObject.SetActive(false);
		}
	}

	public void Activate()
	{
		_mainImage.fillAmount = 0;
		gameObject.SetActive(true);
	}

	public void SendMessage(object data)
	{
		float percentsToAdd = (float)data;
		_mainImage.fillAmount = percentsToAdd;
		if (_mainImage.fillAmount >= 0.9f)
		{
			gameObject.SetActive(false);
		}
	}
}

