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
    [SerializeField] private Light focusLight = null;
    [SerializeField] private LayerMask noMovableMask = default;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerData data = null;
    private float unit = 0f;
    private bool inMovement = false;
    private bool inputEnabled = true;

    //DEBUG
    private bool unlimitedTurns = false;
    #endregion

    #region PROPERTIES
    public bool InputEnabled { get => inputEnabled; set => inputEnabled = value; }
    #endregion

    #region ACTIONS
    private GUIActions guiActions = null;
    private Action<GridIndex> onChechIndexPlayer = null;
    private Func<GridIndex, bool> onCheckGridIndex = null;
    private Action onCameraFollow = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if (!inputEnabled) return;

        Move();
        Jump();
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(GUIActions guiActions, Func<GridIndex, bool> onCheckGridIndex, Action<GridIndex> onChechIndexPlayer, Action onCameraFollow, float unit)
    {
        this.guiActions = guiActions;
        this.onCheckGridIndex = onCheckGridIndex;
        this.onChechIndexPlayer = onChechIndexPlayer;
        this.onCameraFollow = onCameraFollow;
        this.unit = unit;

        data = new PlayerData();
    }

    public void SetData(PlayerModel model)
    {
        data.Lives = model.Lives;
        data.TotalTurns = model.Turns;
        SetTurns(model.Turns);
        data.SpawnIndex = new GridIndex(model.I, model.J);
        data.CurrentIndex = data.SpawnIndex;
    }

    public void SetPositionUnit(GridIndex index)
    {
        data.SpawnIndex = index;
        data.CurrentIndex = index;
        Vector3 pos = transform.position;
        pos.x = index.i * unit;
        pos.y = 0;
        pos.z = index.j * unit;
        transform.position = pos;
    }

    public bool CheckTurns()
    {
        return data.CurrentTurns > 0;
    }

    public void Respawn()
    {
        transform.forward = Vector3.forward;
        SetPositionUnit(data.SpawnIndex);
        SetTurns(data.TotalTurns);
        inputEnabled = true;
        onCameraFollow?.Invoke();
    }

    public void TurnLight()
    {
        focusLight.enabled = !focusLight.enabled;
    }

    public void EnableUnlimitedTurns()
    {
        unlimitedTurns = !unlimitedTurns;
    }
    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        if (inMovement) return;

        MOVEMENT movement = TryGetMovement();
        if (movement == MOVEMENT.NONE) return;

        Vector3 pos = Vector3.zero;
        Vector3 direction = Vector3.zero;
        GridIndex auxIndex = data.CurrentIndex;
        bool validMove = false;

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

        transform.forward = direction;
        if (!onCheckGridIndex(auxIndex)) return;

        if (Physics.Raycast(transform.position, direction, out var hit, 1))
        {
            if (Utils.CheckLayerInMask(noMovableMask, hit.transform.gameObject.layer)) return;

            IMovable movable = hit.transform.GetComponent<IMovable>();

            if (movable != null)
            {
                if (movable.TryMove(movement))
                    validMove = true;
            }


            if (!validMove)
            {
                return;
            }
        }

        inMovement = true;
        data.CurrentIndex = auxIndex;

        if (!unlimitedTurns)
        {
            SetTurns(data.CurrentTurns - 1);
        }

        StartCoroutine(MoveLerp(transform.position + pos));
    }

    private void Jump()
    {
        if (inMovement) return;

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        Vector3 direction = transform.forward;
        Vector3 pos = Vector3.zero;
        GridIndex auxIndex = data.CurrentIndex;

        if (Physics.Raycast(transform.position, direction, out var hit, 1))
        {
            if (Utils.CheckLayerInMask(noMovableMask, hit.transform.gameObject.layer)) return;

            IJumpable jumpable = hit.transform.GetComponent<IJumpable>();

            if (jumpable == null) return;
            if (!jumpable.TryJump(direction)) return;

            if (direction == Vector3.left)
            {
                auxIndex.i -= 2;
                pos.x = -unit * 2;
            }
            else if (direction == Vector3.forward)
            {
                auxIndex.j += 2;
                pos.z = unit * 2;
            }
            else if (direction == Vector3.right)
            {
                auxIndex.i += 2;
                pos.x = unit * 2;
            }
            else if (direction == Vector3.back)
            {
                auxIndex.j -= 2;
                pos.z = -unit * 2;
            }
            else
            {
                return;
            }

            inMovement = true;
            data.CurrentIndex = auxIndex;

            if (!unlimitedTurns)
            {
                SetTurns(data.CurrentTurns - 1);
            }

            StartCoroutine(MoveLerp(transform.position + pos));
        }
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
        onChechIndexPlayer?.Invoke(data.CurrentIndex);

        yield return null;
    }

    private void SetTurns(int turns)
    {
        data.CurrentTurns = turns;
        guiActions.onUpdateTurns?.Invoke(turns);
    }
    #endregion
}