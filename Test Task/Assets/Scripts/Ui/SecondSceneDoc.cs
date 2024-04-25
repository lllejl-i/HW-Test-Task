using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SecondSceneDoc : AnimatedToolkitPage
{
	private UIDocument _doc;
	private List<VisualElement> _rows;
	private WeatherCard[,] _items;
	private List<WeatherCard> _removedCards = new List<WeatherCard>();
	private bool _loaded = false;
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
		 
		_items = WeatherDataController.Controller.CreateWeatherCards(cardsList);

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

		var button = _doc.rootVisualElement.Q<Button>("ResetButton");
		button.clicked += () =>
		{
			CardsReset();
		};
		_doc.rootVisualElement.Q<Button>("CloseButton").clicked += () =>
		{
			SceneManager.LoadScene("FirstScene");
		};

		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(button, AnimationType.Growing);
		AddAnimation<MouseEnterEvent, MouseLeaveEvent>(button, AnimationType.BackgroundColorChanging);
		StartCoroutine(AddEmptyCards(cardsList[0]));
	}

	private IEnumerator AddEmptyCards(VisualElement template)
	{
		yield return new WaitForEndOfFrame();
		foreach (var item in _items)
		{
			if (item.Weather == null)
			{
				item.WeatherCardItem = CreateEmptyElement(template);
				_rows[item.RowGridPosition].RemoveAt(item.ColGridPosition);
				_rows[item.RowGridPosition].Insert(item.ColGridPosition, item.WeatherCardItem);
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

		var element = CreateEmptyElement(card.WeatherCardItem);
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
			var temp = WeatherDataController.Controller.GetCards();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (_items[i, j].Weather != null)
						_removedCards.Add(_items[i, j]);
				}
			}

			foreach (var item in temp)
			{
				foreach (var removedItem in _removedCards)
				{
					if (item.WeatherCardItem == removedItem.WeatherCardItem)
					{
						removedItem.ColGridPosition = item.ColGridPosition;
						removedItem.RowGridPosition = item.RowGridPosition;
					}
				}
			}
			foreach (var item in _removedCards)
			{
				_items[item.RowGridPosition, item.ColGridPosition] = item;
			}
			for (int i = 0; i < 3; i++)
			{
				_rows[i].Clear();
				for (int j = 0; j < 3; j++)
				{
					_rows[i].Add(_items[i, j].WeatherCardItem);
				}
			}
		}
	}

	private VisualElement CreateEmptyElement(VisualElement item)
	{
		var element = new VisualElement();
		element.style.marginBottom = item.resolvedStyle.marginBottom;
		element.style.marginLeft = item.resolvedStyle.marginLeft;
		element.style.marginTop = item.resolvedStyle.marginTop;
		element.style.marginRight = item.resolvedStyle.marginRight;
		element.style.width = item.resolvedStyle.width;
		element.style.height = item.resolvedStyle.height;
		return element;
	}

	private void OnDisable()
	{
		if (_loaded)
		{
			WeatherDataController.Controller.SaveData(_items);
		}
	}
}

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

public class Weather : ICloneable
{
	public string City;
	public string WeatherType;
	public string Temperature;

	public object Clone()
	{
		return new Weather()
		{
			City = City,
			WeatherType = WeatherType,
			Temperature = Temperature
		};
	}
}