using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private GridIndex index = default;
    [SerializeField] private RotationModel rotation = null;
    #endregion

    #region PROPERTIES
    public GridIndex Index { get => index; set => index = value; }
    public RotationModel Rotation { get => rotation; set => rotation = value; }
    #endregion
}