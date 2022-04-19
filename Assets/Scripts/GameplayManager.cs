using System;
using UnityEngine;

[Serializable]
public struct GridIndex
{
    public int i;
    public int j;
}

public class GameplayManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GridIndex gridIndex = default;
    [SerializeField] private float unit = 0f;
    [SerializeField] private Player player = null;
    [SerializeField] private Box box = null; //FOR TESTING
    #endregion

    #region ACTIONS
    private Func<GridIndex, bool> onCheckGridIndex = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        onCheckGridIndex += CheckIndex;

        player.Init(onCheckGridIndex, unit);
        box.Init(onCheckGridIndex, unit);
    }
    #endregion

    #region PRIVATE_FIELDS
    private bool CheckIndex(GridIndex index)
    {
        return index.i >= 0 && index.j >= 0 && index.i < gridIndex.i && index.j < gridIndex.j;
    }
    #endregion
}