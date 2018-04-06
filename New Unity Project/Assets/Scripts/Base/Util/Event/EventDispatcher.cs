using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// 事件处理类
    /// </summary>
public class EventController
{
    private Dictionary<string, Delegate> m_theRouter = new Dictionary<string, Delegate>();
    public Dictionary<string, Delegate> TheRouter
    {
        get
        {
            return m_theRouter;
        }
    }
    /// <summary>
    /// 永久注册事件列表
    /// </summary>
    private List<string> m_permanentEvents = new List<string>();

    /// <summary>
    /// 标记为永久注册事件
    /// </summary>
    /// <param name = "eventType"></param>
    public void MarkAsPermanent(string eventType)
    {
        m_permanentEvents.Add(eventType);
    }

    /// <summary>
    /// 清除非永久注册事件
    /// </summary>
    public void Cleanup()
    {
        List<string> eventToRemove = new List<string>();
        foreach (var key in m_theRouter.Keys)
        {
            bool wasFound = false;
            for (int j = 0; j < m_permanentEvents.Count; j++)
            {
                if (key == m_permanentEvents[j])
                {
                    wasFound = true;
                    break;
                }
            }
            if (!wasFound)
            {
                eventToRemove.Add(key);
            }
        }
        for (int i = 0; i < eventToRemove.Count; i++)
        {
            m_theRouter.Remove(eventToRemove[i]);
        }
    }

    /// <summary>
    /// 处理增加监听器前的事项，检查 参数等
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "listenerBeingAdded"></param>
    private void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
    {
        if (!m_theRouter.ContainsKey(eventType))
        {
            m_theRouter.Add(eventType, null);
        }
        Delegate d = m_theRouter[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            Debugger.Log(eventType + "isnot sanme type,current is" + d.GetType() + "adding type" + listenerBeingAdded.GetType());
        }
    }

    /// <summary>
    /// 移除监听器之前的检查
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "listenerBeingRemoved"></param>
    public bool OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
    {
        if (!m_theRouter.ContainsKey(eventType))
        {
            return false;
        }
        Delegate d = m_theRouter[eventType];
        if (d != null && d.GetType() != listenerBeingRemoved.GetType())
        {
            Debugger.Log(eventType + "isnot sanme type,current is" + d.GetType() + "removing type" + listenerBeingRemoved.GetType());
        }
        return true;
    }

    /// <summary>
    /// 移除监听器之后处理，删除事件
    /// </summary>
    /// <param name = "eventType"></param>
    private void OnListenerRemoved(string eventType)
    {
        if (m_theRouter.ContainsKey(eventType) && m_theRouter[eventType] == null)
        {
            m_theRouter.Remove(eventType);
        }
    }

    #region 增加监听器
    /// <summary>
    /// 增加监听器，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void AddEventListener(string eventType, Action handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action)m_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void AddEventListener<T>(string eventType, Action<T> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<T>)m_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，带2参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void AddEventListener<T, U>(string eventType, Action<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<T, U>)m_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<T, U, V>)m_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，带4参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<T, U, V, W>)m_theRouter[eventType] + handler;
    }

    public void AddEventListener(string eventType, Action<object> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<object>)m_theRouter[eventType] + handler;
    }
    public void AddEventListener(string eventType, Action<object, object> handler)
    {
        OnListenerAdding(eventType, handler);
        m_theRouter[eventType] = (Action<object, object>)m_theRouter[eventType] + handler;
    }
    #endregion

    #region 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void RemoveEventListener(string eventType, Action handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void RemoveEventListener<T>(string eventType, Action<T> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action<T>)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，带2参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void RemoveEventListener<T, U>(string eventType, Action<T, U> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action<T, U>)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action<T, U, V>)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，带4参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action<T, U, V, W>)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    public void RemoveEventListener(string eventType, Action<object> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_theRouter[eventType] = (Action<object>)m_theRouter[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }
    #endregion

    #region
    /// <summary>
    /// 触发事件，不带参数触发
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void TriggerEvent(string eventType)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action callBack = callbacks[i] as Action;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack();
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }
    /// <summary>
    /// 触发事件，带1参数触发
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void TriggerEvent<T>(string eventType, T arg1)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T> callBack = callbacks[i] as Action<T>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }

    /// <summary>
    /// 触发事件，带2参数触发
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void TriggerEvent<T, U>(string eventType, T arg1, U arg2)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U> callBack = callbacks[i] as Action<T, U>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1, arg2);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }

    /// <summary>
    /// 触发事件，带3参数触发
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void TriggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V> callBack = callbacks[i] as Action<T, U, V>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }

    /// <summary>
    /// 触发事件，带4参数触发
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public void TriggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V, W> callBack = callbacks[i] as Action<T, U, V, W>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1, arg2, arg3, arg4);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }
    public void TriggerEvent(string eventType, object arg1)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<object> callBack = callbacks[i] as Action<object>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }
    public void TriggerEvent(string eventType, object arg1, object arg2)
    {
        Delegate d;
        if (!m_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<object, object> callBack = callbacks[i] as Action<object, object>;
            if (callBack.Target == null)
            {
                continue;
            }
            if (callBack == null)
            {

            }
            try
            {
                callBack(arg1, arg2);
            }
            catch (Exception ex)
            {
                Debugger.Log(ex + "");
            }
        }
    }
    #endregion
}
    /// <summary>
    /// 事件分发函数
    /// 提供事件注册，反注册，事件触发
    /// 支持0,1,2,3 等4中不同参数个数的回调函数
    /// </summary>
public class EventDispatcher
{
    private static EventController m_eventController = new EventController();
    public static Dictionary<string, Delegate> TheRouter
    {
        get
        {
            return m_eventController.TheRouter;
        }
    }

    /// <summary>
    /// 标记为永久注册事件
    /// </summary>
    /// <param name="eventType"></param>
    public static void MarkAsPermanent(string eventType)
    {
        m_eventController.MarkAsPermanent(eventType);
    }

    /// <summary>
    /// 清除非永久注册事件
    /// </summary>
    public static void Cleanup()
    {
        m_eventController.Cleanup();
    }

    #region
    /// <summary>
    /// 增加监听器，不带参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void AddEventListener(string eventType, Action handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void AddEventListener<T>(string eventType, Action<T> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，带2参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void AddEventListener<T, U>(string eventType, Action<T, U> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }

    public static void AddEventListener(string eventType, Action<object> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }
    public static void AddEventListener(string eventType, Action<object, object> handler)
    {
        m_eventController.AddEventListener(eventType, handler);
    }
    #endregion

    #region 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void RemoveEventListener(string eventType, Action handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void RemoveEventListener<T>(string eventType, Action<T> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，带2参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void RemoveEventListener<T, U>(string eventType, Action<T, U> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，带4参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }

    public static void RemoveEventListener(string eventType, Action<object> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }
    public static void RemoveEventListener(string eventType, Action<object, object> handler)
    {
        m_eventController.RemoveEventListener(eventType, handler);
    }
    #endregion

    #region 触发事件
    /// <summary>
    /// 触发事件，不带参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void TriggerEvent(string eventType)
    {
        m_eventController.TriggerEvent(eventType);
    }

    /// <summary>
    /// 触发事件，带1参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void TriggerEvent<T>(string eventType, T arg1)
    {
        m_eventController.TriggerEvent(eventType, arg1);
    }

    /// <summary>
    /// 触发事件，带2参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void TriggerEvent<T, U>(string eventType, T arg1, U arg2)
    {
        m_eventController.TriggerEvent(eventType, arg1, arg2);
    }

    /// <summary>
    /// 触发事件，带3参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void TriggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        m_eventController.TriggerEvent(eventType, arg1, arg2, arg3);
    }

    /// <summary>
    /// 触发事件，带4参数
    /// </summary>
    /// <param name = "eventType"></param>
    /// <param name = "handler"></param>
    public static void TriggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        m_eventController.TriggerEvent(eventType, arg1, arg2, arg3, arg4);
    }

    public static void TriggerEvent(string eventType, object arg1)
    {
        m_eventController.TriggerEvent(eventType, arg1);
    }
    public static void TriggerEvent(string eventType, object arg1, object arg2)
    {
        m_eventController.TriggerEvent(eventType, arg1, arg2);
    }
    #endregion
}