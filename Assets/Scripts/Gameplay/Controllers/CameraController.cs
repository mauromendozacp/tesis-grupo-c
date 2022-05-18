using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Vector2 distance = Vector2.zero;
    #endregion

    #region PROPERTIES
    public Transform Target { get; set; } = null;
    #endregion

    #region UNITY_CALLS
    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 distancePos = Target.position - new Vector3(0, distance.y, distance.x);
            transform.position = distancePos;
            transform.LookAt(Target.position);
        }
    }
    #endregion
}