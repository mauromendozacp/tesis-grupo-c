using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("General Info")]
    [SerializeField] private Transform environmentHolder = null;
    [SerializeField] private float unit = 0f;

    [Header("Data"), Space]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private PrefabEntity[] prefabs = null;
    [SerializeField] private TextAsset levelJson = null;
    #endregion

    #region PRIVATE_FIELDS
    private LevelModel levelModel = null;
    private PlayerController playerController = null;
    private GridIndex winIndex = default;

    private GUIActions guiActions = null;
    #endregion

    #region PROPERTIES
    public PlayerController PlayerController { get => playerController; }
    #endregion

    #region PUBLIC_METHODS
    public void Init(GUIActions guiActions)
    {
        this.guiActions = guiActions;
    }

    public void StartGrid()
    {
        levelModel = JsonUtility.FromJson<LevelModel>(levelJson.text);
        SpawnGrid();
        SpawnPlayer();
    }
    #endregion

    #region PRIVATE_METHODS
    private void SpawnPlayer()
    {
        playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Init(guiActions, CheckIndexPlayer, unit);
        playerController.SetData(levelModel.PlayerModel);
        playerController.SetPositionUnit(new GridIndex(levelModel.PlayerModel.I, levelModel.PlayerModel.J));
    }

    private void SpawnGrid()
    {
        winIndex = new GridIndex(levelModel.WinI, levelModel.WinJ);
        for (int i = 0; i < levelModel.Layers.Length; i++)
        {
            GameObject layer = new GameObject("Layer " + (i + 1));
            layer.transform.parent = environmentHolder;
            int posY = levelModel.Layers[i].LayerIndex;

            for (int j = 0; j < levelModel.Layers[i].Models.Length; j++)
            {
                EntityModel entityModel = levelModel.Layers[i].Models[j];
                Vector3 pos = new Vector3(entityModel.I, posY, entityModel.J) * unit;

                if (entityModel.Id != "floor" && entityModel.Id != "chair_1")
                {
                    pos.y -= unit / 2;
                }

                GameObject go = Instantiate(GetPrefab(entityModel.Id), pos, Quaternion.identity, layer.transform);

                
                if (entityModel.Id == "chair_1")
                {
                    go.GetComponent<Box>().Init(unit);
                }
            }
        }
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
            //playerController.InputEnabled = false;
            playerController.Respawn();
            Debug.Log("Win");
            return;
        }

        //if (playerUnlimitedTurns) return;

        if (playerController.CheckTurns()) return;

        //playerController.InputEnabled = false; TODO Re enable when lose screen/popup is added + handle restart logic
        playerController.Respawn();
        Debug.Log("Lose");
    }
    #endregion
}