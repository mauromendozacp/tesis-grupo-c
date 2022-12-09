using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler: MonoBehaviourSingleton<AudioHandler>
{
    #region PRIVATE_FIELDS
    private AudioSource musicAudioSource = null;
    private AudioSource sfxAudioSource = null;
    private AudioMixer audioMixer = null;
    #endregion

    #region CONSTANTS_FIELDS
    private const string sfxVolumeKey = "sfxVolume";
    private const string musicVolumeKey = "musicVolume";

    private const float offsetVolume = 80f;
    private const float maxVolume = 80f;
    #endregion

    public void Setup(AudioMixer audioMixer, AudioSource musicAudioSource, AudioSource sfxAudioSource)
    {
        this.audioMixer = audioMixer;
        this.sfxAudioSource = sfxAudioSource;
        this.musicAudioSource = musicAudioSource;

        musicAudioSource.loop = true;
    }
    
    public void PlayAudio(AudioEvent audioEvent, int index = -1)
    {
        switch (audioEvent.AudioType)
        {
            case AudioEvent.AUDIO_TYPE.SFX:
                sfxAudioSource.PlayOneShot(audioEvent.GetClip(index));
                break;
            case AudioEvent.AUDIO_TYPE.MUSIC:
                musicAudioSource.clip = audioEvent.GetClip(index);
                musicAudioSource.Play();
                break;
        }
    }

    public void PlayMusic(AudioEvent audioEvent, int index = -1)
    {
        musicAudioSource.clip = audioEvent.GetClip(index);
        musicAudioSource.Play();
    }

    public void PlaySound(AudioEvent audioEvent, AudioSource audioSource, int index = -1)
    {
        audioSource.PlayOneShot(audioEvent.GetClip(index));
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeKey, GetRealVolume(volume));
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeKey, GetRealVolume(volume));
    }

    public float GetMusicVolume()
    {
        if (audioMixer.GetFloat(musicVolumeKey, out float volume))
        {
            return GetPercVolume(volume);
        }

        return 0f;
    }

    public float GetSfxVolume()
    {
        if (audioMixer.GetFloat(sfxVolumeKey, out float volume))
        {
            return GetPercVolume(volume);
        }

        return 0f;
    }

    #region PRIVATE_METHODS
    private float GetRealVolume(float perc)
    {
        return perc * maxVolume - offsetVolume;
    }

    private float GetPercVolume(float real)
    {
        return (real + offsetVolume) / maxVolume;
    }
    #endregion
}