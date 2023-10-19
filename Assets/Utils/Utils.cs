using UnityEngine;

namespace SnakeApple.Space
{
    public static class Utils
    {
        public static bool CheckLayer(LayerMask layerMask, int layer)
        {
            if ((layerMask.value & (1 << layer)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}