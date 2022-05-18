using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

#region ENUMS
[System.Serializable]
public enum ENTITY_TYPE
{
    MOVABLE,
    NO_MOVABLE
}
#endregion

[System.Serializable]
public class EntityModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private string id = string.Empty;
    [SerializeField] private ENTITY_TYPE type = default;
    [SerializeField] private int i = 0;
    [SerializeField] private int j = 0;
    #endregion

    #region PROPERTIES
    public string Id { get => id; }
    public ENTITY_TYPE Type { get => type; }
    public int I { get => i; }
    public int J { get => j; }
    #endregion
}
