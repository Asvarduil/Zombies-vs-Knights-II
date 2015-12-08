using UnityEngine;
using System.Collections;

public class Maestro : DebuggableBehavior
{
    #region Variables / Properties

    private AudioSource _soundSource;

    private AudioManager _audioManager;
    private AudioManager AudioManager
    {
        get
        {
            if (_audioManager == null)
                _audioManager = AudioManager.Instance;

            if (_audioManager == null)
                _audioManager = FindObjectOfType<AudioManager>();

            return _audioManager;
        }
    }

    public static Maestro Instance
    {
        get { return FindObjectOfType<Maestro>(); }
    }

    #endregion Variables / Properties

    #region Engine Hooks

    public void Start()
    {
        _soundSource = gameObject.GetComponentInChildren<AudioSource>();
    }

    public void Update()
    {
        //FormattedDebugMessage(LogLevel.Info, "Is AudioManager null?  Answer: {0}", AudioManager == null);

        AudioListener.volume = AudioManager.EffectiveMasterVolume;
        _soundSource.volume = AudioManager.MusicVolume;
    }

    #endregion Engine Hooks

    #region Methods

    public void StopBGM()
    {
        _soundSource.Stop();
    }

    public void ResumeBGM()
    {
        _soundSource.time = 0.0f;
        _soundSource.Play();
    }

    public void PlayOneShot(AudioClip oneShot, float? effectVolume = null)
    {
        if (oneShot == null)
            return;

        if (effectVolume == null)
            effectVolume = AudioManager.EffectVolume;

        _soundSource.PlayOneShot(oneShot, (float)effectVolume);
    }

    public void ChangeTunes(AudioClip newTune)
    {
        if (newTune == null)
            return;

        _soundSource.clip = newTune;
        _soundSource.time = 0.0f;
    }

    public void InterjectTune(AudioClip tempTune, float switchTime = 0.1f)
    {
        PlayOneShotTune(tempTune, switchTime);
    }

    private IEnumerator PlayOneShotTune(AudioClip tempTune, float switchTime = 0.1f)
    {
        if (tempTune == null)
            yield break;

        // Halt Audio
        GetComponent<AudioSource>().Stop();
        yield return 0;

        // Play the one-shot in its entirety
        float resumeTime = Time.time + tempTune.length + switchTime;
        _soundSource.PlayOneShot(tempTune, AudioManager.EffectVolume);
        yield return new WaitForSeconds(resumeTime);

        // Restart the old tune.
        GetComponent<AudioSource>().Play();
    }

    #endregion Methods
}
