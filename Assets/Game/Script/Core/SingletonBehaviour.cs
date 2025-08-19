using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SingletonBehaviour<T> : SerializedMonoBehaviour where T : SingletonBehaviour<T>
{
    private static T instance;
    public static T Instance => instance;

    public static bool HasInstance => instance != null;

    private void Register()
    {
        if (instance != null)
        {
            Debug.LogError("More than one singleton object of type {0} exists." + (typeof (T).Name));
            Destroy(this);
            return;
        }

        instance = (T) this;
    }

    protected virtual void Awake() => Register();

    protected virtual void OnEnable()
    {
        if (instance == null) Register();
    }

    protected virtual void OnDestroy() => instance = null;
    //private void OnApplicationQuit() => instance = null;
}   

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance => instance;

    public static bool HasInstance => instance != null;

    private void Register()
    {
        if (instance != null)
        {
            Debug.LogError("More than one singleton object of type {0} exists." + (typeof (T).Name));
            Destroy(this);
            return;
        }

        instance = (T) this;
    }

    protected virtual void Awake() => Register();

    protected virtual void OnEnable()
    {
        if (instance == null) Register();
    }

    protected virtual void OnDestroy() => instance = null;
    //private void OnApplicationQuit() => instance = null;
} 