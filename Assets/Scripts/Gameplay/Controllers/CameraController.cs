using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float distance = 0f;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        Vector3 distancePos = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 2f, Vector3.up) * transform.position;
        transform.position = distancePos;
        transform.LookAt(Vector3.zero);
    }
    #endregion
}
