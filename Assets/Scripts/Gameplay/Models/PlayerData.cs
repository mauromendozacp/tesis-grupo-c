public class PlayerData
{
    #region PRIVATE_FIELDS
    private int lives = 0;
    private int currentTurns = 0;
    private int totalTurns = 0;
    private GridIndex currentIndex = default;
    private GridIndex spawnIndex = default;
    #endregion

    #region PROPERTIES
    public int Lives { get => lives; set => lives = value; }
    public int CurrentTurns { get => currentTurns; set => currentTurns = value; }
    public int TotalTurns { get => totalTurns; set => totalTurns = value; }
    public GridIndex CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public GridIndex SpawnIndex { get => spawnIndex; set => spawnIndex = value; }
    #endregion
}