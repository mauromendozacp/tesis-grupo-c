using UnityEngine;

public interface IJumpable
{
    public bool TryJump(MOVEMENT movement);
    public bool TryJump(Vector3 movement);
}