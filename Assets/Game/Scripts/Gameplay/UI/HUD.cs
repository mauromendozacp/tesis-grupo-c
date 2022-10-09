using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDActions
{
    public Action onEnableLight = null;
    public Action onEnableUnlimitedTurns = null;
    public Action<int> onUpdateTurns = null;
}

public class HUD : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button lightBtn = null;
    [SerializeField] private Button turnsBtn = null;
    [SerializeField] private TextMeshProUGUI turnsText = null;
    #endregion

    #region PRIVATE_FIELDS
    private HUDActions hudActions = null;
    #endregion

    #region PROPERTIES
    public HUDActions HUDActions { get => hudActions; }
    #endregion

    #region PUBLIC_METHODS
    public void Init()
    {
        hudActions = new HUDActions();
        hudActions.onUpdateTurns += SetTurnsText;

        lightBtn.onClick.AddListener(() => { hudActions.onEnableLight?.Invoke(); });
        turnsBtn.onClick.AddListener(() => { hudActions.onEnableUnlimitedTurns?.Invoke(); });
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetTurnsText(int turns)
    {
        turnsText.text = turns.ToString();
    }
    #endregion
}