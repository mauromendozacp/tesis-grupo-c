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
    public PlayerModel PlayerModel { get => playerModel; set => playerModel = value; }
    public LayerModel[] Layers { get => layers; set => layers = value; }
    public int WinI { get => winI; set => winI = value; }
    public int WinJ { get => winJ; set => winJ = value; }
    public int LimitI { get => limitI; set => limitI = value; }
    public int LimitJ { get => limitJ; set => limitJ = value; }
    #endregion
}
