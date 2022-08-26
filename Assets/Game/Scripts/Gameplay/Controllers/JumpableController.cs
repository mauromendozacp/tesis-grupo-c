using System;
using UnityEngine;

public class JumpableController : PropController, IJumpable
{
    #region PRIVATE_FIELDS
    private GridIndex gridIndex = default;
    #endregion

    #region ACTIONS
    private Func<GridIndex, bool> onCheckGridIndex = null;
    #endregion

    #region PUBLIC_METHODS
    public void Init(Func<GridIndex, bool> onCheckGridIndex, Vector3 spawnPos)
    {
        this.onCheckGridIndex = onCheckGridIndex;

        gridIndex = new GridIndex()
        {
            i = (int)transform.position.x,
            j = (int)transform.position.z
        };

        SetSpawnIndex(spawnPos);
    }

    public bool TryJump(MOVEMENT movement)
    {
        Vector3 direction = Vector3.zero;
        GridIndex auxIndex = gridIndex;

        switch (movement)
        {
            case MOVEMENT.LEFT:
                auxIndex.i--;
                direction = Vector3.left;
                break;
            case MOVEMENT.UP:
                auxIndex.j++;
                direction = Vector3.forward;
                break;
            case MOVEMENT.RIGHT:
                auxIndex.i++;
                direction = Vector3.right;
                break;
            case MOVEMENT.DOWN:
                auxIndex.j--;
                direction = Vector3.back;
                break;
            case MOVEMENT.NONE:
            default:
                break;
        }

        if (!onCheckGridIndex(auxIndex)) return false;

        return !Physics.Raycast(transform.position, direction, out var hit, 1);
    }

    public bool TryJump(Vector3 movement)
    {
        GridIndex auxIndex = gridIndex;

        if (movement == Vector3.left)
        {
            auxIndex.i--;
        }
        else if (movement == Vector3.right)
        {
            auxIndex.i++;
        }
        else if (movement == Vector3.forward)
        {
            auxIndex.j++;
        }
        else if (movement == Vector3.back)
        {
            auxIndex.j--;
        }
        else
        {
            return false;
        }

        if (!onCheckGridIndex(auxIndex)) return false;
        return !Physics.Raycast(transform.position, movement, out var hit, 1);
    }

    public override void Restart()
    {
        base.Restart();
    }
    #endregion
}