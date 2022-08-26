using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    #region EXPOSED_FIELDS
    [SerializeField] private int lives = 0;
    [SerializeField] private int turns = 0;
    [SerializeField] private TextAsset jsonLevel = null;
    #endregion

    #region PRIVATE_FIELDS
    private LevelModel levelModel = null;
    #endregion

    #region PROPERTIES
    public int Lives { get => lives; }
    public int Turns { get => turns; }
    public TextAsset JsonLevel { get => jsonLevel; }
    public LevelModel LevelModel { get => levelModel; }
    #endregion

    #region PUBLIC_METHODS
    public void LoadLevel()
    {
        levelModel = JsonUtility.FromJson<LevelModel>(jsonLevel.text);
    }
    #endregion
}
