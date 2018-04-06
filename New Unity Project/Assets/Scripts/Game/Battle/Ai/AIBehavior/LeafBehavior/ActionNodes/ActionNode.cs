using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class ActionNode : BehaviorNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
    }
    public override void DestroyAll()
    {
        base.DestroyAll();
        EndAction();
    }
    protected virtual void EndAction()
    {
    }
}