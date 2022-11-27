//凭空创建一个GameObject，然后把组件对象挂上，场景切换不会销毁

using UnityEngine;

public class SingletonWithMono<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
}

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    /// <summary>
    /// 静态实例
    /// </summary>
    protected static T mInstance;

    /// <summary>
    /// 标签锁：确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
    /// 如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放
    /// </summary>
    static object mLock = new object();

    /// <summary>
    /// 静态属性
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (mLock)
            {
                if (mInstance == null)
                {
                    mInstance = new T();
                }
            }

            return mInstance;
        }
    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public virtual void Dispose()
    {
        mInstance = null;
    }

    /// <summary>
    /// 单例初始化方法
    /// </summary>
    public virtual void OnSingletonInit()
    {
    }
}