using UnityEngine;

[System.Serializable]
public class LayerModel
{
    #region EXPOSED_FIELDS
    [SerializeField] private int layerIndex = 0;
    [SerializeField] private EntityModel[] models = null;
    #endregion

    #region PROPERTIES
    public int LayerIndex { get => layerIndex; set => layerIndex = value; }
    public EntityModel[] Models { get => models; set => models = value; }
    #endregion
}
