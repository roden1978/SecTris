using UnityEngine;

namespace InputSwipe
{
    public class Singleton<T> : MonoBehaviour
        where T: Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs != null && objs.Length > 0)
                        _instance = objs[0];
                    if (objs != null && objs.Length > 1)
                        Debug.LogError($"There is more that one {typeof(T).Name} in the scene.");
                    if (_instance == null)
                    {
                        var obj = new GameObject {hideFlags = HideFlags.HideAndDontSave};
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }
}
