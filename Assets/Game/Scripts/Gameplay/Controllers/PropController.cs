using UnityEngine;

public class PropController : MonoBehaviour
{
    #region PRIVATE_FIELDS
    protected Vector3 spawnPosition = Vector3.zero;
    #endregion

    #region PUBLIC_METHODS
    protected void SetSpawnIndex(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    public virtual void Restart()
    {
        transform.position = spawnPosition;
    }
    #endregion
}
