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
    [SerializeField] private GameObject box = null;
    #endregion

    #region ACTIONS
    private Func<GridIndex, bool> onCheckGridIndex = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        onCheckGridIndex += CheckIndex;

        player.Init(onCheckGridIndex, unit);

        GameObject go = Instantiate(box, new Vector3(0, 0, 1), Quaternion.identity);
        go.GetComponent<Box>().Init(onCheckGridIndex, unit);
    }
    #endregion

    #region PRIVATE_FIELDS
    private bool CheckIndex(GridIndex index)
    {
        return index.i >= 0 && index.j >= 0 && index.i < gridIndex.i && index.j < gridIndex.j;
    }
    #endregion
}