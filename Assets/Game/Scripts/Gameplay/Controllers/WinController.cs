using UnityEngine;

public class WinController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private ParticleSystem confettiSystem = null;
    #endregion

    #region PUBLIC_METHODS
    public void StartConfetti()
    {
        confettiSystem.Play();
    }
    #endregion
}
