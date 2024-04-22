using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualElementBackgroundChanging : VisualElementAnimation
{
	StyleColor _mainColor;
	public VisualElementBackgroundChanging(VisualElement element) : base(element)
	{
		_mainColor = element.style.backgroundColor;
	}

	public override IEnumerator Animate()
	{
		ColorUtility.TryParseHtmlString("#EBE79D", out var color);
		_elementToChange.style.backgroundColor = color;
		yield break;
	}

	public override IEnumerator ClearAnimation()
	{
		_elementToChange.style.backgroundColor = _mainColor;
		yield break;
	}
}