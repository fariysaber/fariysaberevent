using System;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorLoad
{
    none = 0,
    start = 1,
    update = 2,
    destroy = 3,
}
public class BehaviorNode : MonoBehaviour
{
    public delegate void CallBehaviorBack(bool result);
    public CallBehaviorBack callback;
    public BehaviorLoad behaviorLoad = BehaviorLoad.none;
    [SerializeField, Tooltip("顺序选择节点专用")]
    public int waitTick = -1;//顺序选择节点专用
    protected object startdata;
    protected object initData;

    public bool isVicOrNot;
    public virtual void InitNode(object data)
    {
        initData = data;
    }
    public Entity entity
    {
        get { return (initData as BaseSceMoEntity).entity; }
    }
    public BattlePlayerEntity batentity
    {
        get { return (initData as BaseSceMoEntity).battlePlayerEntity; }
    }
    public List<GameObject> XunluoObj
    {
        get { return (initData as BaseSceMoEntity).xunluPosObj; }
    }
    public virtual void StartNode(object data, CallBehaviorBack becallback)
    {
        startdata = data;
        callback = becallback;
        behaviorLoad = BehaviorLoad.start;
    }
    protected virtual void StartUpdate()
    {
        behaviorLoad = BehaviorLoad.update;
    }
    public virtual void Tick(float dt)
    {
        if (behaviorLoad == BehaviorLoad.update)
        {
            OnUpdate(dt);
        }
    }
    public virtual void OnUpdate(float dt)
    {
    }
    public virtual void SetCallback(bool value,bool end = true)
    {
        callback(value);
        if (end)
        {
            End();
        }
        isVicOrNot = value;
    }
    public virtual void End()
    {
        DestroyAll();
    }
    public virtual void DestroyAll()
    {
        behaviorLoad = BehaviorLoad.destroy;
    }
}