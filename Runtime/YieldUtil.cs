using UnityEngine;

public class YieldUtil
{
    public static object WaitForSecondsScaled(float time, bool scaled)
    {
        return scaled ? (object)new WaitForSeconds(time) : new WaitForSecondsRealtime(time);
    }
}
