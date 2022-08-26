using UnityEngine;

[System.Serializable]
public class LevelModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private GridIndex winIndex = default;
    [SerializeField] private GridIndex limitIndex = default;
    [SerializeField] private PlayerModel playerModel = null;
    [SerializeField] private LayerModel[] layers = null;
    #endregion

    #region PROPERTIES
    public GridIndex WinIndex { get => winIndex; set => winIndex = value; }
    public GridIndex LimitIndex { get => limitIndex; set => limitIndex = value; }
    public PlayerModel PlayerModel { get => playerModel; set => playerModel = value; }
    public LayerModel[] Layers { get => layers; set => layers = value; }
    #endregion
}
