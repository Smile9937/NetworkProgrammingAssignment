using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                GameObject sigletonObject = new GameObject(typeof(T).Name);
                instance = sigletonObject.AddComponent<T>();
                Debug.Log("Creating Instance");
            }

            DontDestroyOnLoad(instance.gameObject);
        }
        else if(instance != FindObjectOfType<T>())
        {
            Destroy(FindObjectOfType<T>());
        }

        return instance;
    }
}
