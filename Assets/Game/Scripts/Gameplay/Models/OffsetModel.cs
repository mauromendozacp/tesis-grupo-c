using UnityEngine;

[System.Serializable]
public class OffsetModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private float x = 0;
    [SerializeField] private float z = 0;
    #endregion

    #region PROPERTIES
    public float X { get => x; }
    public float Z { get => z; }
    #endregion

    #region CONSTRUCTS
    public OffsetModel(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
    #endregion
}