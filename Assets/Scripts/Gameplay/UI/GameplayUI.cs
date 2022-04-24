using System;
using UnityEngine;
using TMPro;

public class GUIActions
{
    public Action<int> onUpdateTurns = null;
}

public class GameplayUI : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private TextMeshProUGUI turnsText = null;
    #endregion

    #region PRIVATE_FIELDS
    private GUIActions guiActions = null;
    #endregion

    #region PROPERTIES
    public GUIActions GUIActions { get => guiActions; }
    #endregion

    #region PUBLIC_METHODS
    public void Init()
    {
        guiActions = new GUIActions();
        guiActions.onUpdateTurns += SetTurnsText;
    }
    #endregion

    #region PRIVATE_FIELDS
    private void SetTurnsText(int turns)
    {
        turnsText.text = "Turns: " + turns;
    }
    #endregion
}
