using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEasyEvent
{
}

public class EasyEvent : IEasyEvent
{
    private Action mOnEvent = () => { };

    public IUnRegister Register(Action onEvent)
    {
        mOnEvent += onEvent;
        return new CustomUnRegister(() => { UnRegister(onEvent); });
    }

    public void UnRegister(Action onEvent)
    {
        mOnEvent -= onEvent;
    }

    public void Trigger()
    {
        mOnEvent?.Invoke();
    }
}

public class EasyEvent<T> : IEasyEvent
{
    private Action<T> mOnEvent = e => { };

    public IUnRegister Register(Action<T> onEvent)
    {
        mOnEvent += onEvent;
        return new CustomUnRegister(() => { UnRegister(onEvent); });
    }

    public void UnRegister(Action<T> onEvent)
    {
        mOnEvent -= onEvent;
    }

    public void Trigger(T t)
    {
        mOnEvent?.Invoke(t);
    }
}

public class EasyEvent<T, K> : IEasyEvent
{
    private Action<T, K> mOnEvent = (t, k) => { };

    public IUnRegister Register(Action<T, K> onEvent)
    {
        mOnEvent += onEvent;
        return new CustomUnRegister(() => { UnRegister(onEvent); });
    }

    public void UnRegister(Action<T, K> onEvent)
    {
        mOnEvent -= onEvent;
    }

    public void Trigger(T t, K k)
    {
        mOnEvent?.Invoke(t, k);
    }
}

public class EasyEvent<T, K, S> : IEasyEvent
{
    private Action<T, K, S> mOnEvent = (t, k, s) => { };

    public IUnRegister Register(Action<T, K, S> onEvent)
    {
        mOnEvent += onEvent;
        return new CustomUnRegister(() => { UnRegister(onEvent); });
    }

    public void UnRegister(Action<T, K, S> onEvent)
    {
        mOnEvent -= onEvent;
    }

    public void Trigger(T t, K k, S s)
    {
        mOnEvent?.Invoke(t, k, s);
    }
}

public class EasyEvents
{
    private static EasyEvents mGlobalEvents = new EasyEvents();

    public static T Get<T>() where T : IEasyEvent
    {
        return mGlobalEvents.GetEvent<T>();
    }


    public static void Register<T>() where T : IEasyEvent, new()
    {
        mGlobalEvents.AddEvent<T>();
    }

    private Dictionary<Type, IEasyEvent> mTypeEvents = new Dictionary<Type, IEasyEvent>();

    public void AddEvent<T>() where T : IEasyEvent, new()
    {
        mTypeEvents.Add(typeof(T), new T());
    }

    public T GetEvent<T>() where T : IEasyEvent
    {
        IEasyEvent e;

        if (mTypeEvents.TryGetValue(typeof(T), out e))
        {
            return (T)e;
        }

        return default;
    }

    public T GetOrAddEvent<T>() where T : IEasyEvent, new()
    {
        var eType = typeof(T);
        if (mTypeEvents.TryGetValue(eType, out var e))
        {
            return (T)e;
        }

        var t = new T();
        mTypeEvents.Add(eType, t);
        return t;
    }
}

public interface IUnRegister
{
    void UnRegister();
}

public interface IUnRegisterList
{
    List<IUnRegister> UnregisterList { get; }
}

public static class IUnRegisterListExtension
{
    public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
    {
        unRegisterList.UnregisterList.Add(self);
    }

    public static void UnRegisterAll(this IUnRegisterList self)
    {
        foreach (var unRegister in self.UnregisterList)
        {
            unRegister.UnRegister();
        }

        self.UnregisterList.Clear();
    }
}

/// <summary>
/// 自定义可注销的类
/// </summary>
public struct CustomUnRegister : IUnRegister
{
    /// <summary>
    /// 委托对象
    /// </summary>
    private Action mOnUnRegister { get; set; }

    /// <summary>
    /// 带参构造函数
    /// </summary>
    /// <param name="onDispose"></param>
    public CustomUnRegister(Action onUnRegsiter)
    {
        mOnUnRegister = onUnRegsiter;
    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public void UnRegister()
    {
        mOnUnRegister.Invoke();
        mOnUnRegister = null;
    }
}

public class UnRegisterOnDestroyTrigger : MonoBehaviour
{
    private readonly HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

    public void AddUnRegister(IUnRegister unRegister)
    {
        mUnRegisters.Add(unRegister);
    }

    public void RemoveUnRegister(IUnRegister unRegister)
    {
        mUnRegisters.Remove(unRegister);
    }

    private void OnDestroy()
    {
        foreach (var unRegister in mUnRegisters)
        {
            unRegister.UnRegister();
        }

        mUnRegisters.Clear();
    }
}

public static class UnRegisterExtension
{
    public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
    {
        var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

        if (!trigger)
        {
            trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
        }

        trigger.AddUnRegister(unRegister);

        return unRegister;
    }
}

public class TypeEventSystem
{
    private readonly EasyEvents mEvents = new EasyEvents();


    public static readonly TypeEventSystem Global = new TypeEventSystem();

    public void Send<T>() where T : new()
    {
        mEvents.GetEvent<EasyEvent<T>>()?.Trigger(new T());
    }

    public void Send<T>(T e)
    {
        mEvents.GetEvent<EasyEvent<T>>()?.Trigger(e);
    }

    public IUnRegister Register<T>(Action<T> onEvent)
    {
        var e = mEvents.GetOrAddEvent<EasyEvent<T>>();
        return e.Register(onEvent);
    }

    public void UnRegister<T>(Action<T> onEvent)
    {
        var e = mEvents.GetEvent<EasyEvent<T>>();
        if (e != null)
        {
            e.UnRegister(onEvent);
        }
    }
}