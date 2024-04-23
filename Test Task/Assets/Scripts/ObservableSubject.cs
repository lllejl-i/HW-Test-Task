using System;
using System.Collections;
using UnityEngine;

public class ObservableSubject : MonoBehaviour, ISubject
{
	private event Action<object> OnNotify;
	private void Start()
	{
		AddObserver(RadialProgressBar.ProgressBar);
	}

	public void AddObserver(IObserver observer)
	{
		OnNotify += observer.SendMessage;
	}

	public void Notify(object data)
	{
		OnNotify?.Invoke(data);
	}

	public void RemoveObserver(IObserver observer)
	{
		OnNotify -= observer.SendMessage;
	}
}
