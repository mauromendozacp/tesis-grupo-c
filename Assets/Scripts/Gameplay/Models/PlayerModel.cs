using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private int lives = 0;
    [SerializeField] private int turns = 0;
    [SerializeField] private int i = 0;
    [SerializeField] private int j = 0;
    [SerializeField] private Vector3 rotation = Vector3.forward;
    #endregion

    #region PROPERTIES
    public int Lives { get => lives; set => lives = value; }
    public int Turns { get => turns; set => turns = value; }
    public int I { get => i; set => i = value; }
    public int J { get => j; set => j = value; }
    public Vector3 Rotation { get => rotation; set => rotation = value; }
    #endregion
}