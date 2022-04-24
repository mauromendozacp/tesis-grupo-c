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

public class GameplayManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GridIndex gridIndex = default;
    [SerializeField] private GridIndex winIndex = default;
    [SerializeField] private float unit = 0f;
    [SerializeField] private GameplayUI gameplayUI = null;

    [Header("Prefabs Configuration"), Space]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject box = null;
    [SerializeField] private GameObject floor = null;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerController playerController = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        StartGamepleyUI();
        SpawnPlayer();

        GameObject go = Instantiate(box, new Vector3(0, 0, 1), Quaternion.identity);
        go.GetComponent<Box>().Init(CheckIndex, unit);

        for (int i = 0; i < gridIndex.i; i++)
        {
            for (int j = 0; j < gridIndex.j; j++)
            {
                Vector3 pos = new Vector3(gridIndex.i - 1 - i, -1, gridIndex.j - 1 - j);
                Instantiate(floor, pos, Quaternion.identity);
            }
        }
    }
    #endregion

    #region PRIVATE_FIELDS
    private void StartGamepleyUI()
    {
        gameplayUI.Init();
    }

    private void SpawnPlayer()
    {
        PlayerModel playerModel = new PlayerModel();
        playerModel.Lives = 3;
        playerModel.Turns = 10;
        playerModel.Index = new GridIndex(3, 3);

        playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Init(gameplayUI.GUIActions, CheckIndex, CheckIndexPlayer, unit);
        playerController.SetData(playerModel);
        playerController.SetPositionUnit(playerModel.Index);
    }

    private bool CheckIndex(GridIndex index)
    {
        return index.i >= 0 && index.j >= 0 && index.i < gridIndex.i && index.j < gridIndex.j;
    }

    private void CheckIndexPlayer(GridIndex index)
    {
        if (index == winIndex)
        {
            playerController.InputEnabled = false;
            Debug.Log("Win");
            return;
        }

        if (playerController.Model.Turns <= 0)
        {
            playerController.InputEnabled = false;
            Debug.Log("Lose");
        }
    }
    #endregion
}