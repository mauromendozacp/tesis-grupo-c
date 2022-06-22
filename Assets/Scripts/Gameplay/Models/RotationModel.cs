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
    public float X { get => x;set=>x=value; }
    public float Y { get => y; set=>y=value;}
    public float Z { get => z;set=>z=value; }
    #endregion
}