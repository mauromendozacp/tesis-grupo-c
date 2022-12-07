using UnityEngine;
using UnityEngine.Audio;

public class MainmenuUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject mainmenuPanel = null;
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject creditsPanel = null;
    [SerializeField] private GameObject audioPanel = null;
    [SerializeField] private GameObject controlsPanel = null;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioEvent music;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        AudioHandler.Get().Setup(audioMixer, audioSource, audioSource);
        AudioHandler.Get().PlayAudio(music);
    }
    #endregion

    #region PUBLIC_FIELDS
    public void PlayGame()
    {
        SceneController.Get().ChangeScene(SceneGame.GamePlay);
    }

    public void OpenMainmenuPanel()
    {
        mainmenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
    }

    public void OpenAudioPanel()
    {
        audioPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
