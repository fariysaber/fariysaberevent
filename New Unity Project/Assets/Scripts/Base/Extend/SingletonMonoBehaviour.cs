using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviorExt where T : SingletonMonoBehaviour<T>
{
    private static T uniqueInstance;
    public static T Instance
    {
        get
        {
            return uniqueInstance;
        }
    }
    public static T GetInstance()
    {
        return Instance;
    }
    protected virtual void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = (T)this;
            Exist = true;
        }
        else if (uniqueInstance != this)
        {
            Debug.Log("dont have two behaviour");
        }
    }
    protected virtual void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            Exist = false;
            uniqueInstance = null;
        }
    }
    protected S AddComponent<S>() where S : Component
    {
        S component = GetComponent<S>();
        if (component == null)
        {
            component = gameObject.AddComponent<S>();
        }
        return component;
    }

    public static bool Exist
    {
        get;
        private set;
    }
}
