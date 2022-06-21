using UnityEngine;

#region ENUMS
[System.Serializable]
public enum ENTITY_TYPE
{
    MOVABLE,
    NO_MOVABLE,
    TRAP,
    JUMPABLE
}
#endregion

[System.Serializable]
public class EntityModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private string id = string.Empty;
    [SerializeField] private ENTITY_TYPE type = default;
    [SerializeField] private GridIndex index = default;
    [SerializeField] private RotationModel rotation = default;
    #endregion

    #region PROPERTIES
    public string Id { get => id; set => id = value; }
    public ENTITY_TYPE Type { get => type; set => type = value; }
    public GridIndex Index { get => index; set => index = value; }
    public RotationModel Rotation { get => rotation; set => rotation = value; }
    #endregion
}