using UnityEngine;

[System.Serializable]
public class LevelModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private int winI = 0;
    [SerializeField] private int winJ = 0;
    [SerializeField] private int limitI = 0;
    [SerializeField] private int limitJ = 0;
    [SerializeField] private PlayerModel playerModel = null;
    [SerializeField] private LayerModel[] layers = null;
    #endregion

    #region PROPERTIES
    public PlayerModel PlayerModel { get => playerModel; }
    public LayerModel[] Layers { get => layers; }
    public int WinI { get => winI; }
    public int WinJ { get => winJ; }
    public int LimitI { get => limitI; }
    public int LimitJ { get => limitJ; }
    #endregion
}
