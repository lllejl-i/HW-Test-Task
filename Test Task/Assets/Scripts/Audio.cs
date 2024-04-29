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
	private bool _playMusic = true;

	public bool PlayMusic => _playMusic;
	public bool MakeSounds => _makeSounds;

	public void ChangePlayingMode(bool playMusic)
	{
		_musicSource.gameObject.SetActive(playMusic);
		_playMusic = playMusic;
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
		_musicSource.volume = volume;
		_clickSource.volume = volume;
	}
}
