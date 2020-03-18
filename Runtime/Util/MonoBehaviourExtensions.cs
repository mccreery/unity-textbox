using UnityEngine;

namespace McCreery.Textbox
{
    static class MonoBehaviourExtensions
    {
        public static T LazyGet<T>(this MonoBehaviour monoBehaviour, ref T inspectorField, bool create = false) where T : Component
        {
            if (inspectorField == null)
            {
                inspectorField = monoBehaviour.GetComponent<T>();

                if (create && inspectorField == null)
                {
                    inspectorField = monoBehaviour.gameObject.AddComponent<T>();
                }
            }
            return inspectorField;
        }
    }
}
