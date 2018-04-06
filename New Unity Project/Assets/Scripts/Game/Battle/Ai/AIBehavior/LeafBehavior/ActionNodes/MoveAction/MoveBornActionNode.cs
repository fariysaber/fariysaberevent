using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class MoveBornActionNode : ActionNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        (batentity.model as PlayerModel).GoToPosition(batentity.model.bornPos, null, null);
        SetCallback(true);
    }
}