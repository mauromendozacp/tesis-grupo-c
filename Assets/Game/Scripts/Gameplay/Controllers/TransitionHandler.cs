using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Animation cutOffAnimation = null;
    [SerializeField] private Image backgroundImage = null;
    #endregion 

    #region PRIVATE_FIELDS
    private PlayerController playerController = null;
    private const string fadeInAnimation = "fadeIn";
    private const string fadeOutAnimation = "fadeOut";
    #endregion

    #region PUBLIC_METHODS
    public void Init(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void StartLevelTransition(Action fadeInFinished, Action fadeOutFinished)
    {
        cutOffAnimation.gameObject.SetActive(true);
        StartCoroutine(FadeImage(true, backgroundImage));
        StartCoroutine(PlayAnimation(fadeInAnimation, animationFinished: () =>
        {
            fadeInFinished?.Invoke();
            StartCoroutine(FadeImage(false, backgroundImage));
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
        cutOffAnimation.Play(clipName);

        yield return new WaitForSeconds(lenght);

        animationFinished?.Invoke();
    }
    
    IEnumerator FadeImage(bool fadeIn, Image img)
    {
        if (!fadeIn)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
          
        }
        else
        {
            for (float i = 0; i <= 3; i += Time.deltaTime)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
          
        }
    }
    #endregion
}