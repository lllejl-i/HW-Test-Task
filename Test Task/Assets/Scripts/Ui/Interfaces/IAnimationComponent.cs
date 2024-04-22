using System.Collections;

public interface IAnimationComponent
{
	public IEnumerator Animate();
	public IEnumerator ClearAnimation();
}