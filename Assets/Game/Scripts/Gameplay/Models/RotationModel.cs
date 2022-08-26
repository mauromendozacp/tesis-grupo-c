using UnityEngine;

[System.Serializable]
public class RotationModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private float x = 0;
    [SerializeField] private float y = 0;
    [SerializeField] private float z = 0;
    #endregion

    #region PROPERTIES
    public float X { get => x; }
    public float Y { get => y; }
    public float Z { get => z; }
    #endregion

    #region CONSTRUCTS
    public RotationModel(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public RotationModel(Vector3 rot)
    {
        x = rot.x;
        y = rot.y;
        z = rot.z;
    }
    #endregion
}