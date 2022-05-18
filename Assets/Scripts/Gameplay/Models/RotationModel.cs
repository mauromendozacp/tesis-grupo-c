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
}