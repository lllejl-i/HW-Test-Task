using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public abstract class VisualElementAnimation : IAnimationComponent
{
	protected bool _continue = false;
	protected VisualElement _elementToChange;
	protected Dictionary<AnimationDataType, object> _parameters;
	public VisualElementAnimation(VisualElement element, Dictionary<AnimationDataType, object> parameters)
	{
		_elementToChange = element;
		_parameters = parameters;
	}
	public abstract IEnumerator Animate();
	public abstract IEnumerator ClearAnimation();
}
