using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIActions
{
    public Action onOpenGameoverPanel = null;
    public Action onOpenWinPanel = null;
    public Action onRestart = null;
}

public class GameplayUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject hudPanel = null;
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject gameoverPanel = null;
    [SerializeField] private GameObject winPanel = null;
    #endregion

    #region PRIVATE_FIELDS
    private GUIActions guiActions = null;
    #endregion

    #region PROPERTIES
    public GUIActions GUIActions { get => guiActions; }
    #endregion

    #region PUBLIC_METHODS
    public void Init()
    {
        guiActions = new GUIActions();
        guiActions.onOpenGameoverPanel = OpenGameoverPanel;
        guiActions.onOpenWinPanel = OpenWinPanel;
    }

    public void PauseToggleStatus(bool status)
    {
        pausePanel.SetActive(status);
        Time.timeScale = status ? 0f : 1f;
    }

    public void OpenGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void Retry()
    {
        guiActions.onRestart?.Invoke();
        gameoverPanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneController.Get().ChangeScene(SceneGame.MainMenu);
    }
    #endregion
}
