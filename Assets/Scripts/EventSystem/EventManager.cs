using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static Dictionary<EventType, Delegate> eventDic = new Dictionary<EventType, Delegate>();

    private static readonly object eventLock = new object();

    // 订阅事件
    public static void Subscribe<T> (EventType eventType, Action<T> listener)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            eventDic[eventType] = existingDelegate as Action<T> + listener;
        }
        else
        {
            eventDic.Add(eventType, listener);
        }
    }

    public static void Subscribe<T, TResult>(EventType eventType, Func<T, TResult> listener)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            eventDic[eventType] = existingDelegate as Func<T, TResult> + listener;
        }
        else
        {
            eventDic.Add(eventType, listener);
        }
    }

    public static void Subscribe<TResult>(EventType eventType, Func<TResult> listener)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            eventDic[eventType] = existingDelegate as Func<TResult> + listener;
        }
        else
        {
            eventDic.Add(eventType, listener);
        }
    }

    public static void Subscribe(EventType eventType, Action listener)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            eventDic[eventType] = existingDelegate as Action + listener;
        }
        else
        {
            eventDic.Add(eventType, listener);
        }
    }

    // 取消订阅事件
    public static void Unsubscribe<T> (EventType eventType, Action<T> listener)
    {
        lock (eventLock)
        {
            if (!eventDic.TryGetValue(eventType,out var existingDelegate))
            {
                var newDelegate = existingDelegate as Action<T> - listener;
                if (newDelegate == null)
                {
                    eventDic.Remove(eventType);
                }
                else
                {
                    eventDic[eventType] = newDelegate;
                }
            }
        }
    }

    public static void Unsubscribe<T, TResult>(EventType eventType, Func<T, TResult> listener)
    {
        lock (eventLock)
        {
            if (!eventDic.TryGetValue(eventType, out var existingDelegate))
            {
                var newDelegate = existingDelegate as Func < T, TResult > - listener;
                if (newDelegate == null)
                {
                    eventDic.Remove(eventType);
                }
                else
                {
                    eventDic[eventType] = newDelegate;
                }
            }
        }
    }

    public static void Unsubscribe<TResult>(EventType eventType, Func<TResult> listener)
    {
        lock (eventLock)
        {
            if (!eventDic.TryGetValue(eventType, out var existingDelegate))
            {
                var newDelegate = existingDelegate as Func<TResult> - listener;
                if (newDelegate == null)
                {
                    eventDic.Remove(eventType);
                }
                else
                {
                    eventDic[eventType] = newDelegate;
                }
            }
        }
    }

    public static void Unsubscribe(EventType eventType, Action listener)
    {
        lock (eventLock)
        {
            if (!eventDic.TryGetValue(eventType, out var existingDelegate))
            {
                var newDelegate = existingDelegate as Action - listener;
                if (newDelegate == null)
                {
                    eventDic.Remove(eventType);
                }
                else
                {
                    eventDic[eventType] = newDelegate;
                }
            }
        }
    }

    // 触发事件
    public static void Trigger<T> (EventType eventType, T eventData)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            (existingDelegate as Action<T>)?.Invoke(eventData);
        }
    }

    public static TResult Trigger<T, TResult>(EventType eventType, T eventData)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            var funcList = existingDelegate as Func<T, TResult>;
            if (funcList != null)
            {
                return funcList(eventData);
            }
        }
        return default(TResult);
    }

    public static TResult Trigger<TResult>(EventType eventType)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            var funcList = existingDelegate as Func<TResult>;
            if (funcList != null)
            {
                return funcList();
            }
        }
        return default(TResult);
    }

    public static void Trigger(EventType eventType)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            (existingDelegate as Action)?.Invoke();
        }
    }

    public static void Clear(EventType eventType)
    {
        if (eventDic.TryGetValue(eventType, out var existingDelegate))
        {
            existingDelegate = null;
        }
    }
}
