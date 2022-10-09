using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Animation cutOffAnimation = null;
    #endregion

    #region PRIVATE_FIELDS
    private PlayerController playerController = null;
    private const string fadeInAnimation = "cutOffIn";
    private const string fadeOutAnimation = "cutOffOut";
    #endregion

    #region PUBLIC_METHODS
    public void Init(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void StartLevelTransition(Action fadeInFinished, Action fadeOutFinished)
    {
        cutOffAnimation.gameObject.SetActive(true);
        StartCoroutine(PlayAnimation(fadeInAnimation, animationFinished: () =>
        {
            fadeInFinished?.Invoke();
            StartCoroutine(PlayAnimation(fadeOutAnimation, animationFinished: () =>
            {
                fadeOutFinished?.Invoke();
                cutOffAnimation.gameObject.SetActive(false);
            }));
        }));
    }
    #endregion

    #region PRIVATE_METHODS
    IEnumerator PlayAnimation(string clipName, Action animationFinished)
    {
        float lenght = cutOffAnimation.GetClip(clipName).length;
        cutOffAnimation.Play(fadeInAnimation);

        yield return new WaitForSeconds(lenght);

        animationFinished?.Invoke();
    }
    #endregion
}