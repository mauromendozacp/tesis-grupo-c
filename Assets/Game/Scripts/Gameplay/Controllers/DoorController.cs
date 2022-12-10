using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Animator animator = null;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource = null;
    [SerializeField] private AudioEvent audioOpen = null;
    #endregion

    #region CONSTANTS_FIELDS
    private const string openKey = "open";
    #endregion

    #region PUBLIC_METHODS
    public void Open()
    {
        animator.SetTrigger(openKey);
        AudioHandler.Get().PlaySound(audioOpen, sfxSource);
    }
    #endregion
}
