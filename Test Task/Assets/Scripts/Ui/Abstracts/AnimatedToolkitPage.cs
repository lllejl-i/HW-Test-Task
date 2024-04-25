using UnityEngine.UIElements;
using UnityEngine;

public abstract class AnimatedToolkitPage : MonoBehaviour
{
	public virtual void AddAnimation<AEventType, LEventType>(VisualElement element, AnimationType type)
		where AEventType : EventBase<AEventType>, new()
		where LEventType : EventBase<LEventType>, new()
	{
		VisualElementAnimation animation = null;
		element.RegisterCallback<AEventType>((e) =>
		{
			animation = VisualElementAnimationFactory.Factory.CreateAnimationComponent(element, type);
			StartCoroutine(animation.Animate());
		});
		element?.RegisterCallback<LEventType>((e) =>
		{
			StartCoroutine(animation?.ClearAnimation());
		});
	}
}