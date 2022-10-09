using UnityEngine;

[CreateAssetMenu(fileName = "AudioEvent_", menuName = "AudioEvent", order = 1)]
public class AudioEvent : ScriptableObject
{
    public enum AUDIO_TYPE
    {
        SFX,
        MUSIC
    }

    [SerializeField] private AUDIO_TYPE audioType = AUDIO_TYPE.SFX;
    [SerializeField] private AudioClip clip = null;
    [SerializeField] private bool useRandomClip = false;
    [SerializeField] private AudioClip[] clips = null;

    [SerializeField] [Range(0, 1)] private float volume = 1.0f;

    [SerializeField] private float pitch = 1.0f;
    [SerializeField] private bool useRandomPitch = false;
    [SerializeField] private float minPitch = 0.0f;
    [SerializeField] private float maxPitch = 1.0f;

    public AUDIO_TYPE AudioType { get => audioType; }
    public AudioClip Clip { get => useRandomClip ? clips[Random.Range(0, clips.Length)] : clip; }
    public float Volume { get => volume; }
    public float Pitch { get => useRandomPitch ? Random.Range(minPitch, maxPitch) : pitch; }
}