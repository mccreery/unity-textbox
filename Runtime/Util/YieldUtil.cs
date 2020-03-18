using UnityEngine;

namespace McCreery.Textbox
{
    static class YieldUtil
    {
        public static object WaitForSecondsScaled(float time, bool scaled)
        {
            return scaled ? (object)new WaitForSeconds(time) : new WaitForSecondsRealtime(time);
        }
    }
}
