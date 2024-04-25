using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollbarAutoScrolling : MonoBehaviour
{
    UIDocument _doc;
	ScrollView _scrollView;
	int _currentItemIndex = 0;
	bool _isRightDirrection = true;
	private void Awake()
	{
		_doc = GetComponent<UIDocument>();
	}

	private void OnEnable()
	{
		_scrollView = _doc.rootVisualElement.Q<ScrollView>("Scroller");
		_scrollView.RegisterCallback<WheelEvent>((e) =>
		{
			e.StopPropagation();
		}, TrickleDown.TrickleDown);
		_scrollView.RegisterCallback<PointerDownEvent>((e) =>
		{
			e.StopPropagation();
		}, TrickleDown.TrickleDown);
		var allItems = _scrollView.Q<VisualElement>("unity-content-container").Query<VisualElement>().ToList();
		allItems.RemoveAt(0);
		StartScrolling(allItems);
	}

	public void StartScrolling(List<VisualElement> items)
	{
		_scrollView.RegisterCallback<WheelEvent>((e) => {
			if(e.delta.y > 0 && _currentItemIndex < items.Count - 1)
			{
				_scrollView.ScrollTo(items[_currentItemIndex + 1]);
				_currentItemIndex++;
			}
			else if (e.delta.y < 0 && _currentItemIndex > 0) {
				_scrollView.ScrollTo(items[_currentItemIndex - 1]);
				_currentItemIndex--;
			}
		}, TrickleDown.TrickleDown);
		StartCoroutine(AutoScroll(items));
	}

	private IEnumerator AutoScroll(List<VisualElement> items)
	{
		while (enabled)
		{
			yield return new WaitForSeconds(3.5f);
			if (_currentItemIndex == items.Count - 1 && _isRightDirrection)
			{
				_isRightDirrection = false;
			}
			else if (_currentItemIndex == 0 && !_isRightDirrection)
			{
				_isRightDirrection = true;
			}
			_currentItemIndex += (_isRightDirrection ? 1 : -1);
			_scrollView.ScrollTo(items[_currentItemIndex]);
		}
	}
}
