using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }

        DontDestroyOnLoad(gameObject);
    }
}

public class SingletonDestroy<T> : MonoBehaviour where T : SingletonDestroy<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        // Nếu đã có một instance khác và nó KHÔNG phải cùng GameObject, thì huỷ cái mới
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}