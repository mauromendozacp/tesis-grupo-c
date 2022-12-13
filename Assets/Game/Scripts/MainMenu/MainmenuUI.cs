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
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioEvent musicAudio;
    [SerializeField] private AudioEvent clickAudio;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        AudioHandler.Get().Setup(audioMixer, musicSource, sfxSource);
        AudioHandler.Get().PlayAudio(musicAudio);
    }
    #endregion

    #region PUBLIC_FIELDS
    public void PlayGame()
    {
        ClickButton();
        SceneController.Get().ChangeScene(SceneGame.GamePlay);
    }

    public void OpenMainmenuPanel()
    {
        ClickButton();
        mainmenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void OpenOptionsPanel()
    {
        ClickButton();
        optionsPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        ClickButton();
        creditsPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
    }

    public void OpenAudioPanel()
    {
        ClickButton();
        audioPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        ClickButton();
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        ClickButton();
        Application.Quit();
    }
    #endregion

    #region PRIVATE_METHODS
    private void ClickButton()
    {
        AudioHandler.Get().PlayAudio(clickAudio, 0);
    }
    #endregion
}
