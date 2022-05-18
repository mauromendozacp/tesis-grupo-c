using UnityEngine;

[System.Serializable]
public class RotationModel
{
    [SerializeField] private int x = 0;
    [SerializeField] private int y = 0;
    [SerializeField] private int z = 0;

    public int X { get => x; }
    public int Y { get => y; }
    public int Z { get => z; }
}