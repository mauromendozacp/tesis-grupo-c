using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject hudPanel = null;
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject gameoverPanel = null;
    [SerializeField] private Button pauseButton = null;
    #endregion

    #region PUBLIC_METHODS
    public void PauseToggleStatus(bool status)
    {
        pausePanel.SetActive(status);
        Time.timeScale = status ? 0f : 1f;
    }

    public void OpenGameoverPanel()
    {
        gameoverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        gameoverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        TransitionManager.Get().ChangeScene(SceneGame.MainMenu);
    }
    #endregion
}
