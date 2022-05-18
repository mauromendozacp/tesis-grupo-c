using UnityEngine;

public class Utils
{
    #region PUBLIC_METHODS

    public static bool CheckLayerInMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    #endregion
}
