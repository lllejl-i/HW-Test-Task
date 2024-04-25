using System;
using UnityEngine.UIElements;
public class VisualElementAnimationFactory
{
	private static VisualElementAnimationFactory _factory = new VisualElementAnimationFactory();
	public static VisualElementAnimationFactory Factory => _factory;
	private VisualElementAnimationFactory() { }

	public VisualElementAnimation CreateAnimationComponent(VisualElement element, AnimationType type)
	{
		switch (type)
		{
			case AnimationType.Growing:
				return new VisualElementGrowing(element);
			case AnimationType.BackgroundColorChanging:
				return new VisualElementBackgroundChanging(element);
			default:
				throw new Exception("Undeclareted animation type");
		}
	}
}