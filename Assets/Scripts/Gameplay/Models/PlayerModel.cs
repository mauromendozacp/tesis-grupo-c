using System;

[Serializable]
public class PlayerModel
{
    #region PRIVATE_FIELDS
    private int lives = 0;
    private int turns = 0;
    private GridIndex index = default;
    #endregion

    #region PROPERTIES
    public int Lives { get => lives; set => lives = value; }
    public int Turns { get => turns; set => turns = value; }
    public GridIndex Index { get => index; set => index = value; }
    #endregion
}