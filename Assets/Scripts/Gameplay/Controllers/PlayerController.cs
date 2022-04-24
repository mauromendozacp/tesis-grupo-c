using System;
using System.Collections;
using UnityEngine;

public enum MOVEMENT
{
    NONE,
    LEFT,
    UP,
    RIGHT,
    DOWN
}

public class PlayerController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 0f;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerModel model = null;
    private float unit = 0f;
    private bool inMovement = false;
    private bool inputEnabled = true;
    #endregion

    #region PROPERTIES
    public bool InputEnabled { get => inputEnabled; set => inputEnabled = value; }
    public PlayerModel Model { get => model; }
    #endregion

    #region ACTIONS
    private Func<GridIndex, bool> onCheckGridIndex = null;
    private Action<GridIndex> onChechIndexPlayer = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if (!inputEnabled) return;

        Move();
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(Func<GridIndex, bool> onCheckGridIndex, Action<GridIndex> onChechIndexPlayer, float unit)
    {
        this.onChechIndexPlayer = onChechIndexPlayer;
        this.onCheckGridIndex = onCheckGridIndex;

        this.unit = unit;
    }

    public void SetData(PlayerModel model)
    {
        this.model = model;
    }

    public void SetPositionUnit(GridIndex index)
    {
        Vector3 pos = transform.position;
        pos.x = index.i * unit;
        pos.z = index.j * unit;
        transform.position = pos;
    }
    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        if (inMovement) return;

        MOVEMENT movement = TryGetMovement();
        if (movement == MOVEMENT.NONE) return;

        if (model.Turns <= 0)
        {
            Debug.Log("Sin turnos!");
            return;
        }

        Vector3 pos = Vector3.zero;
        Vector3 direction = Vector3.zero;
        GridIndex auxIndex = model.Index;
        RaycastHit hit;

        switch (movement)
        {
            case MOVEMENT.LEFT:
                pos.x = -unit;
                auxIndex.i--;
                direction = Vector3.left;
                break;
            case MOVEMENT.UP:
                pos.z = unit;
                auxIndex.j++;
                direction = Vector3.forward;
                break;
            case MOVEMENT.RIGHT:
                pos.x = unit;
                auxIndex.i++;
                direction = Vector3.right;
                break;
            case MOVEMENT.DOWN:
                pos.z = -unit;
                auxIndex.j--;
                direction = Vector3.back;
                break;
            default:
                break;
        }

        if (!onCheckGridIndex(auxIndex)) return;

        if (Physics.Raycast(transform.position, direction, out hit, 1))
        {
            IMovable movable = hit.transform.GetComponent<IMovable>();
            movable?.TryMove(movement);
        }

        inMovement = true;
        model.Index = auxIndex;
        model.Turns--;
        StartCoroutine(MoveLerp(transform.position + pos));
    }

    private MOVEMENT TryGetMovement()
    {
        MOVEMENT movement = default;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            movement = MOVEMENT.LEFT;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            movement = MOVEMENT.UP;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            movement = MOVEMENT.RIGHT;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            movement = MOVEMENT.DOWN;
        }

        return movement;
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
        inMovement = false;
        onChechIndexPlayer?.Invoke(model.Index);

        yield return null;
    }
    #endregion
}