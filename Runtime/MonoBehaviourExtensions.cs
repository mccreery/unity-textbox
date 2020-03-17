using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void GetComponentIfNull<T>(this MonoBehaviour monoBehaviour, ref T inspectorField, bool create = false) where T : Component
    {
        if (inspectorField == null)
        {
            inspectorField = monoBehaviour.GetComponent<T>();

            if (create && inspectorField == null)
            {
                inspectorField = monoBehaviour.gameObject.AddComponent<T>();
            }
        }
    }
}
