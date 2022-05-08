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
    [SerializeField] private UIGameplay uiGameplay = null;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerController playerController = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
       Init();
    }
    #endregion

    #region PRIVATE_METHODS
    private void Init()
    {
        uiGameplay.Init();
        LevelInit();
    }

    private void LevelInit()
    {
        levelController.Init();
        playerController = levelController.SpawnPlayer(uiGameplay.GUIActions, CheckIndexPlayer);
    }

    private void CheckIndexPlayer(GridIndex index)
    {
        if (index == levelController.WinIndex)
        {
            playerController.InputEnabled = false;
            Debug.Log("Win");
            return;
        }

        if (playerController.CheckTurns()) return;

        playerController.InputEnabled = false;
        Debug.Log("Lose");
    }
    #endregion
}