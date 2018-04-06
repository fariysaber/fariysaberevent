using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class XunluoActionNode : ActionNode
{
    public int xunluoWeidian;
    public float waitTime = 3;
    public float waitTimecd = 3f;
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        if (XunluoObj == null || XunluoObj.Count <= 1)
        {
            SetCallback(false);
            return;
        }
        if (waitTime > 0)
        {
            SetCallback(true);
            return;
        }
        Vector3 pos = batentity.model.Pos;
        pos.y = 0;
        xunluoWeidian = xunluoWeidian % XunluoObj.Count;
        Vector3 xunPos = XunluoObj[xunluoWeidian].transform.position;
        xunPos.y = 0;
        if (Vector3.Distance(pos, xunPos) < 0.3f)
        {
            xunluoWeidian += 1;
            waitTime = waitTimecd;
        }
        else
        {
            (batentity.model as PlayerModel).GoToPosition(xunPos, null, null);
        }
        SetCallback(true);
    }
    public override void Tick(float dt)
    {
        if (waitTime > 0)
        {
            waitTime -= dt;
        }
        base.Tick(dt);
    }
}