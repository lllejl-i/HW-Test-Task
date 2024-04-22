using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;

public class VisualElementGrowing : VisualElementAnimation
{
	private float _maxScale;
	private float _currentScale = 1;

	public VisualElementGrowing(VisualElement element) : base(element)
	{
		_maxScale = 1.2f;
	}

	public override IEnumerator Animate()
	{
		_continue = true;
		while (_currentScale < _maxScale && _continue)
		{
			_currentScale += 0.01f;
			_elementToChange.transform.scale = new Vector3(_currentScale, _currentScale, 1);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public override IEnumerator ClearAnimation()
	{
		_continue = false;
		_currentScale = 1;
		_elementToChange.transform.scale = new Vector3(1, 1, 1);
		yield break;
	}
}