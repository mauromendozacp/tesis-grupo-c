using System;
using UnityEngine;
using System.Collections;

public class TrapController : PropController
{
    #region EXPOSED_FIELDS
    [SerializeField] private float activeTrapDelay = 0f;
    [SerializeField] private float fallPlayerDelay = 0f;
    [SerializeField] private float fallSpeed = 0f;
    [SerializeField] private GameObject model = null;
    [SerializeField] private LayerMask falleableMask = default;
    [SerializeField] private TrapData trapData = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool on = false;
    #endregion

    #region ACTIONS
    private Action onActive = null;
    private Action<TrapData> onRestart = null;
    #endregion

    #region UNITY_CALLS
    private void OnTriggerEnter(Collider other)
    {
        if (on) return;

        if (Utils.CheckLayerInMask(falleableMask, other.gameObject.layer))
        {
            on = true;
            ActiveTrap(other.gameObject);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(Action onActive, Action<TrapData> onRestart, Vector3 spawnPos, TrapData trapData)
    {
        this.onActive = onActive;
        this.onRestart = onRestart;
        this.trapData = trapData;
        
        SetSpawnIndex(spawnPos);
    }

    public void ActiveTrap(GameObject obj)
    {
        onActive?.Invoke();
        StartCoroutine(DisappearTrapDelay(() =>
        {
            PlayerController player = obj.GetComponent<PlayerController>();
            player?.FallAnimation();

            StartCoroutine(FallPlayerEndDelay(obj));
        }));
    }

    override public void Restart()
    {
        model.SetActive(true);
        on = false;

        base.Restart();
    }
    #endregion

    #region PRIVATE_METHODS
    private IEnumerator DisappearTrapDelay(Action onSuccess)
    {
        float timer = 0f;
        while (timer < activeTrapDelay)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        model.SetActive(false);
        onSuccess?.Invoke();

        yield return null;
    }

    private IEnumerator FallPlayerEndDelay(GameObject obj)
    {
        float timer = 0f;
        while (timer < fallPlayerDelay)
        {
            timer += Time.deltaTime;
            obj.transform.position = obj.transform.position + new Vector3(0, -fallSpeed, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        onRestart?.Invoke(trapData);

        yield return null;
    }
    #endregion
}
