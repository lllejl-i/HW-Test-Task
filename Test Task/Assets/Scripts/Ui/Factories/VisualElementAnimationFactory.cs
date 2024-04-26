using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
public class VisualElementAnimationFactory
{
	private static VisualElementAnimationFactory _factory = new VisualElementAnimationFactory();
	public static VisualElementAnimationFactory Factory => _factory;
	private VisualElementAnimationFactory() { }

	public VisualElementAnimation CreateAnimationComponent(VisualElement element, 
														AnimationType type,
														Dictionary<AnimationDataType, object> parameters)
	{
		switch (type)
		{
			case AnimationType.Growing:
				return new VisualElementGrowing(element, parameters);
			case AnimationType.BackgroundColorChanging:
				return new VisualElementBackgroundChanging(element, parameters);
			default:
				throw new Exception("Undeclareted animation type");
		}
	}
}