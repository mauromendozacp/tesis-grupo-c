using System;
using UnityEngine;
[Serializable]
public class DoorData 
{
    public int levelToLoad;
    public GridIndex triggerIndex;
    public Vector2 spawnPos;
    public Vector3 spawnDir;
}