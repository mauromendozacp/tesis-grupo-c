using UnityEngine;

public class Utils
{
    #region PUBLIC_METHODS
    public static bool CheckLayerInMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool IsIndexAdjacent(GridIndex mainIndex, GridIndex checkIndex)
    {
        return checkIndex.i >= mainIndex.i - 1 && checkIndex.i <= checkIndex.i + 1 &&
            checkIndex.j >= mainIndex.j - 1 && checkIndex.j <= checkIndex.j + 1;
    }
    #endregion
}
