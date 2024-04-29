using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class SecondSceneDoc : AnimatedToolkitPage
{
	[SerializeField]
	private VisualTreeAsset _cardTemplate;
	private UIDocument _doc;
	private List<VisualElement> _rows;
	private WeatherCard[,] _items;
	private List<WeatherCard> _removedCards = new List<WeatherCard>();
	private bool _loaded = false;
	private WeatherDataController _controller;
	private SettingsDoc _settings;
	private Audio _audio;
	

	[Inject]
	public void InjectDependencies(WeatherDataController weatherData, SettingsDoc settings, Audio audio)
	{
		_controller = weatherData;
		_settings = settings;
		_audio = audio;
	}

	private void Awake()
	{
		_doc = GetComponent<UIDocument>();
	}

	private void OnEnable()
	{
		_rows = _doc.rootVisualElement.Q<VisualElement>("CardsContainer")
			.Query<VisualElement>("RowTemplate")
			.ToList()
			.Select(i => i.Q<VisualElement>("Container"))
			.ToList();
		List<VisualElement> cardsList = _rows.SelectMany(r => r.Query<VisualElement>("WeatherCard").ToList()).ToList();

		_items = _controller.CreateWeatherCards(cardsList);
		LoadData();
		var reset = _doc.rootVisualElement.Q<Button>("ResetButton");
		reset.clicked += () =>
		{
			CardsReset();
			_audio.OnClickMusic();
		};

		_doc.rootVisualElement.Q<Button>("SettingsButton").clicked += () =>
		{
			_settings.gameObject.SetActive(true);
			_audio.OnClickMusic();
		};

		_doc.rootVisualElement.Q<Button>("CloseButton").clicked += () =>
		{
			SceneManager.LoadScene("FirstScene");
			_audio.OnClickMusic();
		};

		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(reset, AnimationType.Growing, new Dictionary<AnimationDataType, object>()
			{
				{AnimationDataType.GrowingValue, 1.2f }
			});
		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(reset, AnimationType.BackgroundColorChanging, new Dictionary<AnimationDataType, object>()
			{
				{ AnimationDataType.ColorToChange, Properties.ButtonChangedColor }
			});
		
		
		StartCoroutine(AddEmptyCards(_cardTemplate.CloneTree()));
	}

	private void LoadData()
	{
		
		foreach (var item in _items)
		{
			if (item.Weather != null)
			{
				item.WeatherCardItem.RegisterCallback<MouseDownEvent>((e) =>
				{
					_audio.OnClickMusic();
				});
				StartCoroutine(LoadIcon(item));
			}
		}


		foreach (var item in _items)
		{
			if (item.Weather != null)
			{
				item.WeatherCardItem.RegisterCallback<PointerUpEvent>((e) =>
				{
					WeatherCardBehaviour(item);
				});
			}
		}

		_loaded = true;
	}

	private IEnumerator AddEmptyCards(VisualElement template)
	{
		yield return new WaitForEndOfFrame();
		foreach (var item in _items)
		{
			if (item.Weather == null)
			{
				item.WeatherCardItem = CreateEmptyElement();
				_rows[item.RowGridPosition].RemoveAt(item.ColGridPosition);
				_rows[item.RowGridPosition].Insert(item.ColGridPosition, item.WeatherCardItem);
			}
		}
	}

	private IEnumerator LoadIcon(WeatherCard item)
	{
		using (UnityWebRequest imgRequest = UnityWebRequestTexture.GetTexture(item.Weather.TextureUrl))
		{
			yield return imgRequest.SendWebRequest();
			yield return new WaitForEndOfFrame();
			if (imgRequest.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(imgRequest.error);
			}
			else
			{
				var texture = DownloadHandlerTexture.GetContent(imgRequest);
				item.WeatherCardItem.Q<VisualElement>("icon").style.backgroundImage = new StyleBackground(texture);
			}
		}
	}

	private void WeatherCardBehaviour(WeatherCard card)
	{
		_rows[card.RowGridPosition].RemoveAt(card.ColGridPosition);
		if (card.Weather != null)
			_removedCards.Add(card);
		for (int k = card.RowGridPosition; k < 2; k++)
		{
			_rows[k].Insert(card.ColGridPosition, _rows[k + 1][card.ColGridPosition]);
			_items[k, card.ColGridPosition] = _items[k + 1, card.ColGridPosition];
			_items[k, card.ColGridPosition].RowGridPosition--;
		}

		var element = CreateEmptyElement();
		_rows[2].Insert(card.ColGridPosition, element);
		_items[2, card.ColGridPosition] = new WeatherCard
		{
			Weather = null,
			WeatherCardItem = element,
			RowGridPosition = 2,
			ColGridPosition = card.ColGridPosition
		};
	}

	private void CardsReset() {
		if (_loaded)
		{
			List<VisualElement> cards = new List<VisualElement>();
			for (int i = 0; i < 9; i++)
			{
				cards.Add(_cardTemplate.CloneTree());
			}
			_items = _controller.ResetCards(cards);
			foreach (VisualElement row in _rows)
				row.Clear();
			LoadData();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					_rows[i].Add(_items[i, j].WeatherCardItem);
					SetCardSize(_items[i, j].WeatherCardItem);
				}
			}
		}
	}

	private void SetCardSize(VisualElement element)
	{
		element.style.width = 300;
		element.style.height = 300;
	}

	private VisualElement CreateEmptyElement()
	{
		var element = new VisualElement();
		element.style.width = 300;
		element.style.height = 300;
		return element;
	}

	private void OnDisable()
	{
		if (_loaded)
		{
			_controller.SaveData(_items);
		}
	}
}
