using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIActions
{
    public Action onExit = null;
    public Action onEnableLight = null;
    public Action onEnableUnlimitedTurns = null;
    public Action<int> onUpdateTurns = null;
}

public class UIGameplay : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button exitBtn = null;
    [SerializeField] private Button lightBtn = null;
    [SerializeField] private Button turnsBtn = null;
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

        exitBtn.onClick.AddListener(() => { guiActions.onExit?.Invoke(); });
        lightBtn.onClick.AddListener(() => { guiActions.onEnableLight?.Invoke(); });
        turnsBtn.onClick.AddListener(() => { guiActions.onEnableUnlimitedTurns?.Invoke(); });
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetTurnsText(int turns)
    {
        turnsText.text = "Turns: " + turns;
    }
    #endregion
}