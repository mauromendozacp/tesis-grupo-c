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
    
    public void PlayAudio(AudioEvent audioEvent)
    {
        switch (audioEvent.AudioType)
        {
            case AudioEvent.AUDIO_TYPE.SFX:
                sfxAudioSource.PlayOneShot(audioEvent.Clip);
                break;
            case AudioEvent.AUDIO_TYPE.MUSIC:
                musicAudioSource.clip = audioEvent.Clip;
                musicAudioSource.Play();
                break;
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}