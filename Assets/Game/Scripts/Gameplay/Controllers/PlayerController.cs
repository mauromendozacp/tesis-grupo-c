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

public class PCActions
{
    public Action<GridIndex> onChechIndexPlayer = null;
    public Func<GridIndex, bool> onCheckGridIndex = null;
    public Action onCameraFollow = null;
    public Action<bool> onEndDeadAnimation = null;
}

public class PlayerController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 0f;
    [SerializeField] private Light focusLight = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private LayerMask noMovementMask = default;
    [SerializeField] private LayerMask noJumpeableMask = default;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerData data = null;
    private float unit = 0f;
    private bool inMovement = false;
    private bool inputEnabled = true;
    private bool movementPressed = false;
    private bool death = false;

    //DEBUG
    private bool unlimitedTurns = false;
    #endregion

    #region CONSTANT_FIELDS
    private const string speedKey = "speed";
    private const string deadKey = "dead";
    #endregion

    #region PROPERTIES
    public bool InputEnabled { get => inputEnabled; set => inputEnabled = value; }
    #endregion

    #region ACTIONS
    private HUDActions guiActions = null;
    private PCActions pcActions = null;
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
    public void Init(HUDActions guiActions, PCActions pcActions, float unit)
    {
        this.guiActions = guiActions;
        this.pcActions = pcActions;
        this.unit = unit;

        data = new PlayerData();
    }

    public void SetData(int lives, int turns, RotationModel rotation, GridIndex spawnIndex)
    {
        data.Lives = lives;
        data.TotalTurns = turns;
        data.SpawnIndex = spawnIndex;

        SetTurns(turns);
        SetPositionUnit(spawnIndex);
        SetRotation(rotation);
    }

    public bool CheckTurns()
    {
        return data.CurrentTurns > 0;
    }

    public void Respawn()
    {
        inputEnabled = true;
        death = false;

        transform.forward = Vector3.forward;
        animator.SetBool(deadKey, false);
        animator.SetFloat(speedKey, 0f);

        SetPositionUnit(data.SpawnIndex);
        SetTurns(data.TotalTurns);

        pcActions.onCameraFollow?.Invoke();
    }

    public void TurnLight()
    {
        focusLight.enabled = !focusLight.enabled;
    }

    public void EnableUnlimitedTurns()
    {
        unlimitedTurns = !unlimitedTurns;
    }

    public void PlayerDeath()
    {
        inputEnabled = false;
        death = true;
    }

    public void PlayDeadAnimation()
    {
        inputEnabled = false;
        //animator.SetBool(deadKey, true);
        EndDeadAnimation();
    }

    public void EndDeadAnimation()
    {
        data.Lives--;
        pcActions.onEndDeadAnimation?.Invoke(data.Lives <= 0);
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetPositionUnit(GridIndex index)
    {
        data.CurrentIndex = index;
        Vector3 pos = transform.position;
        pos.x = index.i * unit;
        pos.y = 0;
        pos.z = index.j * unit;
        transform.position = pos;
    }
    
    private void Move()
    {
        MOVEMENT movement = TryGetMovement();

        if (inMovement) return;

        if (movement == MOVEMENT.NONE) return;

        Vector3 pos = Vector3.zero;
        Vector3 direction = Vector3.zero;
        GridIndex auxIndex = data.CurrentIndex;

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
        if (!pcActions.onCheckGridIndex(auxIndex)) return;

        if (Physics.Raycast(transform.position, direction, out var hit, 1))
        {
            bool validMove = !Utils.CheckLayerInMask(noMovementMask, hit.transform.gameObject.layer);

            IMovable movable = hit.transform.GetComponent<IMovable>();

            if (movable != null)
            {
                validMove = movable.TryMove(movement);
            }

            if (!validMove)
            {
                animator.SetFloat(speedKey, 0f);
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
            if (Utils.CheckLayerInMask(noJumpeableMask, hit.transform.gameObject.layer)) return;

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

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movement = MOVEMENT.LEFT;
            movementPressed = true;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movement = MOVEMENT.UP;
            movementPressed = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movement = MOVEMENT.RIGHT;
            movementPressed = true;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movement = MOVEMENT.DOWN;
            movementPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            movementPressed = false;
        }

        return movement;
    }

    private IEnumerator MoveLerp(Vector3 pos)
    {
        float timer = 0f;
        float speedLerp = 0f;
        Vector3 initialPos = transform.position;

        while (timer < speed / 2)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, pos, timer / speed);

            speedLerp = timer / (speed / 2);
            animator.SetFloat(speedKey, movementPressed ? 1f : speedLerp);

            yield return new WaitForEndOfFrame();
        }
        animator.SetFloat(speedKey, 1f);

        while (timer < speed)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, pos, timer / speed);

            speedLerp = 2f - ((timer * 2f) / speed);
            animator.SetFloat(speedKey, movementPressed ? 1f : speedLerp);

            yield return new WaitForEndOfFrame();
        }
        animator.SetFloat(speedKey, movementPressed && !death ? 1f : 0f);

        transform.position = pos;
        inMovement = false;
        pcActions.onChechIndexPlayer?.Invoke(data.CurrentIndex);

        yield return null;
    }

    private void SetTurns(int turns)
    {
        data.CurrentTurns = turns;
        guiActions.onUpdateTurns?.Invoke(turns);
    }

    private void SetRotation(RotationModel rotation)
    {
        transform.eulerAngles = new Vector3(rotation.X, rotation.Y, rotation.Z);
    }
    #endregion
}