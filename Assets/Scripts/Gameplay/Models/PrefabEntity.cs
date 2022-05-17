using UnityEngine;

[CreateAssetMenu(fileName = "Entity_", menuName = "Entity/EntityObject", order = 1)]
public class PrefabEntity : ScriptableObject
{
    #region EXPOSED_FIELDS
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject prefab = null;
    #endregion

    #region PROPERTIES
    public string Id { get => id; }
    public GameObject Prefab { get => prefab; }
    #endregion
}