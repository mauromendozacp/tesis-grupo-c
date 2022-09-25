using System;
using UnityEngine;

[Serializable]
public struct GridIndex
{
    #region PUBLIC_FIELDS
    public int i;
    public int j;
    #endregion

    #region PUBLIC_METHODS
    public GridIndex(int i, int j)
    {
        this.i = i;
        this.j = j;
    }
    #endregion

    #region OPERATORS
    public static bool operator ==(GridIndex index1, GridIndex index2)
    {
        return index1.i == index2.i && index1.j == index2.j;
    }

    public static bool operator !=(GridIndex index1, GridIndex index2)
    {
        return !(index1 == index2);
    }
    #endregion

    #region INTERNALS
    public override bool Equals(object other)
    {
        if (!(other is GridIndex)) return false;
        return Equals((GridIndex)other);
    }

    public bool Equals(GridIndex other)
    {
        return i == other.i && j == other.j;
    }

    public override int GetHashCode()
    {
        return i.GetHashCode() ^ (j.GetHashCode() << 2);
    }
    #endregion
}

public class GameManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private LevelController levelController = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private HUD hud = null;
    [SerializeField] private GameplayUI gameplayUI = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        Init();
        StartGame();
    }
    #endregion

    #region PRIVATE_METHODS
    private void Init()
    {
        hud.Init();
        gameplayUI.Init();
        levelController.Init(hud.HUDActions, gameplayUI.GUIActions, SetPlayerActions, PlayerDeath, () => { CameraFollowStatus(true); });

        gameplayUI.GUIActions.onRestart += levelController.RestartGame;
        gameplayUI.GUIActions.onRestart += () => { cameraController.Follow = true; };
    }

    private void StartGame()
    {
        levelController.StartGrid();
    }

    private void SetPlayerActions()
    {
        cameraController.Target = levelController.PlayerController.transform;

        hud.HUDActions.onEnableLight = levelController.PlayerController.TurnLight;
        hud.HUDActions.onEnableUnlimitedTurns = levelController.PlayerController.EnableUnlimitedTurns;
    }

    private void PlayerDeath()
    {
        CameraFollowStatus(false);
        levelController.PlayerController.PlayerDeath();
    }

    private void CameraFollowStatus(bool status)
    {
        cameraController.Follow = status;
    }
    #endregion
}