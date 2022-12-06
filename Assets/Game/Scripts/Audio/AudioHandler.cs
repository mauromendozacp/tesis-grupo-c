using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler: MonoBehaviourSingleton<AudioHandler>
{
    private AudioSource musicAudioSource;
    private AudioSource sfxAudioSource;
    private AudioMixer audioMixer;

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

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}