using UnityEngine;

#region ENUMS
[System.Serializable]
public enum ENTITY_TYPE
{
    MOVABLE,
    NO_MOVABLE,
<<<<<<< HEAD
    JUMPABLE
=======
    TRAP
>>>>>>> 40d2dbca8bd262d0db01d88d8a43434a34bff035
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
    public string Id { get => id; }
    public ENTITY_TYPE Type { get => type; }
    public GridIndex Index { get => index; }
    public RotationModel Rotation { get => rotation; }
    #endregion
}
