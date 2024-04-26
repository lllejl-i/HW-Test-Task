using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public enum AnimationDataType
{
	GrowingValue,
	ColorToChange
}

public abstract class AnimatedToolkitPage : MonoBehaviour
{
	public virtual void AddAnimation<AEventType, LEventType>(VisualElement element,
															AnimationType type,
															Dictionary<AnimationDataType, object> parameters)
		where AEventType : EventBase<AEventType>, new()
		where LEventType : EventBase<LEventType>, new()
	{
		VisualElementAnimation animation = null;
		element.RegisterCallback<AEventType>((e) =>
		{
			animation = VisualElementAnimationFactory.Factory.CreateAnimationComponent(element, type, parameters);
			StartCoroutine(animation.Animate());
		});
		element?.RegisterCallback<LEventType>((e) =>
		{
			StartCoroutine(animation?.ClearAnimation());
		});
	}
}