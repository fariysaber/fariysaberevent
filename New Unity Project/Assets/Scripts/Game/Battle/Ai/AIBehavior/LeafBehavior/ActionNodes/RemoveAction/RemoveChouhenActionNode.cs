using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class RemoveChouhenActionNode : ActionNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        batentity.RemoveChouhen(false);
        SetCallback(true);
    }
}