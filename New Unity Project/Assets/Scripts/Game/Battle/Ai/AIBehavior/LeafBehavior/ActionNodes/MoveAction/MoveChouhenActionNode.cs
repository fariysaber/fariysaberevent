using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class MoveChouhenActionNode : ActionNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        BattlePlayerEntity chouhenentity = batentity.chouhenEntity;
        (batentity.model as PlayerModel).GoToPosition(chouhenentity.model.Pos, null, null);
        SetCallback(true);
    }
}