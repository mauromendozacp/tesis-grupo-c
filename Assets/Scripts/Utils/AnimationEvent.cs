using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private UnityEvent event1 = null;
    [SerializeField] private UnityEvent event2 = null;

    #endregion

    #region PUBLIC_METHODS

    public void CallEvent1()
    {
        event1?.Invoke();
    }

    public void CallEvent2()
    {
        event2?.Invoke();
    }

    #endregion
}
