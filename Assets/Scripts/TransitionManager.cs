using System;
using UnityEngine.SceneManagement;

public enum SceneGame
{
    MainMenu,
    GamePlay
}

public class TransitionManager : MonoBehaviourSingleton<TransitionManager>
{
    #region PUBLIC_METHODS
    public void ChangeScene(SceneGame scene)
    {
        string sceneName;

        switch (scene)
        {
            case SceneGame.MainMenu:
                sceneName = "Mainmenu";
                break;
            case SceneGame.GamePlay:
                sceneName = "Gameplay";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }

        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
