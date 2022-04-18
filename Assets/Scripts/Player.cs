using System;
using System.Collections;
using UnityEngine;

public enum MOVEMENT
{
    LEFT,
    UP,
    RIGHT,
    DOWN
}

public class Player : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private int lives = 0;
    [SerializeField] private float speed = 0f;
    #endregion

    #region PRIVATE_FIELDS
    private GridIndex gridIndex = default;
    private float unit = 0f;
    #endregion

    #region ACTIONS
    private Func<GridIndex, bool> onCheckGridIndex = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        Move();
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(Func<GridIndex, bool> onCheckGridIndex, float unit)
    {
        this.onCheckGridIndex = onCheckGridIndex;

        this.unit = unit;
    }
    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        if (!TryGetMovement(out MOVEMENT movement)) return;

        Vector3 pos = Vector3.zero;
        GridIndex auxIndex = gridIndex;

        switch (movement)
        {
            case MOVEMENT.LEFT:
                pos.x = -unit;
                auxIndex.i--;
                break;
            case MOVEMENT.UP:
                pos.y = unit;
                auxIndex.j++;
                break;
            case MOVEMENT.RIGHT:
                pos.x = unit;
                auxIndex.i++;
                break;
            case MOVEMENT.DOWN:
                pos.y = -unit;
                auxIndex.j--;
                break;
            default:
                break;
        }

        if (!onCheckGridIndex(gridIndex)) return;

        StartCoroutine(MoveLerp(transform.position + pos));
        gridIndex = auxIndex;
    }

    private bool TryGetMovement(out MOVEMENT movement)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            movement = MOVEMENT.LEFT;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            movement = MOVEMENT.UP;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            movement = MOVEMENT.RIGHT;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            movement = MOVEMENT.DOWN;
            return true;
        }

        movement = default;
        return false;
    }

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

        yield return null;
    }
    #endregion
}