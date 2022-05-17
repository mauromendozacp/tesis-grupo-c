using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour, IMovable
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 0f;
    #endregion

    #region PRIVATE_FIELDS
    private GridIndex gridIndex = default;
    private float unit = 0f;
    private bool inMovement = false;
    #endregion

    #region PUBLIC_METHODS
    public void Init(float unit)
    {
        this.unit = unit;

        gridIndex = new GridIndex()
        {
            i = (int)transform.position.x,
            j = (int)transform.position.z
        };
    }
    public bool TryMove(MOVEMENT movement)
    {
        if (inMovement) return false;

        Vector3 pos = Vector3.zero;
        GridIndex auxIndex = gridIndex;

        switch (movement)
        {
            case MOVEMENT.LEFT:
                pos.x = -unit;
                auxIndex.i--;
                break;
            case MOVEMENT.UP:
                pos.z = unit;
                auxIndex.j++;
                break;
            case MOVEMENT.RIGHT:
                pos.x = unit;
                auxIndex.i++;
                break;
            case MOVEMENT.DOWN:
                pos.z = -unit;
                auxIndex.j--;
                break;
            default:
                break;
        }

        inMovement = true;
        StartCoroutine(MoveLerp(transform.position + pos));
        gridIndex = auxIndex;
        return true;
    }
    #endregion

    #region PRIVATE_METHODS
    private IEnumerator MoveLerp(Vector3 pos)
    {
        float timer = 0f;
        Vector3 initialPos = transform.position;

        while (timer < speed)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, pos, timer / speed);

            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
        inMovement = false;

        yield return null;
    }
    #endregion
}