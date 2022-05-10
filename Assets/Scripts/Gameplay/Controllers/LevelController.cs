using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("General Info")]
    [SerializeField] private Transform environmentHolder = null;
    [SerializeField] private GridIndex gridIndex = default;
    [SerializeField] private GridIndex winIndex = default;
    [SerializeField] private float unit = 0f;

    [Header("Prefabs"), Space]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject box = null;
    [SerializeField] private GameObject floor = null;

    [Header("Player"), Space]
    [SerializeField] private int lives = 0;
    [SerializeField] private int turns = 0;
    [SerializeField] private GridIndex spawn = default;
    #endregion

    #region PROPERTIES
    public GridIndex WinIndex { get => winIndex; }
    #endregion

    #region PUBLIC_METHODS
    public void Init()
    {
        SpawnGrid();
    }

    public PlayerController SpawnPlayer(GUIActions guiActions, Action<GridIndex> onCheckIndexPlayer)
    {
        PlayerModel playerModel = new PlayerModel
        {
            Lives = lives,
            Turns = turns,
            Index = spawn
        };

        PlayerController playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Init(guiActions, CheckIndex, onCheckIndexPlayer, unit);
        playerController.SetData(playerModel);
        playerController.SetPositionUnit(playerModel.Index);

        return playerController;
    }
    #endregion

    #region PRIVATE_METHODS
    private void SpawnGrid()
    {
        GameObject floorParent = new GameObject("Floor");
        floorParent.transform.parent = environmentHolder;

        for (int i = gridIndex.i - 1; i >= 0; i--)
        {
            for (int j = gridIndex.j - 1; j >= 0; j--)
            {
                Vector3 pos = new Vector3(i, -1, j);
                Instantiate(floor, pos, Quaternion.identity, floorParent.transform);
            }
        }

        GameObject go = Instantiate(box, new Vector3(0, 0, 1), Quaternion.identity, environmentHolder);
        go.GetComponent<Box>().Init(CheckIndex, unit);
    }

    private bool CheckIndex(GridIndex index)
    {
        return index.i >= 0 && index.j >= 0 && index.i < gridIndex.i && index.j < gridIndex.j;
    }
    #endregion
}