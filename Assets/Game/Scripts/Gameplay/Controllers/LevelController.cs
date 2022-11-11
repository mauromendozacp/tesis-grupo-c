using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("General Info")]
    [SerializeField] private TransitionHandler transitionHandler = null;
    [SerializeField] private Transform environmentHolder = null;
    [SerializeField] private Transform levelPersistentHolder = null;
    [SerializeField] private float unit = 0f;

    [Header("Data")]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject winPrefab = null;
    [SerializeField] private PrefabEntity[] prefabs = null;
    [SerializeField] private LevelData[] levels = null;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioEvent music;
    [SerializeField] private AudioEvent testSfx;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerController playerController = null;

    private LevelData levelData = null;
    private int levelIndex = 0;

    private GridIndex winIndex = default;
    private List<GameObject> props = null;
    private DoorController door = null;
    #endregion

    #region ACTIONS
    private HUDActions hudActions = null;
    private PCActions pcActions = null;
    private GUIActions guiActions = null;
    private Action onSpawnWinConfetti = null;
    private Action onPlayerDeath = null;
    private Action onPlayerSpawned = null;
    #endregion

    #region PROPERTIES
    public PlayerController PlayerController { get => playerController; }
    #endregion

    #region PUBLIC_METHODS
    public void Init(HUDActions hudActions, GUIActions guiActions, Action onPlayerSpawned, Action onPlayerDeath, Action onCameraFollow)
    {
        this.hudActions = hudActions;
        this.guiActions = guiActions;
        this.onPlayerDeath = onPlayerDeath;
        this.onPlayerSpawned = onPlayerSpawned;

        pcActions = new PCActions
        {
            onChechIndexPlayer = CheckIndexPlayer,
            onCheckGridIndex = CheckIndex,
            onCameraFollow = onCameraFollow,
            onEndDeadAnimation = DeathPlayer
        };

        AudioHandler.Get().Setup(audioMixer, audioSource, audioSource);
        AudioHandler.Get().PlayAudio(music);
    }

    public void StartGrid()
    {
        if (levels.Length > 0)
        {
            levelData = levels[levelIndex];
            levelData.LoadLevel();
            
            transitionHandler.StartLevelTransition(fadeInFinished: () =>
            {
                for (int i = 0; i < environmentHolder.childCount; i++)
                {
                    Destroy(environmentHolder.GetChild(i).gameObject);
                }
                SpawnGrid();
                SpawnPlayer();
                onPlayerSpawned?.Invoke();
            }, fadeOutFinished: (() =>
            {
                PlayerInputStatus(true);
            }));
        }
    }

    public void RestartGame()
    {
        levelIndex = 0;
        StartGrid();
    }
    #endregion

    #region PRIVATE_METHODS
    private void SpawnPlayer()
    {
        if (FindObjectOfType<PlayerController>() == null) 
        {
            GameObject go = Instantiate(playerPrefab, levelPersistentHolder);
            playerController = go.GetComponent<PlayerController>();
            playerController.Init(hudActions, pcActions, unit);
            transitionHandler.Init(playerController);
        }
        
        playerController.SetData(levelData.Lives, levelData.Turns, levelData.LevelModel.PlayerModel.Rotation, levelData.LevelModel.PlayerModel.Index);
    }

    private void SpawnGrid()
    {
        LevelModel levelModel = levelData.LevelModel;

        SpawnWinZone();

        props = new List<GameObject>();
        winIndex = new GridIndex(levelModel.WinIndex.i, levelModel.WinIndex.j);

        for (int i = 0; i < levelModel.Layers.Length; i++)
        {
            GameObject layer = new GameObject("Layer " + (i + 1));
            layer.transform.parent = environmentHolder;
            int posY = levelModel.Layers[i].LayerIndex;

            for (int j = 0; j < levelModel.Layers[i].Models.Length; j++)
            {
                EntityModel entityModel = levelModel.Layers[i].Models[j];
                Vector3 pos = new Vector3(entityModel.Index.i, posY, entityModel.Index.j) * unit;

                GameObject prefab = GetPrefab(entityModel.Id);
                if (prefab == null) return;

                GameObject go = Instantiate(prefab, pos, Quaternion.identity, layer.transform);

                if (go.transform.childCount > 0)
                {
                    go.transform.GetChild(0).rotation = Quaternion.Euler(entityModel.Rotation.X, entityModel.Rotation.Y, entityModel.Rotation.Z);

                    if (entityModel.Offset.X != 0 || entityModel.Offset.Z != 0) 
                    {
                        go.transform.GetChild(0).position = new Vector3(entityModel.Offset.X, go.transform.GetChild(0).position.y, entityModel.Offset.Z);
                    }
                }

                switch (entityModel.Type)
                {
                    case ENTITY_TYPE.MOVABLE:
                        props.Add(go);
                        go.GetComponent<MovableController>().Init(CheckIndex, unit, pos);
                        break;
                    case ENTITY_TYPE.NO_MOVABLE:
                        break;
                    case ENTITY_TYPE.TRAP:
                        props.Add(go);
                        go.GetComponent<TrapController>().Init(onPlayerDeath, PlayerDeath, pos);
                        break;
                    case ENTITY_TYPE.JUMPABLE:
                        props.Add(go);
                        go.GetComponent<JumpableController>().Init(CheckIndex, pos);
                        break;
                    default:
                        break;
                }

                DoorController d = go.GetComponent<DoorController>();
                if (d != null && Utils.IsIndexAdjacent(winIndex, entityModel.Index))
                {
                    door = d;
                }
            }
        }
    }

    private void SpawnWinZone()
    {
        winIndex = new GridIndex(levelData.LevelModel.WinIndex.i, levelData.LevelModel.WinIndex.j);
        Vector3 pos = new Vector3(winIndex.i, 0, winIndex.j) * unit;

        GameObject go = Instantiate(winPrefab, pos, Quaternion.identity, environmentHolder);
        onSpawnWinConfetti = go.GetComponent<WinController>().StartConfetti;
    }

    private GameObject GetPrefab(string id)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].Id == id)
            {
                return prefabs[i].Prefab;
            }
        }

        return null;
    }

    private void CheckIndexPlayer(GridIndex index)
    {
        if (index == winIndex)
        {
            onSpawnWinConfetti?.Invoke();
            playerController.Win();
            door?.Open();
            
            NextLevel();
            Debug.Log("Win");

            return;
        }

        //if (playerUnlimitedTurns) return;

        if (playerController.CheckTurns()) return;

        playerController.PlayDeadAnimation();
        Debug.Log("Lose");
    }

    private bool CheckIndex(GridIndex index)
    {
        return index.i >= 0 && index.j >= 0 && index.i < levelData.LevelModel.LimitIndex.i && index.j < levelData.LevelModel.LimitIndex.j;
    }

    private void DeathPlayer(bool end)
    {
        if (end)
        {
            guiActions.onOpenGameoverPanel?.Invoke();
        }
        else
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].GetComponent<PropController>().Restart();
        }

        playerController.Respawn();
    }

    private void NextLevel()
    {
        StartCoroutine(NextLevelDelay());
    }

    private void PlayerDeath()
    {
        playerController.EndDeadAnimation();
    }

    private void PlayerInputStatus(bool status)
    {
        playerController.InputEnabled = status;
    }

    private IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(2f);

        levelIndex++;
        if (levelIndex >= levels.Length)
        {
            guiActions.onOpenWinPanel?.Invoke();
        }
        else
        {
            StartGrid();
        }
    }
    #endregion
}