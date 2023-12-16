using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; protected set; }
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this as T)
        {
            Singleton_Despawn();
        }
        else
        {
            Singleton_SetInstance();
        }
    }

    protected void Singleton_Despawn()
    {
        Destroy(gameObject);
    }
    protected void Singleton_SetInstance()
    {
        Instance = this as T;
    }
}