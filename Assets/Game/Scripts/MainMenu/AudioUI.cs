using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider sfxSlider = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        musicSlider.value = AudioHandler.Get().GetMusicVolume();
        sfxSlider.value = AudioHandler.Get().GetSfxVolume();
    }
    #endregion

    #region PRIVATE_METHODS
    private void OnMusicVolumeChanged(float volume)
    {
        musicSlider.value = volume;
        AudioHandler.Get().SetMusicVolume(volume);
    }

    private void OnSfxVolumeChanged(float volume)
    {
        sfxSlider.value = volume;
        AudioHandler.Get().SetSfxVolume(volume);
    }
    #endregion
}
