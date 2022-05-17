using UnityEngine;

[System.Serializable]
public class EntityModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private string id = string.Empty;
    [SerializeField] private int i = 0;
    [SerializeField] private int j = 0;
    #endregion

    #region PROPERTIES
    public string Id { get => id; }
    public int I { get => i; }
    public int J { get => j; }
    #endregion
}
