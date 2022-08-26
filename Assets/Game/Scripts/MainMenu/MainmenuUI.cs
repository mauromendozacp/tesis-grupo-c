using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainmenuUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject mainmenuPanel = null;
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject creditsPanel = null;
    #endregion

    #region PUBLIC_FIELDS
    public void PlayGame()
    {
        TransitionManager.Get().ChangeScene(SceneGame.GamePlay);
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
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
