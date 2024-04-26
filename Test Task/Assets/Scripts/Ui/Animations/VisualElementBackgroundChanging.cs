using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualElementBackgroundChanging : VisualElementAnimation
{
	StyleColor _mainColor;
	public VisualElementBackgroundChanging(VisualElement element, Dictionary<AnimationDataType, object> parameters) : base(element, parameters)
	{
		_mainColor = element.style.backgroundColor;
	}

	public override IEnumerator Animate()
	{
		ColorUtility.TryParseHtmlString((string)_parameters[AnimationDataType.ColorToChange], out var color);
		_elementToChange.style.backgroundColor = color;
		yield break;
	}

	public override IEnumerator ClearAnimation()
	{
		_elementToChange.style.backgroundColor = _mainColor;
		yield break;
	}
}