using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//条件节点
public class CoditionNode : BehaviorNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        ConditionNode();
    }
    //直接判断返回
    protected virtual void ConditionNode()
    {
        SetCallback(true);
    }
}