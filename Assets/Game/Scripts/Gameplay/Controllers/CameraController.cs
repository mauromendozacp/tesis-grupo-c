using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Vector2 distance = Vector2.zero;
    #endregion

    #region PROPERTIES
    public Transform Target { get; set; } = null;
    public bool Follow { get; set; } = true;
    #endregion

    #region UNITY_CALLS
    private void LateUpdate()
    {
        if (Target != null && Follow)
        {
            Vector3 distancePos = Target.position - new Vector3(0, distance.y, distance.x);
            transform.position = distancePos;
            transform.LookAt(Target.position);
        }
    }
    #endregion
}