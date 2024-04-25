using System.Collections;
using UnityEngine.UIElements;

public abstract class VisualElementAnimation : IAnimationComponent
{
	protected bool _continue = false;
	protected VisualElement _elementToChange;
	public VisualElementAnimation(VisualElement element)
	{
		_elementToChange = element;
	}
	public abstract IEnumerator Animate();
	public abstract IEnumerator ClearAnimation();
}
