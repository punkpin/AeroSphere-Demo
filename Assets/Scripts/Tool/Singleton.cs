using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _inst = null;

    public static T instance
    {
        get
        {
            return _inst;
        }
    }

    protected virtual bool isGlobal => false;

    protected virtual void Awake()
    {
        if (_inst == null)
        {
            _inst = (T)this;

            if (isGlobal)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
