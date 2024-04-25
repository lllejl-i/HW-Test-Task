using UnityEngine;

public class Audio : MonoBehaviour
{
	[SerializeField]
	private AudioClip _clickSound;
	[SerializeField]
    private AudioSource _musicSource;
	[SerializeField]
	private AudioSource _clickSource;
	private bool _makeSounds = true;

	public void ChangePlayingMode(bool playMusic)
	{
		_musicSource.gameObject.SetActive(playMusic);
	}

	public void ChangeSoundsMode(bool playSounds)
	{
		_makeSounds = playSounds;
	}

	public void OnClickMusic()
	{
		if (_makeSounds)
		{
			_clickSource.PlayOneShot(_clickSound);
		}
	}

	public void ChangeVolume(float volume)
	{
		Debug.Log($"Volume {volume}");
		_musicSource.volume = volume/100;
		_clickSource.volume = volume/100;
	}
}
